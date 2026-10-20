namespace SistemaInventario.Entidades;

/// <summary>Categoría de productos (puede tener hasta 5 fotos).</summary>
public class Categoria : EntidadBase
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public List<string> Fotos { get; set; } = new();
}
