namespace SistemaInventario.Entidades.Excepciones;

/// <summary>Error al acceder a la base de datos (envuelve a SqlException).</summary>
public class AccesoDatosException : ExcepcionAplicacion
{
    public AccesoDatosException(string mensaje, Exception? interna = null) : base(mensaje, interna) { }
}
