
USE SistemaInventario;
GO

-- Configure esta variable solo en su copia local; no publique su contraseña.
DECLARE @ClaveInicial NVARCHAR(128) = NULL;
IF NULLIF(@ClaveInicial, N'') IS NOT NULL
BEGIN
    INSERT INTO Usuarios (NombreUsuario, ClaveHash, NombreCompleto)
    VALUES ('admin', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @ClaveInicial), 2), N'Administrador del Sistema');
END
ELSE
    PRINT N'No se creó el usuario admin. Configure @ClaveInicial localmente para crearlo.';

INSERT INTO Categorias (Nombre, Descripcion) VALUES
(N'Bebidas',        N'Refrescos, jugos, agua y otras bebidas'),
(N'Lácteos',        N'Leche, quesos, yogures y mantequilla'),
(N'Limpieza',       N'Productos para la limpieza del hogar'),
(N'Tecnología',     N'Accesorios y equipos electrónicos'),
(N'Papelería',      N'Artículos de oficina y escolares');

INSERT INTO Suplidores (Empresa, Contacto, Telefono, Correo, SitioWeb) VALUES
(N'Distribuidora Caribe',  N'Juan Pérez',     '809-555-1234', 'ventas@dcaribe.com.do',  'https://www.dcaribe.com.do'),
(N'Lácteos del Este',      N'María Gómez',    '829-555-2345', 'maria@lacteoseste.com',  NULL),
(N'Limpieza Total SRL',    N'Carlos Rodríguez', '849-555-3456', 'info@limpiezatotal.com', 'https://limpiezatotal.com'),
(N'TecnoRD',               N'Ana Martínez',   '809-555-4567', 'contacto@tecnord.com',   'https://www.tecnord.com'),
(N'Papelera Nacional',     N'Luis Fernández', '829-555-5678', 'luis@papelnacional.do',  NULL);


INSERT INTO Productos (Nombre, Precio, CategoriaId, SuplidorId) VALUES
(N'Agua botella 500ml',        25.00, 1, 1),
(N'Refresco cola 2L',          95.50, 1, 1),
(N'Jugo de naranja 1L',       110.00, 1, 1),
(N'Cerveza lata 355ml',        75.00, 1, 1),
(N'Leche entera 1L',           85.00, 2, 2),
(N'Queso blanco 1 lb',        210.00, 2, 2),
(N'Yogurt natural 500g',      120.00, 2, 2),
(N'Mantequilla 250g',         140.00, 2, 2),
(N'Detergente líquido 1L',    165.00, 3, 3),
(N'Cloro 1 galón',            130.00, 3, 3),
(N'Desinfectante 900ml',      115.00, 3, 3),
(N'Esponja de cocina (3 uds)', 60.00, 3, 3),
(N'Audífonos inalámbricos', 1850.00, 4, 4),
(N'Cargador USB-C 20W',       950.00, 4, 4),
(N'Mouse óptico',             550.00, 4, 4),
(N'Teclado USB',              899.99, 4, 4),
(N'Cuaderno 100 hojas',        70.00, 5, 5),
(N'Caja de lápices (12 uds)',  95.00, 5, 5),
(N'Resma de papel carta',     420.00, 5, 5),
(N'Marcadores (pack 6)',      180.00, 5, 5);
GO
