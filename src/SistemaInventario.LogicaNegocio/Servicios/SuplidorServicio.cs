using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;
using SistemaInventario.Entidades.Excepciones;
using SistemaInventario.LogicaNegocio.Interfaces;
using SistemaInventario.LogicaNegocio.Utilidades;
using SistemaInventario.LogicaNegocio.Validadores;

namespace SistemaInventario.LogicaNegocio.Servicios;

/// Reglas de negocio de suplidores.
public class SuplidorServicio : ISuplidorServicio
{
    private readonly ISuplidorRepositorio _repositorio;
    private readonly SuplidorValidador _validador = new();

    
    public SuplidorServicio(ISuplidorRepositorio repositorio)
    {
        _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
    }

    public IReadOnlyList<Suplidor> Listar(string? busqueda = null) =>
        Buscador.Filtrar(_repositorio.ObtenerActivos(), busqueda,
            s => s.NombreEmpresa, s => s.NombreContacto, s => s.Correo, s => s.Telefono)
            .OrderBy(s => s.NombreEmpresa).ToList();

    public IReadOnlyList<Suplidor> ListarEliminados(string? busqueda = null) =>
        Buscador.Filtrar(_repositorio.ObtenerEliminados(), busqueda,
            s => s.NombreEmpresa, s => s.NombreContacto, s => s.Correo, s => s.Telefono)
            .OrderBy(s => s.NombreEmpresa).ToList();

    public Suplidor? ObtenerPorId(int id) => _repositorio.ObtenerPorId(id);

    public int Crear(Suplidor suplidor)
    {
        Normalizar(suplidor);
        Validar(suplidor, null);
        return _repositorio.Insertar(suplidor);
    }

    public void Actualizar(Suplidor suplidor)
    {
        var existente = ObtenerExistente(suplidor.Id);
        if (!existente.Activo)
            throw new NegocioException("No se puede editar un suplidor eliminado. Restáurelo primero.");

        Normalizar(suplidor);
        Validar(suplidor, suplidor.Id);
        _repositorio.Actualizar(suplidor);
    }

    public void Eliminar(int id)
    {
        var suplidor = ObtenerExistente(id);
        if (!suplidor.Activo) return;

        var productos = _repositorio.ContarProductosActivos(id);
        if (productos > 0)
            throw new NegocioException(
                $"No se puede eliminar el suplidor '{suplidor.NombreEmpresa}': tiene {productos} producto(s) activo(s).");

        _repositorio.EliminarLogico(id);
    }

    public void Restaurar(int id)
    {
        var suplidor = ObtenerExistente(id);
        if (!suplidor.Activo) _repositorio.Restaurar(id);
    }

    private Suplidor ObtenerExistente(int id) =>
        _repositorio.ObtenerPorId(id) ?? throw new NegocioException("El suplidor no existe.");

    private static void Normalizar(Suplidor s)
    {
        s.NombreEmpresa = s.NombreEmpresa?.Trim() ?? string.Empty;
        s.NombreContacto = s.NombreContacto?.Trim() ?? string.Empty;
        s.Telefono = s.Telefono?.Trim() ?? string.Empty;
        s.Correo = s.Correo?.Trim() ?? string.Empty;
        s.SitioWeb = string.IsNullOrWhiteSpace(s.SitioWeb) ? null : s.SitioWeb.Trim();
    }

    private void Validar(Suplidor suplidor, int? excluirId)
    {
        var errores = _validador.Validar(suplidor);
        if (!errores.ContainsKey(nameof(Suplidor.NombreEmpresa)) &&
            _repositorio.ExisteNombreEmpresa(suplidor.NombreEmpresa, excluirId))
            errores[nameof(Suplidor.NombreEmpresa)] = "Ya existe un suplidor con ese nombre de empresa (puede estar en la Papelera).";
        if (errores.Count > 0) throw new ValidacionException(errores);
    }
}