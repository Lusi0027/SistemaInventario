using System.Text.RegularExpressions;

namespace SistemaInventario.LogicaNegocio;

/// Reglas de validación reutilizables
public static class Validador
{
    private static readonly Regex _correo = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    private static readonly Regex _telefono = new(@"^(809|829|849)-\d{3}-\d{4}$");

    public static bool LongitudValida(string texto, int min, int max)
    {
        int largo = (texto ?? "").Trim().Length;
        return largo >= min && largo <= max;
    }

    public static bool EsCorreo(string valor) => !string.IsNullOrWhiteSpace(valor) && _correo.IsMatch(valor.Trim());

    public static bool EsTelefono(string valor) => !string.IsNullOrWhiteSpace(valor) && _telefono.IsMatch(valor.Trim());

    public static bool EsUrl(string valor) =>
        Uri.TryCreate(valor, UriKind.Absolute, out var uri) &&
        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    /// Precio mayor a 0 y con máximo 2 decimales
    public static bool EsPrecioValido(decimal precio) => precio > 0 && decimal.Round(precio, 2) == precio;

    /// Valida una foto (.jpg, .jpeg, .png, máx. 2 MB). Devuelve el error o null.
    public static string ValidarFoto(string ruta)
    {
        if (!File.Exists(ruta)) return "No se encontró el archivo: " + Path.GetFileName(ruta);
        string ext = Path.GetExtension(ruta).ToLowerInvariant();
        if (ext != ".jpg" && ext != ".jpeg" && ext != ".png") return "Solo se permiten fotos .jpg, .jpeg o .png.";
        if (new FileInfo(ruta).Length > 2 * 1024 * 1024) return "Cada foto debe pesar máximo 2 MB.";
        return null;
    }
}
