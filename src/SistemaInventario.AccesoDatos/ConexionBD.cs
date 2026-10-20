using Microsoft.Data.SqlClient;

namespace SistemaInventario.AccesoDatos;

/// <summary>
/// Patrón Singleton: una única instancia que administra la cadena de conexión.
/// Si existe el archivo conexion.txt junto al ejecutable, se usa esa cadena.
/// </summary>
public sealed class ConexionBD
{
    private const string CadenaPorDefecto =
        "Server=localhost;Database=SistemaInventario;Trusted_Connection=True;TrustServerCertificate=True;";

    private static readonly Lazy<ConexionBD> _instancia = new(() => new ConexionBD());
    private readonly string _cadena;

    private ConexionBD()
    {
        string archivo = Path.Combine(AppContext.BaseDirectory, "conexion.txt");
        _cadena = File.Exists(archivo) ? File.ReadAllText(archivo).Trim() : CadenaPorDefecto;
    }

    /// <summary>Instancia única de la clase.</summary>
    public static ConexionBD Instancia => _instancia.Value;

    /// <summary>Crea una conexión nueva (sin abrir) usando la cadena configurada.</summary>
    public SqlConnection CrearConexion() => new SqlConnection(_cadena);
}
