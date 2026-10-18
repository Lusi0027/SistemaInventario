using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;
using SistemaInventario.Entidades.Excepciones;
using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.LogicaNegocio.Utilidades;
using SistemaInventario.LogicaNegocio.Validadores;

namespace SistemaInventario.LogicaNegocio.Servicios;


public class ProductoServicio : IProductoServicio
{
    private readonly IProductoRepositorio _productos;
    private readonly ICategoriaRepositorio _categorias;
    private readonly ISuplidorRepositorio _suplidores;
    private readonly ProductoValidador _validador = new();

   
    public ProductoServicio(IProductoRepositorio productos, ICategoriaRepositorio categorias,
        ISuplidorRepositorio suplidores)
    {
        _productos = productos ?? throw new ArgumentNullException(nameof(productos));
        _categorias = categorias ?? throw new ArgumentNullException(nameof(categorias));
        _suplidores = suplidores ?? throw new ArgumentNullException(nameof(suplidores));
    }

  
    public IReadOnlyList<Producto> Listar(ProductoFiltro? filtro = null)
    {
        filtro ??= new ProductoFiltro();

        var consulta = Buscador.Filtrar(_productos.ObtenerActivos(), filtro.Texto,
            p => p.Nombre, p => p.CategoriaNombre, p => p.SuplidorNombre);

        if (filtro.CategoriaId is int categoriaId)
            consulta = consulta.Where(p => p.CategoriaId == categoriaId);
        if (filtro.SuplidorId is int suplidorId)
            consulta = consulta.Where(p => p.SuplidorId == suplidorId);
        if (filtro.PrecioMinimo is decimal minimo)
            consulta = consulta.Where(p => p.PrecioUnitario >= minimo);
        if (filtro.PrecioMaximo is decimal maximo)
            consulta = consulta.Where(p => p.PrecioUnitario <= maximo);

        return consulta.OrderBy(p => p.Nombre).ToList();
    }


    public IReadOnlyList<Producto> ListarEliminados(string? busqueda = null) =>
        Buscador.Filtrar(_productos.ObtenerEliminados(), busqueda,
            p => p.Nombre, p => p.CategoriaNombre, p => p.SuplidorNombre)
            .OrderBy(p => p.Nombre).ToList();


    public Producto? ObtenerPorId(int id) => _productos.ObtenerPorId(id);


    public int Crear(Producto producto)
    {
        Normalizar(producto);
        Validar(producto);
        return _productos.Insertar(producto);
    }

   
    public void Actualizar(Producto producto)
    {
        var existente = ObtenerExistente(producto.Id);
        if (!existente.Activo)
            throw new NegocioException("No se puede editar un producto eliminado. Restáurelo primero.");

        Normalizar(producto);
        Validar(producto);
        _productos.Actualizar(producto);
    }


    public void Eliminar(int id)
    {
        var producto = ObtenerExistente(id);
        if (producto.Activo) _productos.EliminarLogico(id);
    }


    public void Restaurar(int id)
    {
        var producto = ObtenerExistente(id);
        if (producto.Activo) return;

        var categoria = _categorias.ObtenerPorId(producto.CategoriaId);
        if (categoria is null || !categoria.Activo)
            throw new NegocioException(
                $"No se puede restaurar: la categoría '{producto.CategoriaNombre}' está eliminada. Restáurela primero.");

        var suplidor = _suplidores.ObtenerPorId(producto.SuplidorId);
        if (suplidor is null || !suplidor.Activo)
            throw new NegocioException(
                $"No se puede restaurar: el suplidor '{producto.SuplidorNombre}' está eliminado. Restáurelo primero.");

        _productos.Restaurar(id);
    }

    private Producto ObtenerExistente(int id) =>
        _productos.ObtenerPorId(id) ?? throw new NegocioException("El producto no existe.");

    private static void Normalizar(Producto p) => p.Nombre = p.Nombre?.Trim() ?? string.Empty;

    private void Validar(Producto producto)
    {
        var errores = _validador.Validar(producto);

        if (!errores.ContainsKey(nameof(Producto.CategoriaId)))
        {
            var categoria = _categorias.ObtenerPorId(producto.CategoriaId);
            if (categoria is null || !categoria.Activo)
                errores[nameof(Producto.CategoriaId)] = "La categoría seleccionada no existe o está eliminada.";
        }

        if (!errores.ContainsKey(nameof(Producto.SuplidorId)))
        {
            var suplidor = _suplidores.ObtenerPorId(producto.SuplidorId);
            if (suplidor is null || !suplidor.Activo)
                errores[nameof(Producto.SuplidorId)] = "El suplidor seleccionado no existe o está eliminado.";
        }

        if (errores.Count > 0) throw new ValidacionException(errores);
    }
}
