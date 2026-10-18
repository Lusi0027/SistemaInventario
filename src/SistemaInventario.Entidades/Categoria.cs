namespace SistemaInventario.Entidades;

public class Categoria : EntidadBase
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public List<FotoCategoria> Fotos { get; set; } = new();

    public override string ToString() => Nombre;
}