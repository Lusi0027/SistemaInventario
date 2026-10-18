using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.Presentacion.Utilidades;

namespace SistemaInventario.Presentacion;

/// <summary>Listado de categorías.</summary>
public class ControlCategorias : ControlListadoBase
{
    private readonly ICategoriaServicio _servicio;

    public ControlCategorias(ICategoriaServicio servicio) : base("Categorías")
    {
        _servicio = servicio;
    }

    protected override object ObtenerDatos(string? texto) =>
        _servicio.Listar(texto).Select(c => new
        {
            c.Id,
            c.Nombre,
            Descripción = c.Descripcion,
            Fotos = c.Fotos.Count,
            Creación = c.FechaCreacion,
            Modificación = c.FechaModificacion
        }).ToList();

    protected override void Nuevo()
    {
        using var form = new FormCategoria(_servicio);
        if (form.ShowDialog(this) == DialogResult.OK) Recargar();
    }

    protected override void Editar(int id)
    {
        var categoria = _servicio.ObtenerPorId(id);
        if (categoria is null) { Ui.Aviso("La categoría ya no existe."); Recargar(); return; }

        using var form = new FormCategoria(_servicio, categoria);
        if (form.ShowDialog(this) == DialogResult.OK) Recargar();
    }

    protected override void Eliminar(int id) => _servicio.Eliminar(id);
}
