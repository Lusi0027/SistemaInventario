using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// <summary>Consultas LINQ de resumen.</summary>
public interface IReporteServicio
{
    List<FilaReporte> ProductosPorCategoria();
    List<FilaReporte> TopSuplidores();
    List<FilaReporte> PromedioPrecioPorCategoria();
    List<FilaReporte> ProductoMasCaroPorCategoria();
    List<FilaReporte> ValorInventarioPorSuplidor();
}
