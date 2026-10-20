namespace SistemaInventario.Entidades;

/// <summary>Producto del inventario.</summary>
public class Producto : EntidadBase
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public int SuplidorId { get; set; }
}
