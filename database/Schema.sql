
Create database SistemaDeInventario;

GO
USE SistemaDeInventario;
GO

CREATE TABLE dbo.Categorias (
    Id                INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Categorias PRIMARY KEY,
    Nombre            NVARCHAR(100)      NOT NULL CONSTRAINT UQ_Categorias_Nombre UNIQUE,
    Descripcion       NVARCHAR(500)      NOT NULL,
    Activo            BIT                NOT NULL CONSTRAINT DF_Categorias_Activo DEFAULT (1),
    FechaCreacion     DATETIME2          NOT NULL CONSTRAINT DF_Categorias_FechaCreacion DEFAULT (SYSDATETIME()),
    FechaModificacion DATETIME2          NULL,
    CONSTRAINT CK_Categorias_Nombre CHECK (LEN(LTRIM(RTRIM(Nombre))) >= 3)
);
GO

CREATE TABLE dbo.FotosCategoria (
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_FotosCategoria PRIMARY KEY,
    CategoriaId INT               NOT NULL,
    RutaArchivo NVARCHAR(400)     NOT NULL,
    CONSTRAINT FK_FotosCategoria_Categorias FOREIGN KEY (CategoriaId)
        REFERENCES dbo.Categorias (Id) ON DELETE CASCADE
);
CREATE INDEX IX_FotosCategoria_CategoriaId ON dbo.FotosCategoria (CategoriaId);
GO

CREATE TABLE dbo.Suplidores (
    Id                INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Suplidores PRIMARY KEY,
    NombreEmpresa     NVARCHAR(150)      NOT NULL CONSTRAINT UQ_Suplidores_NombreEmpresa UNIQUE,
    NombreContacto    NVARCHAR(100)      NOT NULL,
    Telefono          NVARCHAR(12)       NOT NULL,
    Correo            NVARCHAR(150)      NOT NULL,
    SitioWeb          NVARCHAR(200)      NULL,
    Activo            BIT                NOT NULL CONSTRAINT DF_Suplidores_Activo DEFAULT (1),
    FechaCreacion     DATETIME2          NOT NULL CONSTRAINT DF_Suplidores_FechaCreacion DEFAULT (SYSDATETIME()),
    FechaModificacion DATETIME2          NULL,
    CONSTRAINT CK_Suplidores_Telefono CHECK (
        Telefono LIKE '[8][0-9][0-9]-[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'
        AND LEFT(Telefono, 3) IN ('809', '829', '849'))
);
GO

CREATE TABLE dbo.Productos (
    Id                INT IDENTITY(1,1)  NOT NULL CONSTRAINT PK_Productos PRIMARY KEY,
    Nombre            NVARCHAR(150)      NOT NULL,
    PrecioUnitario    DECIMAL(18,2)      NOT NULL,
    CategoriaId       INT                NOT NULL,
    SuplidorId        INT                NOT NULL,
    Activo            BIT                NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT (1),
    FechaCreacion     DATETIME2          NOT NULL CONSTRAINT DF_Productos_FechaCreacion DEFAULT (SYSDATETIME()),
    FechaModificacion DATETIME2          NULL,
    CONSTRAINT CK_Productos_Precio CHECK (PrecioUnitario > 0),
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) REFERENCES dbo.Categorias (Id),
    CONSTRAINT FK_Productos_Suplidores FOREIGN KEY (SuplidorId)  REFERENCES dbo.Suplidores (Id)
);
CREATE INDEX IX_Productos_CategoriaId ON dbo.Productos (CategoriaId);
CREATE INDEX IX_Productos_SuplidorId  ON dbo.Productos (SuplidorId);
GO


