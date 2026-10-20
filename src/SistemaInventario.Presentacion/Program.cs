using SistemaInventario.LogicaNegocio;

namespace SistemaInventario.Presentacion;

internal static class Program
{
    /// <summary>Punto de entrada: login -> menú principal (se repite si se cierra sesión).</summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var fabrica = new FabricaServicios();

        while (true)
        {
            using var login = new FrmLogin(fabrica.Usuarios);
            if (login.ShowDialog() != DialogResult.OK) break;

            using var menu = new FrmMenu(fabrica, login.UsuarioActual);
            Application.Run(menu);
            if (!menu.CerrarSesion) break;
        }
    }
}
