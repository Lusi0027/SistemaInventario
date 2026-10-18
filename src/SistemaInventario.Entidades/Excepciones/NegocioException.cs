namespace SistemaInventario.Entidades.Excepciones;

/// <summary>Violación de una regla de negocio (p. ej. eliminar una categoría con productos activos).</summary>
public class NegocioException : ExcepcionAplicacion
{
    public NegocioException(string mensaje) : base(mensaje) { }
}
