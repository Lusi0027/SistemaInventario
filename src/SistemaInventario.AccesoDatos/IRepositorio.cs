using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos;

/// Repositorio genérico con las operaciones CRUD 
public interface IRepositorio<T> where T : EntidadBase
{
    List<T> ObtenerActivos();
    List<T> ObtenerEliminados();
    T ObtenerPorId(int id);
    int Insertar(T entidad);
    void Actualizar(T entidad);
    void Eliminar(int id);
    void Restaurar(int id);
}
