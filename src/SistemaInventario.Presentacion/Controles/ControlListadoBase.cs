using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Base de los listados: título, barra de búsqueda/botones y grid con filtrado en tiempo real.</summary>
public abstract class ControlListadoBase : UserControl
{
    protected readonly TextBox TxtBuscar = new() { PlaceholderText = "Buscar...", Width = 220, Margin = new Padding(0, 3, 12, 0) };
    protected readonly Button BtnNuevo = Ui.Boton("Nuevo", Ui.Primario, 90);
    protected readonly Button BtnEditar = Ui.Boton("Editar", Ui.Neutro, 90);
    protected readonly Button BtnEliminar = Ui.Boton("Eliminar", Ui.Peligro, 90);
    protected readonly FlowLayoutPanel Barra = new()
    {
        Dock = DockStyle.Top,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        WrapContents = true,
        Padding = new Padding(16, 4, 16, 8)
    };
    protected readonly DataGridView Grid = new() { Dock = DockStyle.Fill };
    private readonly System.Windows.Forms.Timer _retardo = new() { Interval = 250 };

    protected ControlListadoBase(string titulo)
    {
        Dock = DockStyle.Fill;
        BackColor = Color.White;
        Font = Ui.Fuente;
        Ui.EstilizarGrid(Grid);

        var lblTitulo = new Label
        {
            Text = titulo,
            Dock = DockStyle.Top,
            Height = 52,
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            Padding = new Padding(16, 12, 0, 0)
        };

        Barra.Controls.AddRange(new Control[] { TxtBuscar, BtnNuevo, BtnEditar, BtnEliminar });
        Controls.Add(Grid);
        Controls.Add(Barra);
        Controls.Add(lblTitulo);

        // Filtrado en tiempo real (con pequeña espera para no consultar en cada tecla).
        _retardo.Tick += (_, _) => { _retardo.Stop(); Recargar(); };
        TxtBuscar.TextChanged += (_, _) => { _retardo.Stop(); _retardo.Start(); };

        BtnNuevo.Click += (_, _) => Ui.Ejecutar(Nuevo);
        BtnEditar.Click += (_, _) => EditarSeleccionado();
        BtnEliminar.Click += (_, _) => EliminarSeleccionado();
        Grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };
        Load += (_, _) => Recargar();
    }

    /// <summary>Id de la fila seleccionada, o null.</summary>
    protected int? IdSeleccionado =>
        Grid.CurrentRow is { } fila && Grid.Columns.Contains("Id") ? (fila.Cells["Id"].Value as int?) : null;

    /// <summary>Datos a mostrar (colección con una propiedad Id) según el texto de búsqueda.</summary>
    protected abstract object ObtenerDatos(string? texto);

    /// <summary>Abre el diálogo de alta; debe llamar a <see cref="Recargar"/> si se guardó.</summary>
    protected abstract void Nuevo();

    /// <summary>Abre el diálogo de edición del registro.</summary>
    protected abstract void Editar(int id);

    /// <summary>Elimina lógicamente el registro.</summary>
    protected abstract void Eliminar(int id);

    /// <summary>Mensaje de confirmación antes de eliminar.</summary>
    protected virtual string MensajeEliminar => "¿Enviar el registro seleccionado a la Papelera?";

    /// <summary>Vuelve a consultar y pinta el grid.</summary>
    public void Recargar() => Ui.Ejecutar(() => Ui.CargarGrid(Grid, ObtenerDatos(TxtBuscar.Text)));

    /// <summary>Agrega un control de filtro a la barra, antes de los botones.</summary>
    protected void AgregarFiltro(Control control)
    {
        control.Margin = new Padding(0, 3, 12, 0);
        Barra.Controls.Add(control);
        Barra.Controls.SetChildIndex(control, Barra.Controls.IndexOf(BtnNuevo));
    }

    private void EditarSeleccionado()
    {
        if (IdSeleccionado is not int id) { Ui.Aviso("Seleccione un registro."); return; }
        Ui.Ejecutar(() => Editar(id));
    }

    private void EliminarSeleccionado()
    {
        if (IdSeleccionado is not int id) { Ui.Aviso("Seleccione un registro."); return; }
        if (!Ui.Confirmar(MensajeEliminar)) return;
        if (Ui.Ejecutar(() => Eliminar(id))) Recargar();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _retardo.Dispose();
        base.Dispose(disposing);
    }
}
