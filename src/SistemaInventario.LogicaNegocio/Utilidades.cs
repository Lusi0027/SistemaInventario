using System.Security.Cryptography;
using System.Text;

namespace SistemaInventario.LogicaNegocio;

/// Métodos genéricos y de ayuda
public static class Utilidades
{
    
    /// Método genérico propio: búsqueda parcial (
   
    public static IEnumerable<T> Buscar<T>(IEnumerable<T> lista, string texto, params Func<T, string>[] campos)
    {
        if (string.IsNullOrWhiteSpace(texto)) return lista;
        string buscado = texto.Trim();
        return lista.Where(x => campos.Any(campo =>
            (campo(x) ?? "").Contains(buscado, StringComparison.OrdinalIgnoreCase)));
    }

    /// Calcula el hash SHA-256 de un texto
    public static string Hash(string texto) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(texto ?? "")));
}
