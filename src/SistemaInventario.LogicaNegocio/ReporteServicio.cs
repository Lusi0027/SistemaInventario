using SistemaInventario.AccesoDatos;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio;

/// <summary>Consultas LINQ de resumen (agrupaciones, promedios, top, etc.).</summary>
public class ReporteServicio : IReporteServicio
{
    private readonly IRepositorio<Producto> _productos;
    private readonly IRepositorio<Categoria> _categorias;
    private readonly IRepositorio<Suplidor> _suplidores;

    public ReporteServicio(IRepositorio<Producto> productos, IRepositorio<Categoria> categorias, IRepositorio<Suplidor> suplidores)
    {
        _productos = productos;
        _categorias = categorias;
        _suplidores = suplidores;
    }

    /// <summary>LINQ #1: productos agrupados por categoría con conteo.</summary>
    public List<FilaReporte> ProductosPorCategoria() =>
        _categorias.ObtenerActivos()
            .GroupJoin(_productos.ObtenerActivos(), c => c.Id, p => p.CategoriaId,
                       (c, ps) => new FilaReporte { Descripcion = c.Nombre, Valor = ps.Count() })
            .OrderByDescending(f => f.Valor).ToList();

    /// <summary>LINQ #2: top 5 suplidores con más productos.</summary>
    public List<FilaReporte> TopSuplidores()
    {
        var productos = _productos.ObtenerActivos();
        return _suplidores.ObtenerActivos()
            .Select(s => new FilaReporte { Descripcion = s.Empresa, Valor = productos.Count(p => p.SuplidorId == s.Id) })
            .OrderByDescending(f => f.Valor).Take(5).ToList();
    }

    /// <summary>LINQ #3: promedio de precios por categoría.</summary>
    public List<FilaReporte> PromedioPrecioPorCategoria() =>
        _productos.ObtenerActivos()
            .GroupBy(p => p.CategoriaId)
            .Join(_categorias.ObtenerActivos(), g => g.Key, c => c.Id,
                  (g, c) => new FilaReporte { Descripcion = c.Nombre, Valor = Math.Round(g.Average(p => p.Precio), 2) })
            .OrderByDescending(f => f.Valor).ToList();

    /// <summary>LINQ #4: producto más caro de cada categoría.</summary>
    public List<FilaReporte> ProductoMasCaroPorCategoria() =>
        _productos.ObtenerActivos()
            .GroupBy(p => p.CategoriaId)
            .Join(_categorias.ObtenerActivos(), g => g.Key, c => c.Id, (g, c) =>
            {
                var masCaro = g.OrderByDescending(p => p.Precio).First();
                return new FilaReporte { Descripcion = $"{c.Nombre}: {masCaro.Nombre}", Valor = masCaro.Precio };
            })
            .OrderBy(f => f.Descripcion).ToList();

    /// <summary>LINQ #5: suma de precios (valor del catálogo) por suplidor.</summary>
    public List<FilaReporte> ValorInventarioPorSuplidor() =>
        _productos.ObtenerActivos()
            .GroupBy(p => p.SuplidorId)
            .Join(_suplidores.ObtenerActivos(), g => g.Key, s => s.Id,
                  (g, s) => new FilaReporte { Descripcion = s.Empresa, Valor = g.Sum(p => p.Precio) })
            .OrderByDescending(f => f.Valor).ToList();
}
