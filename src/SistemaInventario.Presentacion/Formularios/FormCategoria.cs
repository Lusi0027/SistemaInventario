using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Alta y edición de categorías (con hasta 5 fotos).</summary>
public class FormCategoria : FormEdicionBase
{
    private readonly ICategoriaServicio _servicio;
    private readonly Categoria _categoria;
    private readonly bool _esNueva;
    private readonly TextBox _txtNombre = new() { MaxLength = 100 };
    private readonly TextBox _txtDescripcion = new() { Multiline = true, MaxLength = 500, ScrollBars = ScrollBars.Vertical };
    private readonly ListBox _lstFotos = new() { Dock = DockStyle.Left, Width = 190, FormattingEnabled = true };
    private readonly PictureBox _vistaPrevia = new() { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };

    public FormCategoria(ICategoriaServicio servicio, Categoria? categoria = null)
        : base(categoria is null ? "Nueva categoría" : "Editar categoría")
    {
        _servicio = servicio;
        _esNueva = categoria is null;
        _categoria = categoria ?? new Categoria();

        var btnAgregar = Ui.Boton("Agregar", Ui.Primario, 80);
        var btnQuitar = Ui.Boton("Quitar", Ui.Peligro, 80);
        var botonesFoto = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 36, Padding = new Padding(0, 4, 0, 0) };
        botonesFoto.Controls.AddRange(new Control[] { btnAgregar, btnQuitar });

        var panelFotos = new Panel();
        panelFotos.Controls.Add(_vistaPrevia);
        panelFotos.Controls.Add(_lstFotos);
        panelFotos.Controls.Add(botonesFoto);
        botonesFoto.BringToFront();
        _lstFotos.BringToFront();
        _vistaPrevia.BringToFront();

        _lstFotos.Format += (_, e) => e.Value = Path.GetFileName(((FotoCategoria)e.ListItem!).RutaArchivo);
        _lstFotos.SelectedIndexChanged += (_, _) => MostrarVistaPrevia();
        btnAgregar.Click += (_, _) => AgregarFotos();
        btnQuitar.Click += (_, _) =>
        {
            if (_lstFotos.SelectedIndex >= 0) _lstFotos.Items.RemoveAt(_lstFotos.SelectedIndex);
            MostrarVistaPrevia();
        };

        AgregarCampo(nameof(Categoria.Nombre), "Nombre *", _txtNombre);
        AgregarCampo(nameof(Categoria.Descripcion), "Descripción *", _txtDescripcion, 90);
        AgregarCampo(nameof(Categoria.Fotos), "Fotos (máx. 5)", panelFotos, 170);
        Construir();

        _txtNombre.Text = _categoria.Nombre;
        _txtDescripcion.Text = _categoria.Descripcion;
        foreach (var foto in _categoria.Fotos) _lstFotos.Items.Add(foto);
    }

    private void AgregarFotos()
    {
        using var dialogo = new OpenFileDialog
        {
            Title = "Seleccionar fotos",
            Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
            Multiselect = true
        };
        if (dialogo.ShowDialog(this) != DialogResult.OK) return;

        foreach (var ruta in dialogo.FileNames)
        {
            if (_lstFotos.Items.Count >= 5)
            {
                Ui.Aviso("Una categoría admite máximo 5 fotos.");
                break;
            }
            _lstFotos.Items.Add(new FotoCategoria { RutaArchivo = ruta });
        }
    }

    private void MostrarVistaPrevia()
    {
        _vistaPrevia.Image?.Dispose();
        _vistaPrevia.Image = null;
        if (_lstFotos.SelectedItem is not FotoCategoria foto || !File.Exists(foto.RutaArchivo)) return;

        try
        {
            using var flujo = File.OpenRead(foto.RutaArchivo);
            using var imagen = Image.FromStream(flujo);
            _vistaPrevia.Image = new Bitmap(imagen); // copia: no deja el archivo bloqueado
        }
        catch (ArgumentException)
        {
            // Archivo de imagen inválido: el validador lo reportará al guardar.
        }
    }

    protected override void Guardar()
    {
        _categoria.Nombre = _txtNombre.Text;
        _categoria.Descripcion = _txtDescripcion.Text;
        _categoria.Fotos = _lstFotos.Items.Cast<FotoCategoria>().ToList();

        if (_esNueva) _servicio.Crear(_categoria);
        else _servicio.Actualizar(_categoria);
    }
}
