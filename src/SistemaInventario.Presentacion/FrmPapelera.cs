using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Papelera: muestra los registros eliminados y permite restaurarlos.</summary>
public class FrmPapelera : Form
{
    private readonly IPapeleraServicio _servicio;
    private readonly TextBox txtBuscar = new() { Width = 220, Margin = new Padding(3, 7, 15, 3) };
    private readonly DataGridView grid = Estilo.Grid();

    public FrmPapelera(IPapeleraServicio servicio)
    {
        _servicio = servicio;
        Text = "Papelera";
        Size = new Size(850, 550);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Estilo.Fondo;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = Estilo.Fuente;

        var encabezado = new Label
        {
            Text = "  Papelera", Dock = DockStyle.Top, Height = 54,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold), BackColor = Estilo.Encabezado,
            ForeColor = Estilo.Texto, TextAlign = ContentAlignment.MiddleLeft
        };

        var btnRestaurar = Estilo.Boton("Restaurar", Estilo.Primario);
        btnRestaurar.Click += (s, e) => Restaurar();

        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 54, Padding = new Padding(10, 8, 10, 8) };
        barra.Controls.AddRange(new Control[] { Estilo.Etiqueta("Buscar:"), txtBuscar, btnRestaurar });

        Controls.Add(grid);
        Controls.Add(barra);
        Controls.Add(encabezado);

        txtBuscar.TextChanged += (s, e) => Cargar();
        grid.DataBindingComplete += (s, e) =>
        {
            grid.Columns["Id"].Visible = false;
            grid.Columns["FechaModificacion"].HeaderText = "Eliminado el";
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Cargar();
    }

    private void Cargar()
    {
        try { grid.DataSource = _servicio.Listar(txtBuscar.Text); }
        catch (Exception ex) { Estilo.Error(ex.Message); }
    }

    private void Restaurar()
    {
        if (grid.CurrentRow?.DataBoundItem is not ItemPapelera item)
        {
            Estilo.Aviso("Seleccione un registro de la tabla.");
            return;
        }
        try
        {
            string error = _servicio.Restaurar(item);
            if (error != null) Estilo.Aviso(error);
            Cargar();
        }
        catch (Exception ex) { Estilo.Error(ex.Message); }
    }
}
