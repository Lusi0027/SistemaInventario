using SistemaInventario.Entidades.Excepciones;

namespace SistemaInventario.LogicaNegocio.Validadores;

/// Base genérica de validadores. Las claves de error son nombres de propiedad (para ErrorProvider).
/// Entidad a validar.
public abstract class Validador<T>
{
    /// Valida y devuelve los errores por campo (vacío si todo es válido).
    public IDictionary<string, string> Validar(T entidad)
    {
        var errores = new Dictionary<string, string>();
        Reglas(entidad, errores);
        return errores;
    }

    /// Valida y lanza ValidacionException si hay errores.
    /// Si alguna regla falla.
    public void ValidarOLanzar(T entidad)
    {
        var errores = Validar(entidad);
        if (errores.Count > 0) throw new ValidacionException(errores);
    }

    /// Reglas propias de cada entidad.
    protected abstract void Reglas(T entidad, IDictionary<string, string> errores);

    /// Valida un texto obligatorio con longitud mínima y máxima.
    protected static void TextoObligatorio(IDictionary<string, string> errores, string campo, string etiqueta,
        string? valor, int minimo, int maximo)
    {
        var largo = valor?.Trim().Length ?? 0;
        if (largo == 0)
            errores[campo] = $"{etiqueta} es obligatorio.";
        else if (largo < minimo || largo > maximo)
            errores[campo] = $"{etiqueta} debe tener entre {minimo} y {maximo} caracteres.";
    }
}