namespace SistemaInventario.AccesoDatos;

/// <summary>Escribe los errores en el archivo errores.log.</summary>
public static class Logger
{
    private static readonly object _candado = new();

    /// <summary>Registra una excepción con fecha y contexto.</summary>
    public static void Registrar(Exception ex, string contexto = "")
    {
        try
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "errores.log");
            string linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {contexto} | {ex.GetType().Name}: {ex.Message}{Environment.NewLine}";
            lock (_candado) File.AppendAllText(ruta, linea);
        }
        catch (IOException) { /* si no se puede escribir el log, no se hace nada */ }
    }
}
