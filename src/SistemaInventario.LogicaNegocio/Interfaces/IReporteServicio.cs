using SistemaInventario.Entidades;
using SistemaInventario.Entidades.Reportes;

namespace SistemaInventario.LogicaNegocio.Interfaces;

public interface IReporteServicio
{
    IReadOnlyList<ProductosPorCategoriaDto> ProductosPorCategoria();
    IReadOnlyList<SuplidorTopDto> TopSuplidores(int cantidad = 5);
    IReadOnlyList<Producto> ProductosPorRangoYCategoria(decimal minimo, decimal maximo, int? categoriaId);
    IReadOnlyList<Producto> BuscarProductos(string texto);
    IReadOnlyList<PromedioPrecioCategoriaDto> PromedioPrecioPorCategoria();
    IReadOnlyList<ProductoMasCaroDto> ProductoMasCaroPorCategoria();
}