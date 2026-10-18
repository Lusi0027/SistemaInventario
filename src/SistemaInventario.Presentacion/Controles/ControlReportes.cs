using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Reportes basados en las consultas LINQ de la capa de negocio.</summary>
public class ControlReportes : UserControl
{
    private readonly ContenedorServicios _servicios;
    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill, Padding = new Point(16, 6) };
    private readonly Dictionary<TabPage, Action> _cargadores = new();

    public ControlReportes(ContenedorServicios servicios)
    {
        _servicios = servicios;
        Dock = DockStyle.Fill;
        BackColor = Color.White;
        Font = Ui.Fuente;

        AgregarReporte("Productos por categoría", () => servicios.Reportes.ProductosPorCategoria());
        AgregarReporte("Top 5 suplidores", () => servicios.Reportes.TopSuplidores(5));
        AgregarReporte("Precio promedio", () => servicios.Reportes.PromedioPrecioPorCategoria());
        AgregarReporte("Más caro por categoría", () => servicios.Reportes.ProductoMasCaroPorCategoria());
        AgregarReporteRango();
        AgregarReporteBusqueda();

        var titulo = new Label
        {
            Text = "Reportes",
            Dock = DockStyle.Top,
            Height = 52,
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            Padding = new Padding(16, 12, 0, 0)
        };
        Controls.Add(_tabs);
        Controls.Add(titulo);

        _tabs.SelectedIndexChanged += (_, _) => CargarPestanaActual();
        Load += (_, _) => CargarPestanaActual();
    }

    private void CargarPestanaActual()
    {
        if (_tabs.SelectedTab is { } tab && _cargadores.TryGetValue(tab, out var cargar))
            Ui.Ejecutar(cargar);
    }

    private static DataGridView CrearGrid()
    {
        var grid = new DataGridView { Dock = DockStyle.Fill };
        Ui.EstilizarGrid(grid);
        return grid;
    }

    private void AgregarReporte(string titulo, Func<object> datos)
    {
        var grid = CrearGrid();
        var tab = new TabPage(titulo);
        tab.Controls.Add(grid);
        _tabs.TabPages.Add(tab);
        _cargadores[tab] = () => Ui.CargarGrid(grid, datos());
    }

    private void AgregarReporteRango()
    {
        var grid = CrearGrid();
        var txtMin = new TextBox { PlaceholderText = "Precio mín.", Width = 100, Margin = new Padding(0, 3, 8, 0) };
        var txtMax = new TextBox { PlaceholderText = "Precio máx.", Width = 100, Margin = new Padding(0, 3, 8, 0) };
        var cboCategoria = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180, Margin = new Padding(0, 3, 8, 0) };
        var btn = Ui.Boton("Consultar", Ui.Primario, 100);

        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(8, 8, 8, 0) };
        barra.Controls.AddRange(new Control[] { txtMin, txtMax, cboCategoria, btn });

        var tab = new TabPage("Rango y categoría");
        tab.Controls.Add(grid);
        tab.Controls.Add(barra);
        _tabs.TabPages.Add(tab);

        var categorias = new List<Categoria> { new() { Id = 0, Nombre = "Todas las categorías" } };
        Ui.Ejecutar(() => categorias.AddRange(_servicios.Categorias.Listar()));
        cboCategoria.DisplayMember = nameof(Categoria.Nombre);
        cboCategoria.ValueMember = nameof(Categoria.Id);
        cboCategoria.DataSource = categorias;

        void Consultar()
        {
            var minimo = Ui.LeerDecimalOpcional(txtMin.Text) ?? 0m;
            var maximo = Ui.LeerDecimalOpcional(txtMax.Text) ?? 9_999_999_999m;
            int? categoriaId = cboCategoria.SelectedValue is int c && c > 0 ? c : null;

            Ui.CargarGrid(grid, _servicios.Reportes.ProductosPorRangoYCategoria(minimo, maximo, categoriaId)
                .Select(p => new { Producto = p.Nombre, Precio = p.PrecioUnitario, Categoría = p.CategoriaNombre, Suplidor = p.SuplidorNombre })
                .ToList());
        }

        btn.Click += (_, _) => Ui.Ejecutar(Consultar);
        _cargadores[tab] = Consultar;
    }

    private void AgregarReporteBusqueda()
    {
        var grid = CrearGrid();
        var txt = new TextBox { PlaceholderText = "Buscar producto (ignora mayúsculas)...", Width = 300, Margin = new Padding(0, 3, 8, 0) };
        var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(8, 8, 8, 0) };
        barra.Controls.Add(txt);

        var tab = new TabPage("Búsqueda parcial");
        tab.Controls.Add(grid);
        tab.Controls.Add(barra);
        _tabs.TabPages.Add(tab);

        void Buscar() =>
            Ui.CargarGrid(grid, _servicios.Reportes.BuscarProductos(txt.Text)
                .Select(p => new { Producto = p.Nombre, Precio = p.PrecioUnitario, Categoría = p.CategoriaNombre, Suplidor = p.SuplidorNombre })
                .ToList());

        txt.TextChanged += (_, _) => Ui.Ejecutar(Buscar);
        _cargadores[tab] = Buscar;
    }
}
