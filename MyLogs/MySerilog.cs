using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace Integracion_Datos_Banner_Koha.MyLogs
{
    public static class MySerilog
    {
        public static Logger GetInstance()
        {
            //Instancia de appSettings del proyecto
            IConfiguration appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

            //Variables de configuración
            string connectionString = appSettings.GetConnectionString("ZamoWebAppConnectionString")!;
            string codigoApiProject = appSettings.GetSection("CodigoApiProject").Value!;
            LogEventLevel logEventLevel = LogEventLevel.Warning;
            ColumnOptions columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn("RequestId", SqlDbType.VarChar, dataLength:256 ),
                    new SqlColumn("IpUser", SqlDbType.VarChar, dataLength:256 ),
                    new SqlColumn("CodigoApiProject", SqlDbType.UniqueIdentifier)
                }
            };

            //Retorna la configuración de Serilog
            return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", logEventLevel)
            .Enrich.WithProperty("CodigoApiProject", codigoApiProject)
            .Enrich.FromLogContext()
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", SchemaName = "Seguridad", AutoCreateSqlTable = false },
                restrictedToMinimumLevel: logEventLevel,
                columnOptions: columnOptions
            )
            .CreateLogger();
        }

    }
}
