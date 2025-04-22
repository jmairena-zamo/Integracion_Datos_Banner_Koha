## Api Base

Proyecto base para la creación de un api REST.

**Tipo proyecto:** ASP.NET Core Web API

**Versión Framework:** .NET 7.0

### Paquetes NuGet básicos:

1.  Asp.Versioning.Mvc: Manejo de versiones de API, direcciones URL y métodos HTTP.
2.  AutoMapper.Extensions.Microsoft.DependencyInjection: ayuda en la asignación de valores entre los modelos y los DTOs.
3.  Microsoft.AspNetCore.Authentication.JwtBearer: Permite acciones con Bearer Token.
4.  Microsoft.AspNetCore.JsonPatch: Permite realizar peticiones patch.
5.  Microsoft.AspNetCore.Mvc.NewtonsoftJson: Incluye formateadores de entrada y salida para JSON y JSON PATCH.
6.  Microsoft.EntityFrameworkCore.SqlServer: Proveedor de base de datos de Microsoft SQL Server para Entity Framework Core.
7.  Microsoft.EntityFrameworkCore.Tools: Herramientas principales de Entity Framework para la consola del administrador de paquetes NuGet.
8.  Microsoft.Windows.Compatibility: Brinda acceso a las API que anteriormente solo estaban disponibles para .NET Framework.
9.  Serilog.AspNetCore: Habilita el servicio de logs
10. Serilog.Sinks.MSSqlServer: Permite el registro de logs en SQL Server

### Importar plantilla de Api Base

1.  Debe cerrar todos los proyectos de visual studio
2.  Descargar el archivo zip de la ruta: https://bitbucket.org/zamorano-projects/apibase/downloads/ApiBaseZamorano_v1.0.1.zip
3.  Mover el archivo a la su carpeta local: \Documents\Visual Studio {Version}\Templates\ProjectTemplates
4.  Crear el proyecto con la nueva plantilla

### Configurar appsettings.json:

1.  RutaAPI: agregar la ruta del api. Ejemplo: https://api.zamorano.edu /**auth**
2.  CodigoApiProject: código para identificar el proyecto (uniqueidentifier). Para crear un nuevo proyecto se puede utilizar la siguiente instrucción SQL:
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
    'Auth',                   -- NombreApiProject - varchar(256)
    '/auth',                  -- RutaAPI - varchar(256)
    CONCAT(NEWID(), NEWID()), -- ProductionKey - varchar(max)
    CONCAT(NEWID(), NEWID())  -- DevelopmentKey - varchar(max)
);
```
3.  AuthUrl: url utilizada para la integración de la seguridad personalizada
4.  ConnectionStrings: agregar las conexiones a utilizar. Ejemplo: NombreBaseDatosConnectionString. **Se registra la conexión "ZamoWebAppConnectionString" para realizar registros en los logs.**

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

1.  Crear los DTO (Data Transfer Object) de entrada y salida (Models/DTOs): contiene los datos que el usuario debe ingresar (obligatorios o no) y los datos de se expondrán cuando se haga la consulta.
2.  Crear un perfil para AutoMapper (Profiles): contiene los tipos de equivalencias válidas para los modelos y tablas.
3.  Crear una interfaz (Services/Interfaces): contiene los métodos que se usaran en el servicio y para la inyección de dependencias.
4.  Crear el servicio y heredar su interfaz (Services): contiene la lógica del negocio.
5.  Crear un controlador (Controllers/v{version}): gestión de endpoints, llamadas a los servicios y manejo de versiones.

### Eliminar los archivos de ejemplo
Puede eliminar los archivos utilizados de ejemplo con el siguiente comando en Powershell:
```
Remove-Item Models/DTOs/Input/EjemploInputDto.cs & Remove-Item Models/DTOs/Output/EjemploDto.cs & Remove-Item Models/DB/Tbl_test_Estudiante.cs & Remove-Item Profiles/EjemploProfile.cs & Remove-Item Services/Interfaces/IEjemploService.cs & Remove-Item Services/EjemploService.cs & Remove-Item Controllers/v1/EjemploController.cs & Remove-Item Context/ZamoWebAppContext.cs
```

## Notas

### Generar Git Ignore
```
dotnet new gitignore
```

### Tablas utilizadas en el ejemplo dev-02

``` sql
USE ZamoWebApp;
GO

CREATE TABLE Tbl_test_Estudiantes
(
    [Id] [INT] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CodigoEstudiante] [VARCHAR](10) NOT NULL,
    [Nombres] [VARCHAR](256) NOT NULL,
    [Apellidos] [VARCHAR](256) NOT NULL,
    [Telefono] [VARCHAR](64) NULL,
    [CodigoEstado] [VARCHAR](3) NOT NULL,
    [UsuarioCreador] [VARCHAR](128) NOT NULL,
    [FechaCreador] [DATETIME] NOT NULL,
    [UsuarioModifica] [VARCHAR](128) NULL,
    [FechaModifica] [DATETIME] NULL,
);
```

### Uso de APIKEY 

- Se debera de realizar la configuracion de los roles, ya que se define el rol necesario en el endpoint y se asigna en el portal.

