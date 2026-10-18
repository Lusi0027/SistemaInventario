namespace SistemaInventario.LogicaNegocio.Utilidades;

/// Copia las fotos seleccionadas a la carpeta "Fotos" de la aplicación.
public static class AlmacenFotos
{
    /// Carpeta donde se guardan las fotos.
    public static string Carpeta { get; set; } = Path.Combine(AppContext.BaseDirectory, "Fotos");

    /// Copia la foto a la carpeta administrada y devuelve la nueva ruta (si ya está ahí, la misma).
    /// Ruta del archivo elegido por el usuario.
    public static string Guardar(string rutaOrigen)
    {
        Directory.CreateDirectory(Carpeta);
        var carpetaCompleta = Path.GetFullPath(Carpeta);
        var directorioOrigen = Path.GetDirectoryName(Path.GetFullPath(rutaOrigen));
        if (string.Equals(directorioOrigen, carpetaCompleta, StringComparison.OrdinalIgnoreCase))
            return rutaOrigen;

        var destino = Path.Combine(carpetaCompleta, $"{Guid.NewGuid():N}{Path.GetExtension(rutaOrigen).ToLowerInvariant()}");
        File.Copy(rutaOrigen, destino);
        return destino;
    }
}
