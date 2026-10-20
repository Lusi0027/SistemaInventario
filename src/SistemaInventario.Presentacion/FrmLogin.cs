using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Pantalla de inicio de sesión (diseño con Dock: se adapta a cualquier escala de pantalla).</summary>
public class FrmLogin : Form
{
    private readonly IUsuarioServicio _servicio;
    private readonly TextBox txtUsuario = new() { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 8) };
    private readonly TextBox txtClave = new() { Dock = DockStyle.Fill, UseSystemPasswordChar = true, Margin = new Padding(0, 0, 0, 12) };
    private readonly Label lblError = new() { Dock = DockStyle.Fill, ForeColor = Estilo.Peligro, Font = Estilo.FuenteNegrita };

    /// <summary>Usuario que inició sesión correctamente.</summary>
    public Usuario UsuarioActual { get; private set; }

    public FrmLogin(IUsuarioServicio servicio)
    {
        _servicio = servicio;
        Text = "Iniciar sesión";
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(420, 480);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Estilo.Fondo;
        Font = Estilo.Fuente;

        var titulo = new Label
        {
            Text = "Sistema de Inventario", Dock = DockStyle.Top, Height = 80,
            TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            BackColor = Estilo.Encabezado, ForeColor = Estilo.Texto
        };

        var btnIngresar = Estilo.Boton("Ingresar", Estilo.Primario);
        btnIngresar.Dock = DockStyle.Fill;
        btnIngresar.Height = 44;
        btnIngresar.Margin = new Padding(0, 4, 0, 8);
        btnIngresar.Click += (s, e) => Ingresar();

        var hint = new Label
        {
            Text = "Ingrese con la cuenta configurada en su base de datos.", Dock = DockStyle.Bottom, Height = 50,
            ForeColor = Estilo.Texto, Font = new Font("Segoe UI", 9F, FontStyle.Italic),
            TextAlign = ContentAlignment.BottomLeft
        };
        var zonaMensajes = new Panel { Dock = DockStyle.Fill };
        zonaMensajes.Controls.Add(lblError);
        zonaMensajes.Controls.Add(hint);

        // Una sola columna que ocupa todo el ancho; cada fila toma su alto según el contenido
        var tabla = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7, Padding = new Padding(40, 15, 40, 10)
        };
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        for (int i = 0; i < 6; i++) tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        tabla.Controls.Add(Campo("Usuario"), 0, 0);
        tabla.Controls.Add(txtUsuario, 0, 1);
        tabla.Controls.Add(Campo("Contraseña"), 0, 2);
        tabla.Controls.Add(txtClave, 0, 3);
        tabla.Controls.Add(btnIngresar, 0, 4);
        tabla.Controls.Add(zonaMensajes, 0, 6);

        Controls.Add(tabla);
        Controls.Add(titulo);
        AcceptButton = btnIngresar;
    }

    private static Label Campo(string texto) => new()
    {
        Text = texto, AutoSize = true, ForeColor = Estilo.Texto, Font = Estilo.FuenteNegrita,
        Margin = new Padding(0, 8, 0, 3)
    };

    private void Ingresar()
    {
        try
        {
            UsuarioActual = _servicio.Autenticar(txtUsuario.Text, txtClave.Text);
            if (UsuarioActual == null)
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
                return;
            }
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}
