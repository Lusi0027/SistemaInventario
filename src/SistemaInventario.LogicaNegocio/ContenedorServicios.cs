using SistemaInventario.AccesoDatos;
using SistemaInventario.AccesoDatos.Repositorios;
using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.LogicaNegocio.Servicios;

namespace SistemaInventario.LogicaNegocio;

/// Composición de dependencias. Es el único punto que conoce las clases concretas de Acceso a Datos,
/// de modo que la capa de Presentación solo trabaja con las interfaces de servicios.
public sealed class ContenedorServicios
{
    public ICategoriaServicio Categorias { get; }
    public ISuplidorServicio Suplidores { get; }
    public IProductoServicio Productos { get; }
    public IReporteServicio Reportes { get; }

    private ContenedorServicios(ICategoriaServicio categorias, ISuplidorServicio suplidores,
        IProductoServicio productos, IReporteServicio reportes)
    {
        Categorias = categorias;
        Suplidores = suplidores;
        Productos = productos;
        Reportes = reportes;
    }

    /// Crea todos los servicios inyectando repositorios y el Singleton de conexión.
    /// Cadena de conexión a SQL Server.
    public static ContenedorServicios Crear(string cadenaConexion)
    {
        var conexion = ConexionBD.Instancia;
        conexion.Configurar(cadenaConexion);

        var repoCategorias = new RepositorioCategoria(conexion);
        var repoSuplidores = new RepositorioSuplidor(conexion);
        var repoProductos = new RepositorioProducto(conexion);

        var categorias = new CategoriaServicio(repoCategorias);
        var suplidores = new SuplidorServicio(repoSuplidores);
        var productos = new ProductoServicio(repoProductos, repoCategorias, repoSuplidores);
        var reportes = new ReporteServicio(repoProductos);

        return new ContenedorServicios(categorias, suplidores, productos, reportes);
    }
}