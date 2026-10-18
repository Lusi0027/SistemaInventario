using Microsoft.Data.SqlClient;

namespace SistemaInventario.AccesoDatos;

public sealed class ConexionBD
{
    private static readonly Lazy<ConexionBD> InstanciaUnica = new(() => new ConexionBD());
    private string _cadenaConexion =
        "Server=LUSI;Database=SistemaDeInventario;Trusted_Connection=True;TrustServerCertificate=True;";

    private ConexionBD() { }

    public static ConexionBD Instancia => InstanciaUnica.Value;

    public void Configurar(string cadenaConexion)
    {
        if (string.IsNullOrWhiteSpace(cadenaConexion))
            throw new ArgumentException("La cadena de conexión no puede estar vacía.", nameof(cadenaConexion));
        _cadenaConexion = cadenaConexion;
    }

    public SqlConnection CrearConexion() => new(_cadenaConexion);
}