namespace SistemaInventario.AccesoDatos;

public static class Logger
{
    private static readonly object Bloqueo = new();

    public static string RutaArchivo { get; set; } = Path.Combine(AppContext.BaseDirectory, "errores.log");

    public static void Registrar(string contexto, Exception excepcion)
    {
        try
        {
            var linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {contexto}: {excepcion.GetType().Name} - " +
                        $"{excepcion.Message}{Environment.NewLine}{excepcion.StackTrace}{Environment.NewLine}";
            lock (Bloqueo)
            {
                File.AppendAllText(RutaArchivo, linea);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}