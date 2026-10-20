using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Crear o editar un suplidor.</summary>
public class FrmSuplidorEditar : FrmEditarBase
{
    private readonly ISuplidorServicio _servicio;
    private readonly Suplidor _suplidor;
    private readonly TextBox txtEmpresa = new();
    private readonly TextBox txtContacto = new();
    private readonly TextBox txtTelefono = new();
    private readonly TextBox txtCorreo = new();
    private readonly TextBox txtSitioWeb = new();

    public FrmSuplidorEditar(ISuplidorServicio servicio, Suplidor suplidor)
        : base(suplidor == null ? "Nuevo suplidor" : "Editar suplidor")
    {
        _servicio = servicio;
        _suplidor = suplidor ?? new Suplidor();
        ClientSize = new Size(480, 520);

        AgregarCampo("Empresa", "Nombre de la empresa *", txtEmpresa);
        AgregarCampo("Contacto", "Nombre del contacto *", txtContacto);
        AgregarCampo("Telefono", "Teléfono * (809-XXX-XXXX)", txtTelefono);
        AgregarCampo("Correo", "Correo electrónico *", txtCorreo);
        AgregarCampo("SitioWeb", "Sitio web (opcional)", txtSitioWeb);

        txtEmpresa.Text = _suplidor.Empresa;
        txtContacto.Text = _suplidor.Contacto;
        txtTelefono.Text = _suplidor.Telefono;
        txtCorreo.Text = _suplidor.Correo;
        txtSitioWeb.Text = _suplidor.SitioWeb;
    }

    protected override Dictionary<string, string> GuardarEntidad()
    {
        _suplidor.Empresa = txtEmpresa.Text;
        _suplidor.Contacto = txtContacto.Text;
        _suplidor.Telefono = txtTelefono.Text;
        _suplidor.Correo = txtCorreo.Text;
        _suplidor.SitioWeb = txtSitioWeb.Text;
        return _servicio.Guardar(_suplidor);
    }
}
