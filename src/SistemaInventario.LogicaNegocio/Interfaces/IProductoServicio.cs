using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

public interface IProductoServicio
{
    IReadOnlyList<Producto> Listar(ProductoFiltro? filtro = null);
    IReadOnlyList<Producto> ListarEliminados(string? busqueda = null);
    Producto? ObtenerPorId(int id);
    int Crear(Producto producto);
    void Actualizar(Producto producto);
    void Eliminar(int id);
    void Restaurar(int id);
}