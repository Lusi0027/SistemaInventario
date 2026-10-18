using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Alta y edición de suplidores.</summary>
public class FormSuplidor : FormEdicionBase
{
    private readonly ISuplidorServicio _servicio;
    private readonly Suplidor _suplidor;
    private readonly bool _esNuevo;
    private readonly TextBox _txtEmpresa = new() { MaxLength = 150 };
    private readonly TextBox _txtContacto = new() { MaxLength = 100 };
    private readonly TextBox _txtTelefono = new() { MaxLength = 12, PlaceholderText = "809-555-0100" };
    private readonly TextBox _txtCorreo = new() { MaxLength = 150, PlaceholderText = "correo@empresa.com" };
    private readonly TextBox _txtWeb = new() { MaxLength = 200, PlaceholderText = "https://empresa.com (opcional)" };

    public FormSuplidor(ISuplidorServicio servicio, Suplidor? suplidor = null)
        : base(suplidor is null ? "Nuevo suplidor" : "Editar suplidor")
    {
        _servicio = servicio;
        _esNuevo = suplidor is null;
        _suplidor = suplidor ?? new Suplidor();

        AgregarCampo(nameof(Suplidor.NombreEmpresa), "Empresa *", _txtEmpresa);
        AgregarCampo(nameof(Suplidor.NombreContacto), "Contacto *", _txtContacto);
        AgregarCampo(nameof(Suplidor.Telefono), "Teléfono *", _txtTelefono);
        AgregarCampo(nameof(Suplidor.Correo), "Correo *", _txtCorreo);
        AgregarCampo(nameof(Suplidor.SitioWeb), "Sitio web", _txtWeb);
        Construir();

        _txtEmpresa.Text = _suplidor.NombreEmpresa;
        _txtContacto.Text = _suplidor.NombreContacto;
        _txtTelefono.Text = _suplidor.Telefono;
        _txtCorreo.Text = _suplidor.Correo;
        _txtWeb.Text = _suplidor.SitioWeb;
    }

    protected override void Guardar()
    {
        _suplidor.NombreEmpresa = _txtEmpresa.Text;
        _suplidor.NombreContacto = _txtContacto.Text;
        _suplidor.Telefono = _txtTelefono.Text;
        _suplidor.Correo = _txtCorreo.Text;
        _suplidor.SitioWeb = _txtWeb.Text;

        if (_esNuevo) _servicio.Crear(_suplidor);
        else _servicio.Actualizar(_suplidor);
    }
}
