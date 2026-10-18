namespace SistemaInventario.Entidades;

public class FotoCategoria
{
    public int Id { get; set; }
    public int CategoriaId { get; set; }

    public string RutaArchivo { get; set; } = string.Empty;
}