namespace SistemaInventario.Entidades.Excepciones;

/// <summary>Base de las excepciones controladas de la aplicación. La UI puede capturar solo este tipo.</summary>
public class ExcepcionAplicacion : Exception
{
    public ExcepcionAplicacion(string mensaje, Exception? interna = null) : base(mensaje, interna) { }
}
