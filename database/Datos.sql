USE SistemaDeInventario;
GO

INSERT INTO dbo.Categorias (Nombre, Descripcion) VALUES
(N'Electrónica',  N'Dispositivos y accesorios electrónicos de consumo.'),
(N'Hogar',        N'Artículos y electrodomésticos para el hogar.'),
(N'Oficina',      N'Papelería y equipos de oficina.'),
(N'Alimentos',    N'Productos alimenticios no perecederos.'),
(N'Herramientas', N'Herramientas manuales y eléctricas.');

INSERT INTO dbo.Suplidores (NombreEmpresa, NombreContacto, Telefono, Correo, SitioWeb) VALUES
(N'TecnoCaribe SRL',        N'Juan Pérez',    '809-555-0101', 'ventas@tecnocaribe.com.do',    'https://www.tecnocaribe.com.do'),
(N'Distribuidora Del Este', N'María Gómez',   '829-555-0102', 'contacto@disteste.com.do',     NULL),
(N'Oficentro Dominicano',   N'Luis Martínez', '849-555-0103', 'info@oficentro.com.do',        'https://www.oficentro.com.do'),
(N'Alimentos La Romana',    N'Ana Rodríguez', '809-555-0104', 'pedidos@alimentosromana.com',  NULL),
(N'FerroMax SRL',           N'Pedro Santana', '829-555-0105', 'ventas@ferromax.com.do',       'https://www.ferromax.com.do');

INSERT INTO dbo.Productos (Nombre, PrecioUnitario, CategoriaId, SuplidorId) VALUES
(N'Audífonos Bluetooth',        1850.00, 1, 1),
(N'Cargador USB-C 30W',          950.00, 1, 1),
(N'Bocina Portátil',            2400.00, 1, 1),
(N'Teclado Inalámbrico',        1350.00, 1, 3),
(N'Licuadora 600W',             3200.00, 2, 2),
(N'Juego de Sábanas Queen',     1750.00, 2, 2),
(N'Abanico de Pedestal',        2900.00, 2, 2),
(N'Olla de Presión 6L',         2600.00, 2, 5),
(N'Resma de Papel Carta',        420.00, 3, 3),
(N'Caja de Bolígrafos x50',      380.00, 3, 3),
(N'Grapadora Metálica',          275.00, 3, 3),
(N'Silla Ergonómica',           6800.00, 3, 3),
(N'Arroz Selecto 10 lb',         380.00, 4, 4),
(N'Aceite de Girasol 1 L',       310.00, 4, 4),
(N'Habichuelas Rojas 2 lb',      145.00, 4, 4),
(N'Café Molido 1 lb',            295.00, 4, 4),
(N'Taladro Percutor 650W',      4200.00, 5, 5),
(N'Juego de Destornilladores',   890.00, 5, 5),
(N'Martillo de Acero',           520.00, 5, 5),
(N'Cinta Métrica 8 m',           215.00, 5, 5);

-- Un producto ya eliminado para probar la Papelera.
INSERT INTO dbo.Productos (Nombre, PrecioUnitario, CategoriaId, SuplidorId, Activo, FechaModificacion)
VALUES (N'Mouse Óptico (descontinuado)', 350.00, 1, 1, 0, SYSDATETIME());
GO
