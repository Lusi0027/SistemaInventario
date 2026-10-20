using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// <summary>Operaciones de negocio para productos.</summary>
public interface IProductoServicio
{
    /// <summary>Lista productos activos con búsqueda y filtros opcionales.</summary>
    List<ProductoVista> Listar(string texto, int? categoriaId, int? suplidorId, decimal? precioMin, decimal? precioMax);
    Producto ObtenerPorId(int id);
    Dictionary<string, string> Guardar(Producto producto);
    void Eliminar(int id);
}
