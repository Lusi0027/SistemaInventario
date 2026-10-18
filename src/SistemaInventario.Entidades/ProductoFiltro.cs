namespace SistemaInventario.Entidades;

public class ProductoFiltro
{
    public string? Texto { get; set; }
    public int? CategoriaId { get; set; }
    public int? SuplidorId { get; set; }
    public decimal? PrecioMinimo { get; set; }
    public decimal? PrecioMaximo { get; set; }
}