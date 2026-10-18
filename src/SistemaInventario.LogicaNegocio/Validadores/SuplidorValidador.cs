using System.Text.RegularExpressions;
using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Validadores;

/// Reglas de validación de Suplidor.
public class SuplidorValidador : Validador<Suplidor>
{
    private static readonly Regex RegexTelefono = new(@"^(809|829|849)-\d{3}-\d{4}$", RegexOptions.Compiled);
    private static readonly Regex RegexCorreo = new(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", RegexOptions.Compiled);

    protected override void Reglas(Suplidor s, IDictionary<string, string> errores)
    {
        TextoObligatorio(errores, nameof(Suplidor.NombreEmpresa), "El nombre de la empresa", s.NombreEmpresa, 1, 150);
        TextoObligatorio(errores, nameof(Suplidor.NombreContacto), "El nombre del contacto", s.NombreContacto, 1, 100);

        if (string.IsNullOrWhiteSpace(s.Telefono))
            errores[nameof(Suplidor.Telefono)] = "El teléfono es obligatorio.";
        else if (!RegexTelefono.IsMatch(s.Telefono.Trim()))
            errores[nameof(Suplidor.Telefono)] = "Formato inválido. Use 809/829/849-XXX-XXXX.";

        if (string.IsNullOrWhiteSpace(s.Correo))
            errores[nameof(Suplidor.Correo)] = "El correo es obligatorio.";
        else if (s.Correo.Length > 150 || !RegexCorreo.IsMatch(s.Correo.Trim()))
            errores[nameof(Suplidor.Correo)] = "El correo electrónico no es válido.";

        if (!string.IsNullOrWhiteSpace(s.SitioWeb) && !EsUrlValida(s.SitioWeb.Trim()))
            errores[nameof(Suplidor.SitioWeb)] = "La URL no es válida (ej. https://empresa.com).";
    }

    private static bool EsUrlValida(string valor) =>
        valor.Length <= 200 &&
        Uri.TryCreate(valor, UriKind.Absolute, out var uri) &&
        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}