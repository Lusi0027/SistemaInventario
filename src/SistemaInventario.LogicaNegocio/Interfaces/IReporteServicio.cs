using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// Consultas LINQ de resumen
public interface IReporteServicio
{
    List<FilaReporte> ProductosPorCategoria();
    List<FilaReporte> TopSuplidores();
    List<FilaReporte> PromedioPrecioPorCategoria();
    List<FilaReporte> ProductoMasCaroPorCategoria();
    List<FilaReporte> ValorInventarioPorSuplidor();
}
