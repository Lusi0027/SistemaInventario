namespace SistemaInventario.Entidades;

/// Registro eliminado que se muestra en la papelera
public class ItemPapelera
{
    public int Id { get; set; }
    public string Tipo { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaModificacion { get; set; }
}
