using Asp.Versioning;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Integracion_Datos_Banner_Koha.Auth.Authentication;
using Integracion_Datos_Banner_Koha.Auth.Authorization;
using Integracion_Datos_Banner_Koha.Auth.Authorization.Handlers;
using Integracion_Datos_Banner_Koha.Auth.Authorization.Requirements;
using Integracion_Datos_Banner_Koha.Context;
using Integracion_Datos_Banner_Koha.Extensions;
using Integracion_Datos_Banner_Koha.Filters.Action;
using Integracion_Datos_Banner_Koha.Hangfire.Filters;
using Integracion_Datos_Banner_Koha.Middleware;
using Integracion_Datos_Banner_Koha.MyLogs;
using Integracion_Datos_Banner_Koha.Services.Auth;
using Integracion_Datos_Banner_Koha.Services.Auth.Interfaces;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;
using Integracion_Datos_Banner_Koha.Services.Jobs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.IdentityModel.Tokens.Jwt;


// Configuracion Global Logs
Log.Logger = MySerilog.GetInstance();

try
{
    //____________________________________________ Configuración de Servicios
    var builder = WebApplication.CreateBuilder(args);

    //------- Servicios de Logs
    builder.Host.UseSerilog();

    //------- Servicios de Autenticación
    builder.Services.AddAuthentication(CustomAuthenticationOptions.DefaultScheme)
        .AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>(CustomAuthenticationOptions.DefaultScheme, options => { });

    //------- Servicios de Autorización y Políticas
    builder.Services.AddAuthentication("TOKEN_APIKEY")
     .AddCookie("CookieAuth", options =>
     {
         options.Cookie.Name = "token";
         options.LoginPath = "/auth/login-hangfire";
         options.Cookie.HttpOnly = true;
         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         options.Cookie.SameSite = SameSiteMode.Lax;

         options.SlidingExpiration = true;
     })
     .AddPolicyScheme("TOKEN_APIKEY", "Token-ApiKey", options =>
     {
         options.ForwardDefaultSelector = context =>
         {
             // Si la petición viene de la UI de Hangfire, usa Cookies
             if (context.Request.Path.StartsWithSegments("/hangfire")) return "CookieAuth";
             if (context.Request.Headers.ContainsKey("x-api-key")) return "APIKEY";
             return "TOKEN";
         };
     })
     .AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>("TOKEN", options => { })
     .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("APIKEY", options => { });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.CustomRequiredRoles("ADMINISTRADOR"));
        options.AddPolicy("User", policy => policy.CustomRequiredRoles("USUARIO", "ADMINISTRADOR"));
    });

    builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddleware>();
    builder.Services.AddSingleton<IAuthorizationHandler, RolesHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, ClaimHandler>();

    //------- Filtros Globales
    builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
    builder.Services.AddControllers(config => { config.Filters.Add(new ValidationModelAttribute()); });
    builder.Services.AddRazorPages();

    //------- Servicios de Versiones
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    }).AddMvc();

    //------- AutoMapper
    builder.Services.AddAutoMapper(cfg =>
    {

    }, AppDomain.CurrentDomain.GetAssemblies());

    //------- Jobs Hangfire
    builder.Services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(
            builder.Configuration.GetConnectionString("BibliotecaConnectionString"),
            new SqlServerStorageOptions()
            {
                SchemaName = "BANNER-KOHA",
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

    builder.Services.AddHangfireServer(options =>
    {
        options.Queues = [ "default", "bannerkoha" ];
        options.ServerName = "bannerkoha";
    });

    //------- DbContext
    builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaConnectionString")));

    //------- Interface
    builder.Services.AddTransient<IAuthServices, AuthServices>();
    builder.Services.AddHttpClient<IBannerSyncService, BannerSyncService>(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(30);
    });
    builder.Services.AddHttpClient<IKohaSyncService, KohaSyncService>(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(60);
    });
    builder.Services.AddScoped<IAccionesService, AccionesService>();
    //// Add this line with your other service registrations
    builder.Services.AddScoped<HangfireDashboardAuthFilter>();
    builder.Services.AddScoped<RestrictDeleteJobsFilter>();

    //------- Jobs Scheduler
    builder.Services.AddScoped<JobScheduler>();

    //------- Servicios de la aplicacion
    builder.Services.AddApplicationServices();

    //------- Evitar que se cambien los nombres de los claim
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    //------- Permitir referencias en los include
    builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

    //------- Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //____________________________________________ Configuración de la APP
    var app = builder.Build();
    app.UseSerilogRequestLogging();

    //------- Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //------- Middlewares
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapRazorPages();

    //------- Dashboard Hangfire
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        DashboardTitle = "Administración de Jobs",
        Authorization = new IDashboardAuthorizationFilter[] { app.Services.GetRequiredService<HangfireDashboardAuthFilter>(), new RestrictDeleteJobsFilter() },
        IgnoreAntiforgeryToken = true
    });

    //------- Ejecutar los Jobs
    using (var scope = app.Services.CreateScope())
    {
        var scheduler = scope.ServiceProvider.GetRequiredService<JobScheduler>();
        scheduler.RegisterJobs();
    };

    app.Run();
}

catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
} finally
{
    Log.CloseAndFlush();
}