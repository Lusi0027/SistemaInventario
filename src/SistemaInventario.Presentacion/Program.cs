using System.Text.Json;
using SistemaInventario.LogicaNegocio;

namespace SistemaInventario.Presentacion;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            var servicios = ContenedorServicios.Crear(LeerCadenaConexion());
            Application.Run(new FormPrincipal(servicios));
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo iniciar la aplicación:" + Environment.NewLine + ex.Message,
                "Sistema de Inventario", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string LeerCadenaConexion()
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        using var documento = JsonDocument.Parse(File.ReadAllText(ruta));
        return documento.RootElement.GetProperty("CadenaConexion").GetString()
               ?? throw new InvalidOperationException("Falta 'CadenaConexion' en appsettings.json.");
    }
}
