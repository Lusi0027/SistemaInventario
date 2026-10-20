using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// Operaciones de negocio para productos
public interface IProductoServicio
{
    /// Lista productos activos con búsqueda y filtros opcionales
    List<ProductoVista> Listar(string texto, int? categoriaId, int? suplidorId, decimal? precioMin, decimal? precioMax);
    Producto ObtenerPorId(int id);
    Dictionary<string, string> Guardar(Producto producto);
    void Eliminar(int id);
}
