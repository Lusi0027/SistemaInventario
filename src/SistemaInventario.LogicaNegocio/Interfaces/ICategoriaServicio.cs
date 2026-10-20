using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// Operaciones de negocio para categorías
public interface ICategoriaServicio
{
    /// Lista las categorías activas filtrando por texto
    List<Categoria> Listar(string texto);
    Categoria ObtenerPorId(int id);
    /// Valida y guarda. Devuelve los errores por campo
    Dictionary<string, string> Guardar(Categoria categoria);
    /// Elimina. Devuelve un mensaje de error o null si se eliminó
    string Eliminar(int id);
}
