using System.Security.Cryptography;
using System.Text;

namespace SistemaInventario.LogicaNegocio;

/// <summary>Métodos genéricos y de ayuda.</summary>
public static class Utilidades
{
    /// <summary>
    /// Método genérico propio: búsqueda parcial (sin distinguir mayúsculas) sobre
    /// cualquier lista, indicando qué campos de texto se comparan.
    /// </summary>
    public static IEnumerable<T> Buscar<T>(IEnumerable<T> lista, string texto, params Func<T, string>[] campos)
    {
        if (string.IsNullOrWhiteSpace(texto)) return lista;
        string buscado = texto.Trim();
        return lista.Where(x => campos.Any(campo =>
            (campo(x) ?? "").Contains(buscado, StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>Calcula el hash SHA-256 (hexadecimal en mayúsculas) de un texto.</summary>
    public static string Hash(string texto) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(texto ?? "")));
}
