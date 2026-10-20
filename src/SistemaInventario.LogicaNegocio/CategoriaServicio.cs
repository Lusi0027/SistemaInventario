using SistemaInventario.AccesoDatos;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio;

/// Reglas de negocio de categorías. Recibe los repositorios por constructor.
public class CategoriaServicio : ICategoriaServicio
{
    private readonly IRepositorio<Categoria> _repo;
    private readonly IRepositorio<Producto> _repoProductos;

    public CategoriaServicio(IRepositorio<Categoria> repo, IRepositorio<Producto> repoProductos)
    {
        _repo = repo;
        _repoProductos = repoProductos;
    }

  
    public List<Categoria> Listar(string texto) =>
        Utilidades.Buscar(_repo.ObtenerActivos(), texto, c => c.Nombre, c => c.Descripcion)
                  .OrderBy(c => c.Nombre).ToList();

 
    public Categoria ObtenerPorId(int id) => _repo.ObtenerPorId(id);


    public Dictionary<string, string> Guardar(Categoria c)
    {
        var errores = new Dictionary<string, string>();
        string nombre = (c.Nombre ?? "").Trim();

        if (!Validador.LongitudValida(nombre, 3, 100))
            errores["Nombre"] = "El nombre debe tener entre 3 y 100 caracteres.";
        else if (_repo.ObtenerActivos().Concat(_repo.ObtenerEliminados())
                      .Any(x => x.Id != c.Id && x.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            errores["Nombre"] = "Ya existe una categoría con ese nombre (revise también la papelera).";

        if (string.IsNullOrWhiteSpace(c.Descripcion) || c.Descripcion.Trim().Length > 500)
            errores["Descripcion"] = "La descripción es obligatoria (máximo 500 caracteres).";

        if (c.Fotos.Count > 5)
            errores["Fotos"] = "Se permiten máximo 5 fotos.";
        else
        {
            string errorFoto = c.Fotos.Select(Validador.ValidarFoto).FirstOrDefault(e => e != null);
            if (errorFoto != null) errores["Fotos"] = errorFoto;
        }

        if (errores.Count > 0) return errores;

        c.Nombre = nombre;
        c.Descripcion = c.Descripcion.Trim();
        c.Fotos = c.Fotos.Select(CopiarFoto).ToList();

        if (c.Id == 0) _repo.Insertar(c); else _repo.Actualizar(c);
        return errores;
    }


    public string Eliminar(int id)
    {
        if (_repoProductos.ObtenerActivos().Any(p => p.CategoriaId == id))
            return "No se puede eliminar: la categoría tiene productos activos asociados.";
        _repo.Eliminar(id);
        return null;
    }

    /// Copia la foto a la carpeta Fotos de la aplicación y devuelve la nueva ruta
    private static string CopiarFoto(string ruta)
    {
        string carpeta = Path.Combine(AppContext.BaseDirectory, "Fotos");
        if (Path.GetFullPath(ruta).StartsWith(Path.GetFullPath(carpeta), StringComparison.OrdinalIgnoreCase))
            return ruta; 

        Directory.CreateDirectory(carpeta);
        string destino = Path.Combine(carpeta, Guid.NewGuid() + Path.GetExtension(ruta));
        File.Copy(ruta, destino);
        return destino;
    }
}
