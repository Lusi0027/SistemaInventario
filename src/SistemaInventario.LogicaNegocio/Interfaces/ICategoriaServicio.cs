using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// <summary>Operaciones de negocio para categorías.</summary>
public interface ICategoriaServicio
{
    /// <summary>Lista las categorías activas filtrando por texto.</summary>
    List<Categoria> Listar(string texto);
    Categoria ObtenerPorId(int id);
    /// <summary>Valida y guarda. Devuelve los errores por campo (vacío si todo salió bien).</summary>
    Dictionary<string, string> Guardar(Categoria categoria);
    /// <summary>Elimina (lógico). Devuelve un mensaje de error o null si se eliminó.</summary>
    string Eliminar(int id);
}
