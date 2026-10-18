using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Listado de suplidores.</summary>
public class ControlSuplidores : ControlListadoBase
{
    private readonly ISuplidorServicio _servicio;

    public ControlSuplidores(ISuplidorServicio servicio) : base("Suplidores")
    {
        _servicio = servicio;
    }

    protected override object ObtenerDatos(string? texto) =>
        _servicio.Listar(texto).Select(s => new
        {
            s.Id,
            Empresa = s.NombreEmpresa,
            Contacto = s.NombreContacto,
            Teléfono = s.Telefono,
            Correo = s.Correo,
            Web = s.SitioWeb,
            Creación = s.FechaCreacion,
            Modificación = s.FechaModificacion
        }).ToList();

    protected override void Nuevo()
    {
        using var form = new FormSuplidor(_servicio);
        if (form.ShowDialog(this) == DialogResult.OK) Recargar();
    }

    protected override void Editar(int id)
    {
        var suplidor = _servicio.ObtenerPorId(id);
        if (suplidor is null) { Ui.Aviso("El suplidor ya no existe."); Recargar(); return; }

        using var form = new FormSuplidor(_servicio, suplidor);
        if (form.ShowDialog(this) == DialogResult.OK) Recargar();
    }

    protected override void Eliminar(int id) => _servicio.Eliminar(id);
}
