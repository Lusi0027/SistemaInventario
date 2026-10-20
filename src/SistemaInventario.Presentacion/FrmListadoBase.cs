namespace SistemaInventario.Presentacion;

/// <summary>
/// Formulario base de los listados: encabezado, búsqueda en tiempo real, tabla y botones
/// Nuevo / Editar / Eliminar. Cada listado solo implementa lo que cambia.
/// </summary>
public abstract class FrmListadoBase : Form
{
    protected readonly TextBox txtBuscar = new() { Width = 220, Margin = new Padding(3, 7, 15, 3) };
    protected readonly DataGridView grid = Estilo.Grid();
    protected readonly FlowLayoutPanel barra = new()
    {
        Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
        Padding = new Padding(10, 8, 10, 8)
    };
    protected readonly Button btnNuevo = Estilo.Boton("Nuevo", Estilo.Primario);
    protected readonly Button btnEditar = Estilo.Boton("Editar", Estilo.Secundario);
    protected readonly Button btnEliminar = Estilo.Boton("Eliminar", Estilo.Peligro);

    protected FrmListadoBase(string titulo)
    {
        Text = titulo;
        Size = new Size(1000, 600);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Estilo.Fondo;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = Estilo.Fuente;

        var encabezado = new Label
        {
            Text = "  " + titulo, Dock = DockStyle.Top, Height = 54,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold), BackColor = Estilo.Encabezado,
            ForeColor = Estilo.Texto, TextAlign = ContentAlignment.MiddleLeft
        };

        barra.Controls.AddRange(new Control[] { Estilo.Etiqueta("Buscar:"), txtBuscar, btnNuevo, btnEditar, btnEliminar });

        // El orden importa: primero lo que ocupa el centro, luego lo de arriba
        Controls.Add(grid);
        Controls.Add(barra);
        Controls.Add(encabezado);

        txtBuscar.TextChanged += (s, e) => Refrescar();
        btnNuevo.Click += (s, e) => Nuevo();
        btnEditar.Click += (s, e) => AlEditar();
        btnEliminar.Click += (s, e) => AlEliminar();
        grid.DoubleClick += (s, e) => AlEditar();
        grid.DataBindingComplete += (s, e) =>
        {
            if (grid.Columns.Contains("Id")) grid.Columns["Id"].Visible = false;
        };
    }

    protected abstract void Cargar();
    protected abstract void Nuevo();
    protected abstract void Editar(int id);
    /// <summary>Elimina el registro. Devuelve un mensaje de error o null si se eliminó.</summary>
    protected abstract string Eliminar(int id);

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Refrescar();
    }

    /// <summary>Recarga la tabla mostrando un mensaje si falla la base de datos.</summary>
    protected void Refrescar()
    {
        try { Cargar(); }
        catch (Exception ex) { Estilo.Error(ex.Message); }
    }

    /// <summary>Id de la fila seleccionada (null si no hay).</summary>
    protected int? IdSeleccionado() =>
        grid.CurrentRow == null ? null : (int?)Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);

    private void AlEditar()
    {
        int? id = IdSeleccionado();
        if (id == null) { Estilo.Aviso("Seleccione un registro de la tabla."); return; }
        Editar(id.Value);
    }

    private void AlEliminar()
    {
        int? id = IdSeleccionado();
        if (id == null) { Estilo.Aviso("Seleccione un registro de la tabla."); return; }
        if (!Estilo.Confirmar("¿Desea enviar este registro a la papelera?")) return;
        try
        {
            string error = Eliminar(id.Value);
            if (error != null) Estilo.Aviso(error);
            Refrescar();
        }
        catch (Exception ex) { Estilo.Error(ex.Message); }
    }
}
