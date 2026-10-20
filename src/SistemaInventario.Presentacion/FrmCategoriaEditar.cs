using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Crear o editar una categoría (con sus fotos).</summary>
public class FrmCategoriaEditar : FrmEditarBase
{
    private readonly ICategoriaServicio _servicio;
    private readonly Categoria _categoria;
    private readonly TextBox txtNombre = new();
    private readonly TextBox txtDescripcion = new() { Multiline = true, Height = 80 };
    private readonly ListBox lstFotos = new() { Height = 70 };
    private readonly PictureBox picVista = new() { Width = 380, Height = 110, SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };

    public FrmCategoriaEditar(ICategoriaServicio servicio, Categoria categoria)
        : base(categoria == null ? "Nueva categoría" : "Editar categoría")
    {
        _servicio = servicio;
        _categoria = categoria ?? new Categoria();
        ClientSize = new Size(480, 700);

        AgregarCampo("Nombre", "Nombre *", txtNombre);
        AgregarCampo("Descripcion", "Descripción *", txtDescripcion);
        AgregarCampo("Fotos", "Fotos (máx. 5, jpg/png, 2 MB c/u)", lstFotos);

        var btnAgregar = Estilo.Boton("Agregar foto", Estilo.Secundario, 130);
        var btnQuitar = Estilo.Boton("Quitar foto", Estilo.Peligro, 130);
        btnAgregar.Click += (s, e) => AgregarFoto();
        btnQuitar.Click += (s, e) => QuitarFoto();
        var botones = new FlowLayoutPanel { Width = 380, Height = 44 };
        botones.Controls.Add(btnAgregar);
        botones.Controls.Add(btnQuitar);
        AgregarControl(botones);
        AgregarControl(picVista);

        txtNombre.Text = _categoria.Nombre;
        txtDescripcion.Text = _categoria.Descripcion;
        foreach (string ruta in _categoria.Fotos) lstFotos.Items.Add(ruta);
        lstFotos.SelectedIndexChanged += (s, e) => MostrarVista();
    }

    private void AgregarFoto()
    {
        using var dialogo = new OpenFileDialog { Filter = "Imágenes|*.jpg;*.jpeg;*.png", Title = "Seleccione una foto" };
        if (dialogo.ShowDialog(this) == DialogResult.OK) lstFotos.Items.Add(dialogo.FileName);
    }

    private void QuitarFoto()
    {
        if (lstFotos.SelectedIndex >= 0) lstFotos.Items.RemoveAt(lstFotos.SelectedIndex);
        MostrarVista();
    }

    private void MostrarVista()
    {
        picVista.Image?.Dispose();
        picVista.Image = null;
        if (lstFotos.SelectedItem == null) return;
        try
        {
            using var flujo = new FileStream((string)lstFotos.SelectedItem, FileMode.Open, FileAccess.Read);
            using var imagen = Image.FromStream(flujo);
            picVista.Image = new Bitmap(imagen);
        }
        catch (Exception) { /* si la foto no se puede mostrar, se deja vacío */ }
    }

    protected override Dictionary<string, string> GuardarEntidad()
    {
        _categoria.Nombre = txtNombre.Text;
        _categoria.Descripcion = txtDescripcion.Text;
        _categoria.Fotos = lstFotos.Items.Cast<string>().ToList();
        return _servicio.Guardar(_categoria);
    }
}
