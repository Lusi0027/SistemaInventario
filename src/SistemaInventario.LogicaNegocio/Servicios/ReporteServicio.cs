using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;
using SistemaInventario.Entidades.Reportes;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio.Servicios;

/// Consultas LINQ de análisis sobre los productos activos.
public class ReporteServicio : IReporteServicio
{
    private readonly IProductoRepositorio _productos;


    public ReporteServicio(IProductoRepositorio productos)
    {
        _productos = productos ?? throw new ArgumentNullException(nameof(productos));
    }

    /// LINQ 1: productos agrupados por categoría con conteo.
    public IReadOnlyList<ProductosPorCategoriaDto> ProductosPorCategoria() =>
        _productos.ObtenerActivos()
            .GroupBy(p => p.CategoriaNombre)
            .Select(g => new ProductosPorCategoriaDto(g.Key, g.Count()))
            .OrderByDescending(x => x.Cantidad).ThenBy(x => x.Categoria)
            .ToList();

    /// LINQ 2: top N de suplidores con más productos.
    public IReadOnlyList<SuplidorTopDto> TopSuplidores(int cantidad = 5) =>
        _productos.ObtenerActivos()
            .GroupBy(p => p.SuplidorNombre)
            .Select(g => new SuplidorTopDto(g.Key, g.Count()))
            .OrderByDescending(x => x.CantidadProductos).ThenBy(x => x.Suplidor)
            .Take(cantidad)
            .ToList();

    /// LINQ 3: productos en un rango de precios, opcionalmente de una categoría.
    public IReadOnlyList<Producto> ProductosPorRangoYCategoria(decimal minimo, decimal maximo, int? categoriaId) =>
        _productos.ObtenerActivos()
            .Where(p => p.PrecioUnitario >= minimo && p.PrecioUnitario <= maximo)
            .Where(p => categoriaId == null || p.CategoriaId == categoriaId)
            .OrderBy(p => p.PrecioUnitario)
            .ToList();

    /// LINQ 4: búsqueda parcial ignorando mayúsculas/minúsculas.
    public IReadOnlyList<Producto> BuscarProductos(string texto) =>
        _productos.ObtenerActivos()
            .Where(p => p.Nombre.Contains(texto ?? string.Empty, StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p.Nombre)
            .ToList();

    /// LINQ 5: promedio de precios por categoría.
    public IReadOnlyList<PromedioPrecioCategoriaDto> PromedioPrecioPorCategoria() =>
        _productos.ObtenerActivos()
            .GroupBy(p => p.CategoriaNombre)
            .Select(g => new PromedioPrecioCategoriaDto(g.Key, Math.Round(g.Average(p => p.PrecioUnitario), 2)))
            .OrderBy(x => x.Categoria)
            .ToList();

    /// LINQ 6: producto más caro de cada categoría.
    public IReadOnlyList<ProductoMasCaroDto> ProductoMasCaroPorCategoria() =>
        _productos.ObtenerActivos()
            .GroupBy(p => p.CategoriaNombre)
            .Select(g => g.OrderByDescending(p => p.PrecioUnitario).First())
            .Select(p => new ProductoMasCaroDto(p.CategoriaNombre, p.Nombre, p.PrecioUnitario))
            .OrderBy(x => x.Categoria)
            .ToList();
}