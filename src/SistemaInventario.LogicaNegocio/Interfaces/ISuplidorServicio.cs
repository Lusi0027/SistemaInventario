using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

public interface ISuplidorServicio
{
    IReadOnlyList<Suplidor> Listar(string? busqueda = null);
    IReadOnlyList<Suplidor> ListarEliminados(string? busqueda = null);
    Suplidor? ObtenerPorId(int id);
    int Crear(Suplidor suplidor);
    void Actualizar(Suplidor suplidor);
    void Eliminar(int id);
    void Restaurar(int id);
}