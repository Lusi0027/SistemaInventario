using SistemaInventario.AccesoDatos;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio;

/// <summary>
/// Raíz de composición: crea los repositorios e inyecta las dependencias por constructor.
/// Así la capa de Presentación nunca necesita referenciar Acceso a Datos.
/// </summary>
public class FabricaServicios
{
    public ICategoriaServicio Categorias { get; }
    public ISuplidorServicio Suplidores { get; }
    public IProductoServicio Productos { get; }
    public IUsuarioServicio Usuarios { get; }
    public IPapeleraServicio Papelera { get; }
    public IReporteServicio Reportes { get; }

    public FabricaServicios()
    {
        IRepositorio<Categoria> repoCategorias = new CategoriaRepositorio();
        IRepositorio<Suplidor> repoSuplidores = new SuplidorRepositorio();
        IRepositorio<Producto> repoProductos = new ProductoRepositorio();
        IRepositorio<Usuario> repoUsuarios = new UsuarioRepositorio();

        Categorias = new CategoriaServicio(repoCategorias, repoProductos);
        Suplidores = new SuplidorServicio(repoSuplidores, repoProductos);
        Productos = new ProductoServicio(repoProductos, repoCategorias, repoSuplidores);
        Usuarios = new UsuarioServicio(repoUsuarios);
        Papelera = new PapeleraServicio(repoCategorias, repoSuplidores, repoProductos);
        Reportes = new ReporteServicio(repoProductos, repoCategorias, repoSuplidores);
    }
}
