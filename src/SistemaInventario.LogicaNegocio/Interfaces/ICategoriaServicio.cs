using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

public interface ICategoriaServicio
{
    IReadOnlyList<Categoria> Listar(string? busqueda = null);
    IReadOnlyList<Categoria> ListarEliminadas(string? busqueda = null);
    Categoria? ObtenerPorId(int id);
    int Crear(Categoria categoria);
    void Actualizar(Categoria categoria);
    void Eliminar(int id);
    void Restaurar(int id);
}
