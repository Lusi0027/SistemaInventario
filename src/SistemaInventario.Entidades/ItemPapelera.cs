namespace SistemaInventario.Entidades;

/// <summary>Registro eliminado (de cualquier tipo) que se muestra en la papelera.</summary>
public class ItemPapelera
{
    public int Id { get; set; }
    public string Tipo { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaModificacion { get; set; }
}
