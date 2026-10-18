namespace SistemaInventario.Presentacion.Utilidades;

/// <summary>Ayuda para mostrar los errores de <c>ValidacionException</c> con un ErrorProvider.</summary>
public static class ErrorProviderExtensiones
{
    /// <summary>
    /// Limpia el provider y asigna cada error al control cuyo nombre de propiedad coincide con la clave.
    /// </summary>
    /// <param name="provider">ErrorProvider del formulario.</param>
    /// <param name="errores">Errores (clave = nombre de propiedad, p. ej. "Nombre").</param>
    /// <param name="controles">Mapa propiedad → control, p. ej. { ["Nombre"] = txtNombre }.</param>
    /// <returns>Errores sin control asociado (para mostrarlos en un MessageBox).</returns>
    public static IReadOnlyList<string> MostrarErrores(this ErrorProvider provider,
        IReadOnlyDictionary<string, string> errores, IReadOnlyDictionary<string, Control> controles)
    {
        provider.Clear();
        var sinControl = new List<string>();
        foreach (var (campo, mensaje) in errores)
        {
            if (controles.TryGetValue(campo, out var control)) provider.SetError(control, mensaje);
            else sinControl.Add(mensaje);
        }
        return sinControl;
    }
}
