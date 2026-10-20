using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.Presentacion;

/// <summary>Listado de categorías.</summary>
public class FrmCategorias : FrmListadoBase
{
    private readonly ICategoriaServicio _servicio;

    public FrmCategorias(ICategoriaServicio servicio) : base("Categorías")
    {
        _servicio = servicio;
    }

    protected override void Cargar() =>
        grid.DataSource = _servicio.Listar(txtBuscar.Text)
            .Select(c => new { c.Id, c.Nombre, c.Descripcion, Creada = c.FechaCreacion, Modificada = c.FechaModificacion })
            .ToList();

    protected override void Nuevo()
    {
        using var f = new FrmCategoriaEditar(_servicio, null);
        if (f.ShowDialog(this) == DialogResult.OK) Refrescar();
    }

    protected override void Editar(int id)
    {
        using var f = new FrmCategoriaEditar(_servicio, _servicio.ObtenerPorId(id));
        if (f.ShowDialog(this) == DialogResult.OK) Refrescar();
    }

    protected override string Eliminar(int id) => _servicio.Eliminar(id);
}
