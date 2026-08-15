IF DB_ID('SistemaSegurosDB') IS NULL
BEGIN
    EXEC('CREATE DATABASE SistemaSegurosDB');
END;
GO

USE SistemaSegurosDB;
GO

IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        IdCliente INT NOT NULL,
        Cedula VARCHAR(10) NOT NULL,
        Nombre VARCHAR(100) NOT NULL,
        AlertasUAF VARCHAR(100) NOT NULL,
        CONSTRAINT PK_Clientes PRIMARY KEY (IdCliente),
        CONSTRAINT UQ_Clientes_Cedula UNIQUE (Cedula)
    );
END;
GO

IF OBJECT_ID('dbo.Ramos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Ramos
    (
        IdRamo INT NOT NULL,
        Nombre VARCHAR(100) NOT NULL,
        Descripcion VARCHAR(200) NOT NULL,
        Activo BIT NOT NULL,
        CONSTRAINT PK_Ramos PRIMARY KEY (IdRamo),
        CONSTRAINT UQ_Ramos_Nombre UNIQUE (Nombre)
    );
END;
GO

IF OBJECT_ID('dbo.Logs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Logs
    (
        IdLog INT NOT NULL,
        Fecha DATETIME2 NOT NULL,
        Modulo VARCHAR(30) NOT NULL,
        Tipo VARCHAR(30) NOT NULL,
        Mensaje VARCHAR(500) NOT NULL,
        Usuario VARCHAR(100) NOT NULL,
        CONSTRAINT PK_Logs PRIMARY KEY (IdLog),
        CONSTRAINT CK_Logs_Modulo CHECK
        (
            Modulo IN
            (
                'Poliza', 'Siniestro', 'Reaseguro',
                'Contabilidad', 'Sistema', 'Archivos', 'BaseDatos'
            )
        ),
        CONSTRAINT CK_Logs_Tipo CHECK
        (
            Tipo IN ('Informacion', 'Advertencia', 'Error')
        )
    );
END;
GO

IF OBJECT_ID('dbo.Polizas', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Polizas
    (
        IdPoliza INT NOT NULL,
        IdCliente INT NOT NULL,
        IdRamo INT NOT NULL,
        CapitalAsegurado DECIMAL(18, 2) NOT NULL,
        CapitalRemanente DECIMAL(18, 2) NOT NULL,
        TasaRiesgo DECIMAL(10, 4) NOT NULL,
        PrimaBase DECIMAL(18, 2) NOT NULL,
        SuperBancos DECIMAL(18, 2) NOT NULL,
        SeguroCampesino DECIMAL(18, 2) NOT NULL,
        DerechoEmision DECIMAL(18, 2) NOT NULL,
        Subtotal DECIMAL(18, 2) NOT NULL,
        IVA DECIMAL(18, 2) NOT NULL,
        PrimaTotal DECIMAL(18, 2) NOT NULL,
        FechaEmision DATETIME2 NOT NULL,
        Estado VARCHAR(20) NOT NULL,
        CONSTRAINT PK_Polizas PRIMARY KEY (IdPoliza),
        CONSTRAINT FK_Polizas_Clientes FOREIGN KEY (IdCliente)
            REFERENCES dbo.Clientes (IdCliente),
        CONSTRAINT FK_Polizas_Ramos FOREIGN KEY (IdRamo)
            REFERENCES dbo.Ramos (IdRamo),
        CONSTRAINT CK_Polizas_CapitalAsegurado CHECK
            (CapitalAsegurado > 0),
        CONSTRAINT CK_Polizas_CapitalRemanente CHECK
            (CapitalRemanente >= 0),
        CONSTRAINT CK_Polizas_CapitalRemanenteAsegurado CHECK
            (CapitalRemanente <= CapitalAsegurado),
        CONSTRAINT CK_Polizas_TasaRiesgo CHECK
            (TasaRiesgo > 0 AND TasaRiesgo <= 100),
        CONSTRAINT CK_Polizas_Estado CHECK
            (Estado IN ('Activa', 'Inactiva'))
    );
END;
GO

IF OBJECT_ID('dbo.Siniestros', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Siniestros
    (
        IdSiniestro INT NOT NULL,
        IdPoliza INT NOT NULL,
        MontoReclamo DECIMAL(18, 2) NOT NULL,
        PorcentajeDeducible DECIMAL(10, 4) NOT NULL,
        ValorDeducible DECIMAL(18, 2) NOT NULL,
        PagoNeto DECIMAL(18, 2) NOT NULL,
        CapitalConsumido DECIMAL(18, 2) NOT NULL,
        FechaSiniestro DATETIME2 NOT NULL,
        Estado VARCHAR(20) NOT NULL,
        Observacion VARCHAR(300) NOT NULL,
        CONSTRAINT PK_Siniestros PRIMARY KEY (IdSiniestro),
        CONSTRAINT FK_Siniestros_Polizas FOREIGN KEY (IdPoliza)
            REFERENCES dbo.Polizas (IdPoliza),
        CONSTRAINT CK_Siniestros_MontoReclamo CHECK
            (MontoReclamo > 0),
        CONSTRAINT CK_Siniestros_PorcentajeDeducible CHECK
            (PorcentajeDeducible >= 0 AND PorcentajeDeducible <= 100),
        CONSTRAINT CK_Siniestros_ValorDeducible CHECK
            (ValorDeducible >= 0),
        CONSTRAINT CK_Siniestros_PagoNeto CHECK
            (PagoNeto >= 0),
        CONSTRAINT CK_Siniestros_CapitalConsumido CHECK
            (CapitalConsumido >= 0),
        CONSTRAINT CK_Siniestros_Estado CHECK
            (Estado IN ('Aprobado', 'Rechazado'))
    );
END;
GO

IF OBJECT_ID('dbo.Reaseguros', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Reaseguros
    (
        IdReaseguro INT NOT NULL,
        IdPoliza INT NOT NULL,
        MontoRetencion DECIMAL(18, 2) NOT NULL,
        MontoContrato DECIMAL(18, 2) NOT NULL,
        MontoFacultativo DECIMAL(18, 2) NOT NULL,
        TotalRepartido DECIMAL(18, 2) NOT NULL,
        IndiceRetencion INT NOT NULL,
        IndiceContrato INT NOT NULL,
        IndiceFacultativo INT NOT NULL,
        FechaGeneracion DATETIME2 NOT NULL,
        Generado BIT NOT NULL,
        CONSTRAINT PK_Reaseguros PRIMARY KEY (IdReaseguro),
        CONSTRAINT FK_Reaseguros_Polizas FOREIGN KEY (IdPoliza)
            REFERENCES dbo.Polizas (IdPoliza),
        CONSTRAINT CK_Reaseguros_MontoRetencion CHECK
            (MontoRetencion >= 0),
        CONSTRAINT CK_Reaseguros_MontoContrato CHECK
            (MontoContrato >= 0),
        CONSTRAINT CK_Reaseguros_MontoFacultativo CHECK
            (MontoFacultativo >= 0),
        CONSTRAINT CK_Reaseguros_TotalRepartido CHECK
            (TotalRepartido >= 0),
        CONSTRAINT CK_Reaseguros_IndiceRetencion CHECK
            (IndiceRetencion >= -1),
        CONSTRAINT CK_Reaseguros_IndiceContrato CHECK
            (IndiceContrato >= -1),
        CONSTRAINT CK_Reaseguros_IndiceFacultativo CHECK
            (IndiceFacultativo >= -1)
    );
END;
GO

IF OBJECT_ID('dbo.AsientosContables', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AsientosContables
    (
        IdAsiento INT NOT NULL,
        TipoOperacion VARCHAR(30) NOT NULL,
        CuentaDebe VARCHAR(100) NOT NULL,
        CuentaHaber VARCHAR(100) NOT NULL,
        Valor DECIMAL(18, 2) NOT NULL,
        Fecha DATETIME2 NOT NULL,
        IdPoliza INT NULL,
        IdSiniestro INT NULL,
        Descripcion VARCHAR(300) NOT NULL,
        Estado VARCHAR(20) NOT NULL,
        CONSTRAINT PK_AsientosContables PRIMARY KEY (IdAsiento),
        CONSTRAINT FK_AsientosContables_Polizas FOREIGN KEY (IdPoliza)
            REFERENCES dbo.Polizas (IdPoliza),
        CONSTRAINT FK_AsientosContables_Siniestros FOREIGN KEY (IdSiniestro)
            REFERENCES dbo.Siniestros (IdSiniestro),
        CONSTRAINT CK_AsientosContables_TipoOperacion CHECK
            (TipoOperacion IN ('Emision', 'Siniestro', 'Reaseguro')),
        CONSTRAINT CK_AsientosContables_Valor CHECK
            (Valor >= 0),
        CONSTRAINT CK_AsientosContables_Estado CHECK
            (Estado IN ('Registrado', 'Anulado'))
    );
END;
GO
