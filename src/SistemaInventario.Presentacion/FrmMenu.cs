using System.Globalization;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio;

namespace SistemaInventario.Presentacion;

/// <summary>Menú principal: cuadrícula de mosaicos que se adapta al tamaño de la ventana.</summary>
public class FrmMenu : Form
{
    private readonly FabricaServicios _fabrica;

    /// <summary>True si el usuario eligió "Cerrar sesión" (para volver al login).</summary>
    public bool CerrarSesion { get; private set; }

    public FrmMenu(FabricaServicios fabrica, Usuario usuario)
    {
        _fabrica = fabrica;
        Text = "Sistema de Inventario - Menú principal";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(900, 600);
        MinimumSize = new Size(700, 500);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Estilo.Fondo;
        Font = Estilo.Fuente;

        // Encabezado: título + saludo
        var saludo = new Label
        {
            Text = $"Bienvenido, {usuario.NombreCompleto}", Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 11F), ForeColor = Estilo.Texto, TextAlign = ContentAlignment.TopLeft
        };
        var titulo = new Label
        {
            Text = "Sistema de Inventario", Dock = DockStyle.Top, Height = 48,
            Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Estilo.Texto, TextAlign = ContentAlignment.BottomLeft
        };
        var encabezado = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Estilo.Encabezado, Padding = new Padding(30, 8, 30, 8) };
        encabezado.Controls.Add(saludo);
        encabezado.Controls.Add(titulo);

        // Pie con la fecha
        string fecha = DateTime.Now.ToString("dddd, d 'de' MMMM 'de' yyyy", new CultureInfo("es-DO"));
        var pie = new Label
        {
            Text = "  " + char.ToUpper(fecha[0]) + fecha[1..], Dock = DockStyle.Bottom, Height = 34,
            BackColor = Estilo.Encabezado, ForeColor = Estilo.Texto, TextAlign = ContentAlignment.MiddleLeft
        };

        // Cuadrícula 3 x 2 con celdas iguales
        var rejilla = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2, Padding = new Padding(25) };
        for (int i = 0; i < 3; i++) rejilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        for (int i = 0; i < 2; i++) rejilla.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

        rejilla.Controls.Add(Tile("📁", "Categorías", Estilo.Primario, () => Abrir(new FrmCategorias(_fabrica.Categorias))), 0, 0);
        rejilla.Controls.Add(Tile("🚚", "Suplidores", Estilo.Secundario, () => Abrir(new FrmSuplidores(_fabrica.Suplidores))), 1, 0);
        rejilla.Controls.Add(Tile("📦", "Productos", Estilo.Acento, () => Abrir(new FrmProductos(_fabrica.Productos, _fabrica.Categorias, _fabrica.Suplidores))), 2, 0);
        rejilla.Controls.Add(Tile("🗑", "Papelera", Estilo.Lila, () => Abrir(new FrmPapelera(_fabrica.Papelera))), 0, 1);
        rejilla.Controls.Add(Tile("📊", "Reportes", Estilo.Primario, () => Abrir(new FrmReportes(_fabrica.Reportes))), 1, 1);
        rejilla.Controls.Add(Tile("🚪", "Cerrar sesión", Estilo.Peligro, () => { CerrarSesion = true; Close(); }), 2, 1);

        Controls.Add(rejilla);
        Controls.Add(encabezado);
        Controls.Add(pie);
    }

    /// <summary>Mosaico grande con icono y nombre; se aclara al pasar el mouse.</summary>
    private static Button Tile(string icono, string texto, Color color, Action accion)
    {
        var b = Estilo.Boton(icono + "\n" + texto, color);
        b.Dock = DockStyle.Fill;
        b.Margin = new Padding(12);
        b.Font = new Font("Segoe UI Emoji", 16F, FontStyle.Bold);
        b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color, 0.25F);
        b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(color, 0.1F);
        b.Click += (s, e) => accion();
        return b;
    }

    private void Abrir(Form formulario)
    {
        using (formulario) formulario.ShowDialog(this);
    }
}
