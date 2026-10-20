using Microsoft.Data.SqlClient;

namespace SistemaInventario.AccesoDatos;


/// Patrón Singleton: una única instancia que administra la cadena de conexión

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

    /// Instancia única de la clase
    public static ConexionBD Instancia => _instancia.Value;

    /// Crea una conexión nueva (sin abrir) usando la cadena configurada
    public SqlConnection CrearConexion() => new SqlConnection(_cadena);
}
