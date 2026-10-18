using SistemaInventario.AccesoDatos;
using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;
using SistemaInventario.Entidades.Excepciones;
using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.LogicaNegocio.Utilidades;
using SistemaInventario.LogicaNegocio.Validadores;

namespace SistemaInventario.LogicaNegocio.Servicios;

/// Reglas de negocio de categorías.
public class CategoriaServicio : ICategoriaServicio
{
    private readonly ICategoriaRepositorio _repositorio;
    private readonly CategoriaValidador _validador = new();

  
    public CategoriaServicio(ICategoriaRepositorio repositorio)
    {
        _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
    }


    public IReadOnlyList<Categoria> Listar(string? busqueda = null) =>
        Buscador.Filtrar(_repositorio.ObtenerActivos(), busqueda, c => c.Nombre, c => c.Descripcion)
            .OrderBy(c => c.Nombre).ToList();

    
    public IReadOnlyList<Categoria> ListarEliminadas(string? busqueda = null) =>
        Buscador.Filtrar(_repositorio.ObtenerEliminados(), busqueda, c => c.Nombre, c => c.Descripcion)
            .OrderBy(c => c.Nombre).ToList();


    public Categoria? ObtenerPorId(int id) => _repositorio.ObtenerPorId(id);

    public int Crear(Categoria categoria)
    {
        Normalizar(categoria);
        Validar(categoria, null);
        GuardarFotos(categoria);
        return _repositorio.Insertar(categoria);
    }

    public void Actualizar(Categoria categoria)
    {
        var existente = ObtenerExistente(categoria.Id);
        if (!existente.Activo)
            throw new NegocioException("No se puede editar una categoría eliminada. Restáurela primero.");

        Normalizar(categoria);
        Validar(categoria, categoria.Id);
        GuardarFotos(categoria);
        _repositorio.Actualizar(categoria);
    }

    public void Eliminar(int id)
    {
        var categoria = ObtenerExistente(id);
        if (!categoria.Activo) return;

        var productos = _repositorio.ContarProductosActivos(id);
        if (productos > 0)
            throw new NegocioException(
                $"No se puede eliminar la categoría '{categoria.Nombre}': tiene {productos} producto(s) activo(s).");

        _repositorio.EliminarLogico(id);
    }

   
    public void Restaurar(int id)
    {
        var categoria = ObtenerExistente(id);
        if (!categoria.Activo) _repositorio.Restaurar(id);
    }

    private Categoria ObtenerExistente(int id) =>
        _repositorio.ObtenerPorId(id) ?? throw new NegocioException("La categoría no existe.");

    private static void Normalizar(Categoria c)
    {
        c.Nombre = c.Nombre?.Trim() ?? string.Empty;
        c.Descripcion = c.Descripcion?.Trim() ?? string.Empty;
        c.Fotos ??= new List<FotoCategoria>();
    }

    private void Validar(Categoria categoria, int? excluirId)
    {
        var errores = _validador.Validar(categoria);
        if (!errores.ContainsKey(nameof(Categoria.Nombre)) && _repositorio.ExisteNombre(categoria.Nombre, excluirId))
            errores[nameof(Categoria.Nombre)] = "Ya existe una categoría con ese nombre (puede estar en la Papelera).";
        if (errores.Count > 0) throw new ValidacionException(errores);
    }

    private static void GuardarFotos(Categoria categoria)
    {
        try
        {
            foreach (var foto in categoria.Fotos)
                foto.RutaArchivo = AlmacenFotos.Guardar(foto.RutaArchivo);
        }
        catch (IOException ex)
        {
            Logger.Registrar("CategoriaServicio.GuardarFotos", ex);
            throw new NegocioException("No se pudieron guardar las fotos: " + ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            Logger.Registrar("CategoriaServicio.GuardarFotos", ex);
            throw new NegocioException("No hay permisos para guardar las fotos en la carpeta de la aplicación.");
        }
    }
}
