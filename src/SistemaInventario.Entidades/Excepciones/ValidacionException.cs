namespace SistemaInventario.Entidades.Excepciones;

/// <summary>Errores de validación por campo. La clave es el nombre de la propiedad (para ErrorProvider).</summary>
public class ValidacionException : ExcepcionAplicacion
{
    public IReadOnlyDictionary<string, string> Errores { get; }

    public ValidacionException(IDictionary<string, string> errores)
        : base("Existen errores de validación:" + Environment.NewLine +
               string.Join(Environment.NewLine, errores.Values))
    {
        Errores = new Dictionary<string, string>(errores);
    }
}
