using SistemaInventario.LogicaNegocio;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Ventana principal: menú lateral y área de contenido.</summary>
public class FormPrincipal : Form
{
    private readonly ContenedorServicios _servicios;
    private readonly Panel _contenido = new() { Dock = DockStyle.Fill, BackColor = Color.White };
    private readonly Dictionary<Button, Func<UserControl>> _secciones = new();

    public FormPrincipal(ContenedorServicios servicios)
    {
        _servicios = servicios;
        Text = "Sistema de Inventario";
        Font = Ui.Fuente;
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1150, 700);
        MinimumSize = new Size(900, 550);

        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 210,
            BackColor = Ui.Oscuro,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };
        menu.Controls.Add(new Label
        {
            Text = "INVENTARIO",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 15f, FontStyle.Bold),
            Width = 210,
            Height = 70,
            TextAlign = ContentAlignment.MiddleCenter
        });

        AgregarSeccion(menu, "Categorías", () => new ControlCategorias(_servicios.Categorias));
        AgregarSeccion(menu, "Suplidores", () => new ControlSuplidores(_servicios.Suplidores));
        AgregarSeccion(menu, "Productos", () => new ControlProductos(_servicios));
        AgregarSeccion(menu, "Papelera", () => new ControlPapelera(_servicios));
        AgregarSeccion(menu, "Reportes", () => new ControlReportes(_servicios));

        Controls.Add(_contenido);
        Controls.Add(menu);

        Navegar(_secciones.Keys.First());
    }

    private void AgregarSeccion(FlowLayoutPanel menu, string texto, Func<UserControl> crear)
    {
        var boton = new Button
        {
            Text = "   " + texto,
            Width = 210,
            Height = 46,
            Margin = new Padding(0),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Ui.Oscuro,
            TextAlign = ContentAlignment.MiddleLeft,
            Cursor = Cursors.Hand,
            FlatAppearance = { BorderSize = 0 }
        };
        boton.Click += (_, _) => Navegar(boton);
        menu.Controls.Add(boton);
        _secciones[boton] = crear;
    }

    private void Navegar(Button boton)
    {
        foreach (var b in _secciones.Keys)
            b.BackColor = b == boton ? Ui.Primario : Ui.Oscuro;

        foreach (Control anterior in _contenido.Controls.Cast<Control>().ToList())
        {
            _contenido.Controls.Remove(anterior);
            anterior.Dispose();
        }
        // Se crea una instancia nueva: así cada pantalla siempre muestra datos actualizados.
        _contenido.Controls.Add(_secciones[boton]());
    }
}
