using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos.Interfaces;

/// <summary>Repositorio de suplidores.</summary>
public interface ISuplidorRepositorio : IRepositorio<Suplidor>
{
    /// <summary>Indica si ya existe un suplidor (activo o eliminado) con ese nombre de empresa.</summary>
    bool ExisteNombreEmpresa(string nombreEmpresa, int? excluirId = null);

    /// <summary>Cantidad de productos activos del suplidor.</summary>
    int ContarProductosActivos(int suplidorId);
}
