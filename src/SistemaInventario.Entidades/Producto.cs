namespace SistemaInventario.Entidades;

public class Producto : EntidadBase
{
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }
    public int CategoriaId { get; set; }
    public int SuplidorId { get; set; }

    public string CategoriaNombre { get; set; } = string.Empty;

    public string SuplidorNombre { get; set; } = string.Empty;
}
