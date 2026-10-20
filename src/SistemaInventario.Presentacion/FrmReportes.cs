using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Muestra las consultas LINQ de resumen.</summary>
public class FrmReportes : Form
{
    private readonly (string Nombre, Func<List<FilaReporte>> Consulta)[] _reportes;
    private readonly ComboBox cmbReporte = new() { Width = 380, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(3, 7, 3, 3) };
    private readonly DataGridView grid = Estilo.Grid();

    public FrmReportes(IReporteServicio servicio)
    {
        _reportes = new (string, Func<List<FilaReporte>>)[]
        {
            ("Productos por categoría (cantidad)", servicio.ProductosPorCategoria),
            ("Top 5 suplidores con más productos", servicio.TopSuplidores),
            ("Promedio de precios por categoría", servicio.PromedioPrecioPorCategoria),
            ("Producto más caro por categoría", servicio.ProductoMasCaroPorCategoria),
            ("Suma de precios por suplidor", servicio.ValorInventarioPorSuplidor)
        };

        Text = "Reportes";
        Size = new Size(800, 520);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Estilo.Fondo;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = Estilo.Fuente;

        var encabezado = new Label
        {
            Text = "  Reportes", Dock = DockStyle.Top, Height = 54,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold), BackColor = Estilo.Encabezado,
            ForeColor = Estilo.Texto, TextAlign = ContentAlignment.MiddleLeft
        };

        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 54, Padding = new Padding(10, 8, 10, 8) };
        barra.Controls.Add(Estilo.Etiqueta("Reporte:"));
        barra.Controls.Add(cmbReporte);

        Controls.Add(grid);
        Controls.Add(barra);
        Controls.Add(encabezado);

        cmbReporte.Items.AddRange(_reportes.Select(r => (object)r.Nombre).ToArray());
        cmbReporte.SelectedIndexChanged += (s, e) => Mostrar();
        grid.DataBindingComplete += (s, e) =>
        {
            grid.Columns["Valor"].DefaultCellStyle.Format = "N2";
            grid.Columns["Valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["Descripcion"].HeaderText = "Descripción";
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        cmbReporte.SelectedIndex = 0;
    }

    private void Mostrar()
    {
        try { grid.DataSource = _reportes[cmbReporte.SelectedIndex].Consulta(); }
        catch (Exception ex) { Estilo.Error(ex.Message); }
    }
}
