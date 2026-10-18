using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Listado de productos con filtros por categoría, suplidor y rango de precios.</summary>
public class ControlProductos : ControlListadoBase
{
    private readonly ContenedorServicios _servicios;
    private readonly ComboBox _cboCategoria = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 170 };
    private readonly ComboBox _cboSuplidor = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 170 };
    private readonly TextBox _txtMinimo = new() { PlaceholderText = "Precio mín.", Width = 90 };
    private readonly TextBox _txtMaximo = new() { PlaceholderText = "Precio máx.", Width = 90 };

    public ControlProductos(ContenedorServicios servicios) : base("Productos")
    {
        _servicios = servicios;
        CargarCombos();

        AgregarFiltro(_cboCategoria);
        AgregarFiltro(_cboSuplidor);
        AgregarFiltro(_txtMinimo);
        AgregarFiltro(_txtMaximo);

        _cboCategoria.SelectedIndexChanged += (_, _) => Recargar();
        _cboSuplidor.SelectedIndexChanged += (_, _) => Recargar();
        _txtMinimo.TextChanged += (_, _) => Recargar();
        _txtMaximo.TextChanged += (_, _) => Recargar();
    }

    private void CargarCombos()
    {
        Ui.Ejecutar(() =>
        {
            var categorias = new List<Categoria> { new() { Id = 0, Nombre = "Todas las categorías" } };
            categorias.AddRange(_servicios.Categorias.Listar());
            _cboCategoria.DisplayMember = nameof(Categoria.Nombre);
            _cboCategoria.ValueMember = nameof(Categoria.Id);
            _cboCategoria.DataSource = categorias;

            var suplidores = new List<Suplidor> { new() { Id = 0, NombreEmpresa = "Todos los suplidores" } };
            suplidores.AddRange(_servicios.Suplidores.Listar());
            _cboSuplidor.DisplayMember = nameof(Suplidor.NombreEmpresa);
            _cboSuplidor.ValueMember = nameof(Suplidor.Id);
            _cboSuplidor.DataSource = suplidores;
        });
    }

    protected override object ObtenerDatos(string? texto)
    {
        int? categoriaId = _cboCategoria.SelectedValue is int c && c > 0 ? c : null;
        int? suplidorId = _cboSuplidor.SelectedValue is int s && s > 0 ? s : null;

        var filtro = new ProductoFiltro
        {
            Texto = texto,
            CategoriaId = categoriaId,
            SuplidorId = suplidorId,
            PrecioMinimo = Ui.LeerDecimalOpcional(_txtMinimo.Text),
            PrecioMaximo = Ui.LeerDecimalOpcional(_txtMaximo.Text)
        };

        return _servicios.Productos.Listar(filtro).Select(p => new
        {
            p.Id,
            p.Nombre,
            Precio = p.PrecioUnitario,
            Categoría = p.CategoriaNombre,
            Suplidor = p.SuplidorNombre,
            Creación = p.FechaCreacion,
            Modificación = p.FechaModificacion
        }).ToList();
    }

    protected override void Nuevo()
    {
        using var form = new FormProducto(_servicios);
        if (form.ShowDialog(this) == DialogResult.OK) Recargar();
    }

    protected override void Editar(int id)
    {
        var producto = _servicios.Productos.ObtenerPorId(id);
        if (producto is null) { Ui.Aviso("El producto ya no existe."); Recargar(); return; }

        using var form = new FormProducto(_servicios, producto);
        if (form.ShowDialog(this) == DialogResult.OK) Recargar();
    }

    protected override void Eliminar(int id) => _servicios.Productos.Eliminar(id);
}
