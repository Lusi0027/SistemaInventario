using SistemaInventario.AccesoDatos;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio;

/// <summary>Lista y restaura los registros eliminados.</summary>
public class PapeleraServicio : IPapeleraServicio
{
    private readonly IRepositorio<Categoria> _categorias;
    private readonly IRepositorio<Suplidor> _suplidores;
    private readonly IRepositorio<Producto> _productos;

    public PapeleraServicio(IRepositorio<Categoria> categorias, IRepositorio<Suplidor> suplidores, IRepositorio<Producto> productos)
    {
        _categorias = categorias;
        _suplidores = suplidores;
        _productos = productos;
    }

    /// <inheritdoc/>
    public List<ItemPapelera> Listar(string texto)
    {
        var cats = _categorias.ObtenerEliminados().Select(c =>
            new ItemPapelera { Id = c.Id, Tipo = "Categoría", Nombre = c.Nombre, FechaModificacion = c.FechaModificacion });
        var sups = _suplidores.ObtenerEliminados().Select(s =>
            new ItemPapelera { Id = s.Id, Tipo = "Suplidor", Nombre = s.Empresa, FechaModificacion = s.FechaModificacion });
        var prods = _productos.ObtenerEliminados().Select(p =>
            new ItemPapelera { Id = p.Id, Tipo = "Producto", Nombre = p.Nombre, FechaModificacion = p.FechaModificacion });

        return Utilidades.Buscar(cats.Concat(sups).Concat(prods), texto, i => i.Nombre, i => i.Tipo)
                         .OrderByDescending(i => i.FechaModificacion).ToList();
    }

    /// <inheritdoc/>
    public string Restaurar(ItemPapelera item)
    {
        switch (item.Tipo)
        {
            case "Categoría":
                _categorias.Restaurar(item.Id);
                break;
            case "Suplidor":
                _suplidores.Restaurar(item.Id);
                break;
            case "Producto":
                var p = _productos.ObtenerPorId(item.Id);
                if (!_categorias.ObtenerActivos().Any(c => c.Id == p.CategoriaId))
                    return "No se puede restaurar: la categoría del producto está en la papelera. Restáurela primero.";
                if (!_suplidores.ObtenerActivos().Any(s => s.Id == p.SuplidorId))
                    return "No se puede restaurar: el suplidor del producto está en la papelera. Restáurelo primero.";
                _productos.Restaurar(item.Id);
                break;
        }
        return null;
    }
}
