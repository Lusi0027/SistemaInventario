namespace SistemaInventario.Presentacion;

/// <summary>
/// Formulario base de edición: apila campos con etiqueta, muestra los errores de validación
/// con ErrorProvider y tiene los botones Guardar / Cancelar.
/// </summary>
public abstract class FrmEditarBase : Form
{
    protected readonly ErrorProvider errores = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private readonly Dictionary<string, Control> _campos = new();
    private readonly FlowLayoutPanel _contenido = new()
    {
        Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
        WrapContents = false, AutoScroll = true, Padding = new Padding(25, 10, 10, 10)
    };

    protected FrmEditarBase(string titulo)
    {
        Text = titulo;
        ClientSize = new Size(480, 560);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Estilo.Fondo;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = Estilo.Fuente;

        var encabezado = new Label
        {
            Text = "  " + titulo, Dock = DockStyle.Top, Height = 50,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold), BackColor = Estilo.Encabezado,
            ForeColor = Estilo.Texto, TextAlign = ContentAlignment.MiddleLeft
        };

        var btnGuardar = Estilo.Boton("Guardar", Estilo.Primario);
        var btnCancelar = Estilo.Boton("Cancelar", Estilo.Peligro);
        btnGuardar.Click += (s, e) => Guardar();
        btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

        var pie = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom, Height = 58, FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(15, 10, 15, 0), BackColor = Estilo.Encabezado
        };
        pie.Controls.Add(btnCancelar);
        pie.Controls.Add(btnGuardar);

        _contenido.SizeChanged += (s, e) => AjustarAnchos();
        Controls.Add(_contenido);
        Controls.Add(pie);
        Controls.Add(encabezado);
        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;
    }

    /// <summary>Agrega un campo con etiqueta. La clave debe coincidir con la que devuelve la validación.</summary>
    protected void AgregarCampo(string clave, string etiqueta, Control control)
    {
        _contenido.Controls.Add(new Label
        {
            Text = etiqueta, AutoSize = true, ForeColor = Estilo.Texto,
            Font = Estilo.FuenteNegrita, Margin = new Padding(3, 10, 3, 2)
        });
        control.Width = 380;
        _contenido.Controls.Add(control);
        _campos[clave] = control;
    }

    /// <summary>Estira los campos al ancho disponible (deja espacio para el icono de error).</summary>
    private void AjustarAnchos()
    {
        int ancho = Math.Max(200, _contenido.ClientSize.Width - _contenido.Padding.Horizontal - 40);
        foreach (Control c in _contenido.Controls)
            if (c is not Label) c.Width = ancho;
    }

    /// <summary>Agrega un control sin etiqueta (por ejemplo, una fila de botones).</summary>
    protected void AgregarControl(Control control) => _contenido.Controls.Add(control);

    /// <summary>Valida y guarda. Devuelve los errores por campo (vacío si se guardó).</summary>
    protected abstract Dictionary<string, string> GuardarEntidad();

    private void Guardar()
    {
        errores.Clear();
        try
        {
            var resultado = GuardarEntidad();
            if (resultado.Count == 0)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            foreach (var par in resultado)
            {
                if (_campos.TryGetValue(par.Key, out var control))
                    errores.SetError(control, par.Value);
            }
        }
        catch (Exception ex)
        {
            Estilo.Error(ex.Message);
        }
    }
}
