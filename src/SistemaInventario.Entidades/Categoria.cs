namespace SistemaInventario.Entidades;

/// Categoría de productos 
public class Categoria : EntidadBase
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public List<string> Fotos { get; set; } = new();
}
