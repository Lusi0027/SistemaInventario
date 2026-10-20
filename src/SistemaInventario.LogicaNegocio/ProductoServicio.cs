using SistemaInventario.AccesoDatos;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio;

/// Reglas de negocio de productos
public class ProductoServicio : IProductoServicio
{
    private readonly IRepositorio<Producto> _repo;
    private readonly IRepositorio<Categoria> _repoCategorias;
    private readonly IRepositorio<Suplidor> _repoSuplidores;

    public ProductoServicio(IRepositorio<Producto> repo, IRepositorio<Categoria> repoCategorias, IRepositorio<Suplidor> repoSuplidores)
    {
        _repo = repo;
        _repoCategorias = repoCategorias;
        _repoSuplidores = repoSuplidores;
    }


    public List<ProductoVista> Listar(string texto, int? categoriaId, int? suplidorId, decimal? precioMin, decimal? precioMax)
    {
        // LINQ: join de productos con categorías y suplidores activos
        IEnumerable<ProductoVista> consulta =
            from p in _repo.ObtenerActivos()
            join c in _repoCategorias.ObtenerActivos() on p.CategoriaId equals c.Id
            join s in _repoSuplidores.ObtenerActivos() on p.SuplidorId equals s.Id
            select new ProductoVista
            {
                Id = p.Id, Nombre = p.Nombre, Precio = p.Precio,
                CategoriaId = c.Id, Categoria = c.Nombre,
                SuplidorId = s.Id, Suplidor = s.Empresa,
                FechaCreacion = p.FechaCreacion
            };

        if (categoriaId.HasValue) consulta = consulta.Where(v => v.CategoriaId == categoriaId.Value);
        if (suplidorId.HasValue) consulta = consulta.Where(v => v.SuplidorId == suplidorId.Value);
        if (precioMin.HasValue) consulta = consulta.Where(v => v.Precio >= precioMin.Value);
        if (precioMax.HasValue) consulta = consulta.Where(v => v.Precio <= precioMax.Value);

        return Utilidades.Buscar(consulta, texto, v => v.Nombre, v => v.Categoria, v => v.Suplidor)
                         .OrderBy(v => v.Nombre).ToList();
    }


    public Producto ObtenerPorId(int id) => _repo.ObtenerPorId(id);


    public Dictionary<string, string> Guardar(Producto p)
    {
        var errores = new Dictionary<string, string>();

        if (!Validador.LongitudValida(p.Nombre, 3, 100))
            errores["Nombre"] = "El nombre debe tener entre 3 y 100 caracteres.";

        if (!Validador.EsPrecioValido(p.Precio))
            errores["Precio"] = "El precio debe ser mayor a 0 y tener máximo 2 decimales.";

        if (!_repoCategorias.ObtenerActivos().Any(c => c.Id == p.CategoriaId))
            errores["CategoriaId"] = "Seleccione una categoría activa.";

        if (!_repoSuplidores.ObtenerActivos().Any(s => s.Id == p.SuplidorId))
            errores["SuplidorId"] = "Seleccione un suplidor activo.";

        if (errores.Count > 0) return errores;

        p.Nombre = p.Nombre.Trim();
        if (p.Id == 0) _repo.Insertar(p); else _repo.Actualizar(p);
        return errores;
    }


    public void Eliminar(int id) => _repo.Eliminar(id);
}
