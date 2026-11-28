IF DB_ID('NexusDB') IS NULL
BEGIN
    CREATE DATABASE NexusDB;
END
GO

USE NexusDB;
GO

-- 1. Tabla principal de Cadetes (Jugadores)
IF OBJECT_ID('Cadetes', 'U') IS NULL
BEGIN
    CREATE TABLE Cadetes (
        CadeteID INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(60) NOT NULL UNIQUE,
        Edad INT NOT NULL DEFAULT 18,
        Realidad NVARCHAR(80) NOT NULL DEFAULT 'Neo Cochabamba',
        Salud INT NOT NULL DEFAULT 100,
        Energia INT NOT NULL DEFAULT 80,
        Estabilidad INT NOT NULL DEFAULT 85,
        Peligro INT NOT NULL DEFAULT 18,
        Escudo INT NOT NULL DEFAULT 20,
        XP INT NOT NULL DEFAULT 0,
        ModuloAtaque INT NOT NULL DEFAULT 15,
        DatosIris INT NOT NULL DEFAULT 0,
        NivelDesbloqueado INT NOT NULL DEFAULT 1,
        TotalRescatados INT NOT NULL DEFAULT 0,
        UltimaConexion DATETIME NOT NULL DEFAULT GETDATE()
    );
END
ELSE
BEGIN
    IF COL_LENGTH('Cadetes', 'NivelDesbloqueado') IS NULL
        ALTER TABLE Cadetes ADD NivelDesbloqueado INT NOT NULL DEFAULT 1;
    IF COL_LENGTH('Cadetes', 'TotalRescatados') IS NULL
        ALTER TABLE Cadetes ADD TotalRescatados INT NOT NULL DEFAULT 0;
END
GO

-- 2. Tabla de Inventario persistente por Cadete
IF OBJECT_ID('InventarioCadete', 'U') IS NULL
BEGIN
    CREATE TABLE InventarioCadete (
        ItemID INT IDENTITY(1,1) PRIMARY KEY,
        CadeteID INT NOT NULL FOREIGN KEY REFERENCES Cadetes(CadeteID) ON DELETE CASCADE,
        NombreObjeto NVARCHAR(80) NOT NULL,
        TipoObjeto NVARCHAR(40) NOT NULL,
        Valor INT NOT NULL,
        FechaObtencion DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 3. Tabla de Bitácora de Misiones / Sectores completados
IF OBJECT_ID('BitacoraMisiones', 'U') IS NULL
BEGIN
    CREATE TABLE BitacoraMisiones (
        MisionID INT IDENTITY(1,1) PRIMARY KEY,
        CadeteID INT NOT NULL FOREIGN KEY REFERENCES Cadetes(CadeteID) ON DELETE CASCADE,
        Sector NVARCHAR(80) NOT NULL,
        Rescatados INT NOT NULL DEFAULT 0,
        PuntuacionObtenida INT NOT NULL DEFAULT 0,
        Resultado NVARCHAR(40) NOT NULL,
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 4. Tabla de Historial de Acciones (Terminal SQL)
IF OBJECT_ID('HistorialAcciones', 'U') IS NULL
BEGIN
    CREATE TABLE HistorialAcciones (
        HistorialID INT IDENTITY(1,1) PRIMARY KEY,
        CadeteID INT NOT NULL FOREIGN KEY REFERENCES Cadetes(CadeteID) ON DELETE CASCADE,
        NombreCadete NVARCHAR(60) NOT NULL,
        TipoAccion NVARCHAR(50) NOT NULL,
        Detalle NVARCHAR(250) NOT NULL,
        FechaHora DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 5. Tabla de Estadísticas de Combate contra Entidades
IF OBJECT_ID('EstadisticasCombate', 'U') IS NULL
BEGIN
    CREATE TABLE EstadisticasCombate (
        CombateID INT IDENTITY(1,1) PRIMARY KEY,
        CadeteID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Cadetes(CadeteID) ON DELETE CASCADE,
        DronesEliminados INT NOT NULL DEFAULT 0,
        AliensEliminados INT NOT NULL DEFAULT 0,
        JefesDerrotados INT NOT NULL DEFAULT 0,
        DanioTotalInfligido INT NOT NULL DEFAULT 0
    );
END
GO