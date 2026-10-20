-- =====================================================
--  SistemaInventario - Script de creación de la base de datos (SQL Server)
-- =====================================================
IF DB_ID('SistemaInventario') IS NULL
    CREATE DATABASE SistemaInventario;
GO
USE SistemaInventario;
GO

-- Se borran las tablas (si existen) para poder ejecutar el script varias veces
IF OBJECT_ID('Productos') IS NOT NULL DROP TABLE Productos;
IF OBJECT_ID('CategoriaFotos') IS NOT NULL DROP TABLE CategoriaFotos;
IF OBJECT_ID('Categorias') IS NOT NULL DROP TABLE Categorias;
IF OBJECT_ID('Suplidores') IS NOT NULL DROP TABLE Suplidores;
IF OBJECT_ID('Usuarios') IS NOT NULL DROP TABLE Usuarios;
GO

CREATE TABLE Usuarios (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario     NVARCHAR(50)  NOT NULL UNIQUE,
    ClaveHash         VARCHAR(64)   NOT NULL,          -- SHA-256 en hexadecimal
    NombreCompleto    NVARCHAR(100) NOT NULL,
    Activo            BIT           NOT NULL DEFAULT 1,
    FechaCreacion     DATETIME      NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME      NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Categorias (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion       NVARCHAR(500) NOT NULL,
    Activo            BIT           NOT NULL DEFAULT 1,
    FechaCreacion     DATETIME      NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME      NOT NULL DEFAULT GETDATE()
);

CREATE TABLE CategoriaFotos (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    CategoriaId INT           NOT NULL,
    Ruta        NVARCHAR(500) NOT NULL,
    CONSTRAINT FK_CategoriaFotos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);

CREATE TABLE Suplidores (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    Empresa           NVARCHAR(100) NOT NULL UNIQUE,
    Contacto          NVARCHAR(100) NOT NULL,
    Telefono          VARCHAR(12)   NOT NULL,
    Correo            NVARCHAR(150) NOT NULL,
    SitioWeb          NVARCHAR(200) NULL,
    Activo            BIT           NOT NULL DEFAULT 1,
    FechaCreacion     DATETIME      NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME      NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Productos (
    Id                INT IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(100)  NOT NULL,
    Precio            DECIMAL(18,2)  NOT NULL CHECK (Precio > 0),
    CategoriaId       INT            NOT NULL,
    SuplidorId        INT            NOT NULL,
    Activo            BIT            NOT NULL DEFAULT 1,
    FechaCreacion     DATETIME       NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id),
    CONSTRAINT FK_Productos_Suplidores FOREIGN KEY (SuplidorId)  REFERENCES Suplidores(Id)
);
GO
