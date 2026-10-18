using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos.Interfaces;

/// <summary>Repositorio de categorías.</summary>
public interface ICategoriaRepositorio : IRepositorio<Categoria>
{
    /// <summary>Indica si ya existe una categoría (activa o eliminada) con ese nombre.</summary>
    bool ExisteNombre(string nombre, int? excluirId = null);

    /// <summary>Cantidad de productos activos de la categoría.</summary>
    int ContarProductosActivos(int categoriaId);
}
