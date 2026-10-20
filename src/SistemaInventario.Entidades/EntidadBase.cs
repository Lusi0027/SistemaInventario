namespace SistemaInventario.Entidades;

/// <summary>Clase base con los campos comunes de todas las entidades (incluye borrado lógico).</summary>
public abstract class EntidadBase
{
    public int Id { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModificacion { get; set; }
}
