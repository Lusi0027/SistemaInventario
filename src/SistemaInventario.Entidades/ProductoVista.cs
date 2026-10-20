namespace SistemaInventario.Entidades;

/// Producto con los nombres de su categoría y suplidor 
public class ProductoVista
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public string Categoria { get; set; }
    public int SuplidorId { get; set; }
    public string Suplidor { get; set; }
    public DateTime FechaCreacion { get; set; }
}
