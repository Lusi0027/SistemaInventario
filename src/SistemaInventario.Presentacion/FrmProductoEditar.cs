using System.Globalization;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Crear o editar un producto.</summary>
public class FrmProductoEditar : FrmEditarBase
{
    private readonly IProductoServicio _servicio;
    private readonly Producto _producto;
    private readonly TextBox txtNombre = new();
    private readonly TextBox txtPrecio = new();
    private readonly ComboBox cmbCategoria = new() { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Nombre", ValueMember = "Id" };
    private readonly ComboBox cmbSuplidor = new() { DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "Empresa", ValueMember = "Id" };

    public FrmProductoEditar(IProductoServicio servicio, ICategoriaServicio categorias, ISuplidorServicio suplidores, Producto producto)
        : base(producto == null ? "Nuevo producto" : "Editar producto")
    {
        _servicio = servicio;
        _producto = producto ?? new Producto();
        ClientSize = new Size(480, 440);

        AgregarCampo("Nombre", "Nombre del producto *", txtNombre);
        AgregarCampo("Precio", "Precio unitario * (mayor a 0)", txtPrecio);
        AgregarCampo("CategoriaId", "Categoría *", cmbCategoria);
        AgregarCampo("SuplidorId", "Suplidor *", cmbSuplidor);

        cmbCategoria.DataSource = categorias.Listar("");
        cmbSuplidor.DataSource = suplidores.Listar("");

        if (producto != null)
        {
            txtNombre.Text = producto.Nombre;
            txtPrecio.Text = producto.Precio.ToString("0.##", CultureInfo.CurrentCulture);
            cmbCategoria.SelectedValue = producto.CategoriaId;
            cmbSuplidor.SelectedValue = producto.SuplidorId;
        }
    }

    protected override Dictionary<string, string> GuardarEntidad()
    {
        if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal precio))
            return new Dictionary<string, string> { ["Precio"] = "Ingrese un número válido." };

        _producto.Nombre = txtNombre.Text;
        _producto.Precio = precio;
        _producto.CategoriaId = cmbCategoria.SelectedValue is int cat ? cat : 0;
        _producto.SuplidorId = cmbSuplidor.SelectedValue is int sup ? sup : 0;
        return _servicio.Guardar(_producto);
    }
}
