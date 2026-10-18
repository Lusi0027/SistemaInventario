using System.Globalization;
using SistemaInventario.Entidades;
using SistemaInventario.Entidades.Excepciones;
using SistemaInventario.LogicaNegocio;

namespace SistemaInventario.Presentacion;

/// <summary>Alta y edición de productos.</summary>
public class FormProducto : FormEdicionBase
{
    private readonly ContenedorServicios _servicios;
    private readonly Producto _producto;
    private readonly bool _esNuevo;
    private readonly TextBox _txtNombre = new() { MaxLength = 150 };
    private readonly TextBox _txtPrecio = new() { PlaceholderText = "0.00" };
    private readonly ComboBox _cboCategoria = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox _cboSuplidor = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public FormProducto(ContenedorServicios servicios, Producto? producto = null)
        : base(producto is null ? "Nuevo producto" : "Editar producto")
    {
        _servicios = servicios;
        _esNuevo = producto is null;
        _producto = producto ?? new Producto();

        _cboCategoria.DisplayMember = nameof(Categoria.Nombre);
        _cboCategoria.ValueMember = nameof(Categoria.Id);
        _cboCategoria.DataSource = servicios.Categorias.Listar().ToList();
        _cboSuplidor.DisplayMember = nameof(Suplidor.NombreEmpresa);
        _cboSuplidor.ValueMember = nameof(Suplidor.Id);
        _cboSuplidor.DataSource = servicios.Suplidores.Listar().ToList();

        AgregarCampo(nameof(Producto.Nombre), "Nombre *", _txtNombre);
        AgregarCampo(nameof(Producto.PrecioUnitario), "Precio unitario *", _txtPrecio);
        AgregarCampo(nameof(Producto.CategoriaId), "Categoría *", _cboCategoria);
        AgregarCampo(nameof(Producto.SuplidorId), "Suplidor *", _cboSuplidor);
        Construir();

        _txtNombre.Text = _producto.Nombre;
        if (_esNuevo)
        {
            _cboCategoria.SelectedIndex = -1;
            _cboSuplidor.SelectedIndex = -1;
        }
        else
        {
            _txtPrecio.Text = _producto.PrecioUnitario.ToString("0.##", CultureInfo.CurrentCulture);
            _cboCategoria.SelectedValue = _producto.CategoriaId;
            _cboSuplidor.SelectedValue = _producto.SuplidorId;
        }
    }

    protected override void Guardar()
    {
        if (!decimal.TryParse(_txtPrecio.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var precio))
            throw new ValidacionException(new Dictionary<string, string>
            {
                [nameof(Producto.PrecioUnitario)] = "El precio no es un número válido."
            });

        _producto.Nombre = _txtNombre.Text;
        _producto.PrecioUnitario = precio;
        _producto.CategoriaId = _cboCategoria.SelectedValue is int c ? c : 0;
        _producto.SuplidorId = _cboSuplidor.SelectedValue is int s ? s : 0;

        if (_esNuevo) _servicios.Productos.Crear(_producto);
        else _servicios.Productos.Actualizar(_producto);
    }
}
