using ApiBase.Auth.Authentication;
using ApiBase.Auth.Authorization;
using ApiBase.Auth.Authorization.Attributes;
using ApiBase.Auth.Authorization.Handlers;
using ApiBase.Context;
using ApiBase.Filters.Action;
using ApiBase.Middleware;
using ApiBase.MyLogs;
using ApiBase.Services;
using ApiBase.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.CustomRequiredRoles("ADMINISTRADOR"));
        options.AddPolicy("User", policy => policy.CustomRequiredRoles("USUARIO", "ADMINISTRADOR"));
    });
    builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddleware>();
    builder.Services.AddSingleton<IAuthorizationHandler, RolesHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, ClaimHandler>();

    //------- Filtros Globales
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
    builder.Services.AddControllers(config =>
    {
        config.Filters.Add(new ValidationModelAttribute());
    });

    //------- Servicios de Versiones
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    }).AddMvc();

    //------- AutoMapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    //------- DbContext
    builder.Services.AddDbContext<ZamoWebAppContext>();

    //------- Interface
    builder.Services.AddTransient<IAuthServices, AuthServices>();
    
    builder.Services.AddTransient<IEjemploService, EjemploService>();

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

    //------- Ruta Inicial
    string rutaAPI = builder.Configuration["RutaAPI"]!;
    app.UsePathBase(rutaAPI);
    app.Use((context, next) =>
    {
        context.Request.PathBase = rutaAPI;
        return next();
    });

    //------- Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //------- Middlewares
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}