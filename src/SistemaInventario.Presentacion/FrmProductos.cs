using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Listado de productos con búsqueda y filtros por categoría, suplidor y rango de precios.</summary>
public class FrmProductos : FrmListadoBase
{
    private readonly IProductoServicio _servicio;
    private readonly ICategoriaServicio _categorias;
    private readonly ISuplidorServicio _suplidores;
    private readonly ComboBox cmbCategoria = new() { Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Nombre" };
    private readonly ComboBox cmbSuplidor = new() { Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Empresa" };
    private readonly NumericUpDown numMin = new() { Width = 90, DecimalPlaces = 2, Maximum = 100000000 };
    private readonly NumericUpDown numMax = new() { Width = 90, DecimalPlaces = 2, Maximum = 100000000 };

    public FrmProductos(IProductoServicio servicio, ICategoriaServicio categorias, ISuplidorServicio suplidores)
        : base("Productos")
    {
        _servicio = servicio;
        _categorias = categorias;
        _suplidores = suplidores;

        var btnLimpiar = Estilo.Boton("Limpiar", Estilo.Lila, 90);
        btnLimpiar.Click += (s, e) => LimpiarFiltros();

        barra.Controls.Add(Estilo.Etiqueta("Categoría:"));
        barra.Controls.Add(cmbCategoria);
        barra.Controls.Add(Estilo.Etiqueta("Suplidor:"));
        barra.Controls.Add(cmbSuplidor);
        barra.Controls.Add(Estilo.Etiqueta("Precio desde:"));
        barra.Controls.Add(numMin);
        barra.Controls.Add(Estilo.Etiqueta("hasta:"));
        barra.Controls.Add(numMax);
        barra.Controls.Add(btnLimpiar);
        foreach (Control c in new Control[] { cmbCategoria, cmbSuplidor, numMin, numMax }) c.Margin = new Padding(3, 7, 3, 3);

        CargarCombos();
        cmbCategoria.SelectedIndexChanged += (s, e) => Refrescar();
        cmbSuplidor.SelectedIndexChanged += (s, e) => Refrescar();
        numMin.ValueChanged += (s, e) => Refrescar();
        numMax.ValueChanged += (s, e) => Refrescar();
    }

    private void CargarCombos()
    {
        var categorias = _categorias.Listar("");
        categorias.Insert(0, new Categoria { Id = 0, Nombre = "— Todas —" });
        cmbCategoria.DataSource = categorias;

        var suplidores = _suplidores.Listar("");
        suplidores.Insert(0, new Suplidor { Id = 0, Empresa = "— Todos —" });
        cmbSuplidor.DataSource = suplidores;
    }

    private void LimpiarFiltros()
    {
        txtBuscar.Clear();
        cmbCategoria.SelectedIndex = 0;
        cmbSuplidor.SelectedIndex = 0;
        numMin.Value = 0;
        numMax.Value = 0;
    }

    protected override void Cargar()
    {
        int categoriaId = (cmbCategoria.SelectedItem as Categoria)?.Id ?? 0;
        int suplidorId = (cmbSuplidor.SelectedItem as Suplidor)?.Id ?? 0;
        // 0 significa "sin filtro"
        int? idCategoria = categoriaId > 0 ? categoriaId : null;
        int? idSuplidor = suplidorId > 0 ? suplidorId : null;
        decimal? min = numMin.Value > 0 ? numMin.Value : null;
        decimal? max = numMax.Value > 0 ? numMax.Value : null;

        grid.DataSource = _servicio.Listar(txtBuscar.Text, idCategoria, idSuplidor, min, max)
            .Select(v => new { v.Id, v.Nombre, v.Precio, v.Categoria, v.Suplidor, Creado = v.FechaCreacion })
            .ToList();
        grid.Columns["Precio"].DefaultCellStyle.Format = "N2";
        grid.Columns["Precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    protected override void Nuevo()
    {
        using var f = new FrmProductoEditar(_servicio, _categorias, _suplidores, null);
        if (f.ShowDialog(this) == DialogResult.OK) Refrescar();
    }

    protected override void Editar(int id)
    {
        using var f = new FrmProductoEditar(_servicio, _categorias, _suplidores, _servicio.ObtenerPorId(id));
        if (f.ShowDialog(this) == DialogResult.OK) Refrescar();
    }

    protected override string Eliminar(int id)
    {
        _servicio.Eliminar(id);
        return null;
    }
}
