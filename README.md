# Sistema Integral de Seguros

Proyecto de consola desarrollado en C# con .NET 10 para gestionar clientes, ramos, pólizas, siniestros, reaseguros y asientos contables.

## Requisitos

- Windows con [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
- SQL Server con autenticación integrada de Windows.
- SQL Server Management Studio o una herramienta equivalente para ejecutar el script SQL.

## Preparar la base de datos

1. Abre `ProyectoFinal/BaseDatos/CrearBaseDatos.sql` en SQL Server Management Studio.
2. Conéctate a la instancia de SQL Server.
3. Ejecuta el script completo. Este crea la base de datos `SistemaSegurosDB` y sus tablas.
4. La aplicación tiene configurado el servidor SQL `xJors` en `ProyectoFinal/ConexionBaseDatos.cs`. La instancia utilizada debe ser accesible con ese nombre. Si el equipo usa otro nombre de servidor, se debe ajustar el valor de `Ja_servidor` antes de ejecutar.

## Clonar y ejecutar

```powershell
git clone https://github.com/xJorss/Proyecto-Intregrador-.git
cd Proyecto-Intregrador-
dotnet restore ProyectoFinal.slnx
dotnet build ProyectoFinal.slnx
dotnet run --project ProyectoFinal/ProyectoFinal.csproj
```

También se puede abrir `ProyectoFinal.slnx` directamente en Visual Studio y ejecutar el proyecto `ProyectoFinal`.

## Estructura principal

```text
ProyectoFinal.slnx
ProyectoFinal/
├── ProyectoFinal.csproj
├── Program.cs
├── Entidades.cs
├── Repositorios.cs
├── ConexionBaseDatos.cs
├── BaseDatos/
│   └── CrearBaseDatos.sql
└── Datos/
    ├── clientes.csv
    └── ramos.csv
```

Las carpetas `bin` y `obj` no se incluyen en el repositorio porque se regeneran automáticamente al restaurar y compilar el proyecto.
