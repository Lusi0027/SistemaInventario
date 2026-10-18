using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos.Interfaces;

/// <summary>Repositorio genérico con las operaciones CRUD y borrado lógico.</summary>
/// <typeparam name="T">Entidad de dominio.</typeparam>
public interface IRepositorio<T> where T : EntidadBase
{
    /// <summary>Registros con Activo = 1.</summary>
    IEnumerable<T> ObtenerActivos();

    /// <summary>Registros con Activo = 0 (Papelera).</summary>
    IEnumerable<T> ObtenerEliminados();

    /// <summary>Obtiene un registro (activo o no) o null si no existe.</summary>
    T? ObtenerPorId(int id);

    /// <summary>Inserta y devuelve el Id generado.</summary>
    int Insertar(T entidad);

    /// <summary>Actualiza el registro y su fecha de modificación.</summary>
    void Actualizar(T entidad);

    /// <summary>Marca el registro como inactivo.</summary>
    void EliminarLogico(int id);

    /// <summary>Marca el registro como activo.</summary>
    void Restaurar(int id);
}
