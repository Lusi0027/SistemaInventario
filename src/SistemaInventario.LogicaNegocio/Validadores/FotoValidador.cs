namespace SistemaInventario.LogicaNegocio.Validadores;

/// Valida archivos de foto: .jpg/.jpeg/.png y máximo 2 MB.
public static class FotoValidador
{
    private static readonly string[] Extensiones = { ".jpg", ".jpeg", ".png" };

    /// Tamaño máximo en bytes (2 MB).
    public const long TamanoMaximoBytes = 2 * 1024 * 1024;

    /// Devuelve el mensaje de error o null si la foto es válida.
    public static string? Validar(string ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta)) return "La ruta de la foto está vacía.";

        var extension = Path.GetExtension(ruta).ToLowerInvariant();
        if (!Extensiones.Contains(extension))
            return $"'{Path.GetFileName(ruta)}': solo se permiten .jpg, .jpeg y .png.";

        if (!File.Exists(ruta))
            return $"'{Path.GetFileName(ruta)}': el archivo no existe.";

        if (new FileInfo(ruta).Length > TamanoMaximoBytes)
            return $"'{Path.GetFileName(ruta)}': supera el tamaño máximo de 2 MB.";

        return null;
    }
}