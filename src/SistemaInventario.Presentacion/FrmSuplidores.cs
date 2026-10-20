using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Listado de suplidores.</summary>
public class FrmSuplidores : FrmListadoBase
{
    private readonly ISuplidorServicio _servicio;

    public FrmSuplidores(ISuplidorServicio servicio) : base("Suplidores")
    {
        _servicio = servicio;
    }

    protected override void Cargar() =>
        grid.DataSource = _servicio.Listar(txtBuscar.Text)
            .Select(s => new { s.Id, s.Empresa, s.Contacto, s.Telefono, s.Correo, s.SitioWeb })
            .ToList();

    protected override void Nuevo()
    {
        using var f = new FrmSuplidorEditar(_servicio, null);
        if (f.ShowDialog(this) == DialogResult.OK) Refrescar();
    }

    protected override void Editar(int id)
    {
        using var f = new FrmSuplidorEditar(_servicio, _servicio.ObtenerPorId(id));
        if (f.ShowDialog(this) == DialogResult.OK) Refrescar();
    }

    protected override string Eliminar(int id) => _servicio.Eliminar(id);
}
