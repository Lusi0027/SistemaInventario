using SistemaInventario.Entidades.Excepciones;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Base de los diálogos de alta/edición: tabla de campos, ErrorProvider y botones Guardar/Cancelar.</summary>
public abstract class FormEdicionBase : Form
{
    /// <summary>ErrorProvider del formulario.</summary>
    protected readonly ErrorProvider Errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };

    private readonly Dictionary<string, Control> _controles = new();
    private readonly TableLayoutPanel _tabla = new()
    {
        AutoSize = true,
        ColumnCount = 2,
        Dock = DockStyle.None
    };

    protected FormEdicionBase(string titulo)
    {
        Text = titulo;
        Font = Ui.Fuente;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(16);
        _tabla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380));
        Controls.Add(_tabla);
    }

    /// <summary>Agrega una fila etiqueta + control. La propiedad enlaza con las claves de ValidacionException.</summary>
    protected void AgregarCampo(string propiedad, string etiqueta, Control control, int alto = 0)
    {
        var fila = _tabla.RowCount++;
        _tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var etiquetaCtl = new Label
        {
            Text = etiqueta,
            AutoSize = true,
            Anchor = alto > 0 ? AnchorStyles.Top | AnchorStyles.Left : AnchorStyles.Left,
            Margin = new Padding(0, alto > 0 ? 9 : 6, 12, 6)
        };
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        control.Margin = new Padding(0, 4, 0, 6);
        if (alto > 0)
        {
            control.MinimumSize = new Size(0, alto);
            control.Height = alto;
        }
        _tabla.Controls.Add(etiquetaCtl, 0, fila);
        _tabla.Controls.Add(control, 1, fila);
        _controles[propiedad] = control;
    }

    /// <summary>Agrega la fila de botones. Debe llamarse al final del constructor de la clase hija.</summary>
    protected void Construir()
    {
        var aceptar = Ui.Boton("Guardar", Ui.Primario);
        var cancelar = Ui.Boton("Cancelar", Ui.Neutro);
        aceptar.Click += (_, _) => Aceptar();
        cancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        AcceptButton = aceptar;
        CancelButton = cancelar;

        var botones = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 10, 0, 0)
        };
        cancelar.Margin = new Padding(0);
        botones.Controls.Add(cancelar);
        botones.Controls.Add(aceptar);

        var fila = _tabla.RowCount++;
        _tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _tabla.Controls.Add(botones, 0, fila);
        _tabla.SetColumnSpan(botones, 2);
    }

    /// <summary>Lee los controles y llama al servicio. Puede lanzar ValidacionException.</summary>
    protected abstract void Guardar();

    private void Aceptar()
    {
        Errores.Clear();
        try
        {
            Guardar();
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidacionException ex)
        {
            var sinControl = Errores.MostrarErrores(ex.Errores, _controles);
            if (sinControl.Count > 0) Ui.Error(string.Join(Environment.NewLine, sinControl));
        }
        catch (ExcepcionAplicacion ex)
        {
            Ui.Error(ex.Message);
        }
        catch (Exception ex)
        {
            Ui.Error("Ocurrió un error inesperado: " + ex.Message);
        }
    }
}
