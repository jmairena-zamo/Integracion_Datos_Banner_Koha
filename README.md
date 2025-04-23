## Api Base

Proyecto base para la creación de un api REST.

**Tipo proyecto:** ASP.NET Core Web API

**Versión Framework:** .NET 9.0

### Paquetes NuGet básicos:

1.  Asp.Versioning.Mvc: Manejo de versiones de API, direcciones URL y métodos HTTP.
2.  AutoMapper: ayuda en la asignación de valores entre distintos modelos.
3.  Microsoft.AspNetCore.Authentication.JwtBearer: Permite acciones con Bearer Token.
4.  Microsoft.AspNetCore.JsonPatch: Permite realizar peticiones patch.
5.  Microsoft.AspNetCore.Mvc.NewtonsoftJson: Incluye formateadores de entrada y salida para JSON y JSON PATCH.
6.  Microsoft.EntityFrameworkCore.SqlServer: Proveedor de base de datos de Microsoft SQL Server para Entity Framework Core.
7.  Microsoft.EntityFrameworkCore.Tools: Herramientas principales de Entity Framework para la consola del administrador de paquetes NuGet.
8.  Microsoft.Windows.Compatibility: Brinda acceso a las API que anteriormente solo estaban disponibles para .NET Framework.
9.  NJsonSchema.NewtonsoftJson: Lector, generador y validador de esquemas JSON.
10. Serilog.AspNetCore: Habilita el servicio de logs.
11. Serilog.Sinks.MSSqlServer: Permite el registro de logs en SQL Server.
12. Swashbuckle.AspNetCore: Herramientas Swagger.

### Importar plantilla de Api Base

1.  Debe cerrar todos los proyectos de visual studio
2.  Descargar el archivo zip de la ruta: https://bitbucket.org/zamorano-projects/apibase/downloads/ApiBaseZamorano_v1.1.0.zip
3.  Mover el archivo a la su carpeta local: \Documents\Visual Studio {Version}\Templates\ProjectTemplates
4.  Crear el proyecto con la nueva plantilla

### Configurar appsettings.json:

1.  CodigoApiProject: código para identificar el proyecto (uniqueidentifier). Para crear un nuevo proyecto se puede utilizar la siguiente instrucción SQL:
``` sql
INSERT INTO ZamoWebApp.Seguridad.ApiProject
(
    CodigoApiProject,
    NombreApiProject,
    RutaAPI,
    ProductionKey,
    DevelopmentKey
)
VALUES
(   NEWID(),                  -- CodigoApiProject - uniqueidentifier
    'Feature',                -- NombreApiProject - varchar(256)
    '/feature',               -- RutaAPI - varchar(256)
    CONCAT(NEWID(), NEWID()), -- ProductionKey - varchar(max)
    CONCAT(NEWID(), NEWID())  -- DevelopmentKey - varchar(max)
);
```
2.  ConnectionStrings: agregar las conexiones a utilizar. Ejemplo: NombreBaseDatosConnectionString. **Se registra la conexión "ZamoWebAppConnectionString" para realizar registros en los logs.**

### Crear o actualizar modelos y DBContext

Cada base de datos debe tener su propio DBContext. Para ejecutar estos comandos se debe utilizar la Package Manager Console (PM>) de Visual Studio. Ejemplos:

#### tablas
```plaintext
Scaffold-DbContext "Name=ConnectionStrings:ZamoWebAppConnectionString" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/DB/ZamoWebApp -UseDatabaseNames -ContextDir Context -Tables 'dbo.Tbl_test_Estudiantes' -Force
```

#### Vistas
```
Scaffold-DbContext "Name=ConnectionStrings:integracionConnectionString" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/DB/Integracion -UseDatabaseNames -ContextDir Context -Tables 'RRHH.vw_GeneralEmpleados' -Force
```

#### Esquemas
```
Scaffold-DbContext "Name=ConnectionStrings:ZamoWebAppConnectionString" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/DB/ZamoWebApp -UseDatabaseNames -ContextDir Context -Schema Clinica -Force
```

### Configurar Program.cs

1.  Para utilizar un DBContext o una interfaz se debe declarar en el apartado "DbContext" e "Interface". 

### Pasos para crear un controlador:

1.  Crear los DTO (Data Transfer Object) de entrada y salida (Models/DTOs/**Feature**): contiene los datos que el usuario debe ingresar (obligatorios o no) y los datos de se expondrán cuando se haga la consulta.
2.  Crear un perfil para AutoMapper (Profiles/**Feature**): contiene las equivalencias válidas para las propiedades de los modelos y tablas.
3.  Crear una interfaz (Services/**Feature**/Interfaces): contiene los métodos que se usaran en el servicio y para la inyección de dependencias.
4.  Crear el servicio y heredar su interfaz (Services/**Feature**): contiene la lógica del negocio.
5.  Crear un controlador (Controllers/**Feature**/v{version}): gestión de endpoints, llamadas a los servicios y manejo de versiones.

## Notas

### Generar un nuevo Git Ignore
```
dotnet new gitignore
```

### Uso de APIKEY 

- Se debera de realizar la configuracion de los roles, ya que se define el rol necesario en el endpoint y se asigna en el portal.

