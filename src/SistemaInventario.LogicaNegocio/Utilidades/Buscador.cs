namespace SistemaInventario.LogicaNegocio.Utilidades;

/// Utilidad genérica de búsqueda parcial (sin distinguir mayúsculas/minúsculas).
public static class Buscador
{
    /// Filtra una secuencia de T por texto en cualquiera de los campos indicados.
    /// Tipo de elemento.
    /// Secuencia a filtrar.
    /// Texto a buscar. Si es vacío se devuelve todo.
    /// Funciones que devuelven los campos donde buscar.
    public static IEnumerable<T> Filtrar<T>(IEnumerable<T> origen, string? texto,
        params Func<T, string?>[] selectores)
    {
        if (string.IsNullOrWhiteSpace(texto) || selectores.Length == 0) return origen;

        var buscado = texto.Trim();
        return origen.Where(item => selectores.Any(campo =>
            campo(item)?.Contains(buscado, StringComparison.OrdinalIgnoreCase) == true));
    }
}