using SistemaInventario.AccesoDatos;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio;

/// <summary>Reglas de negocio de suplidores.</summary>
public class SuplidorServicio : ISuplidorServicio
{
    private readonly IRepositorio<Suplidor> _repo;
    private readonly IRepositorio<Producto> _repoProductos;

    public SuplidorServicio(IRepositorio<Suplidor> repo, IRepositorio<Producto> repoProductos)
    {
        _repo = repo;
        _repoProductos = repoProductos;
    }

    /// <inheritdoc/>
    public List<Suplidor> Listar(string texto) =>
        Utilidades.Buscar(_repo.ObtenerActivos(), texto, s => s.Empresa, s => s.Contacto, s => s.Correo)
                  .OrderBy(s => s.Empresa).ToList();

    /// <inheritdoc/>
    public Suplidor ObtenerPorId(int id) => _repo.ObtenerPorId(id);

    /// <inheritdoc/>
    public Dictionary<string, string> Guardar(Suplidor s)
    {
        var errores = new Dictionary<string, string>();
        string empresa = (s.Empresa ?? "").Trim();

        if (!Validador.LongitudValida(empresa, 2, 100))
            errores["Empresa"] = "El nombre de la empresa es obligatorio (máximo 100 caracteres).";
        else if (_repo.ObtenerActivos().Concat(_repo.ObtenerEliminados())
                      .Any(x => x.Id != s.Id && x.Empresa.Equals(empresa, StringComparison.OrdinalIgnoreCase)))
            errores["Empresa"] = "Ya existe un suplidor con ese nombre (revise también la papelera).";

        if (!Validador.LongitudValida(s.Contacto, 2, 100))
            errores["Contacto"] = "El nombre del contacto es obligatorio (máximo 100 caracteres).";

        if (!Validador.EsTelefono(s.Telefono))
            errores["Telefono"] = "Formato inválido. Use 809-XXX-XXXX, 829-XXX-XXXX o 849-XXX-XXXX.";

        if (!Validador.EsCorreo(s.Correo))
            errores["Correo"] = "Ingrese un correo electrónico válido.";

        if (!string.IsNullOrWhiteSpace(s.SitioWeb) && !Validador.EsUrl(s.SitioWeb.Trim()))
            errores["SitioWeb"] = "Ingrese una URL válida (ej: https://www.empresa.com).";

        if (errores.Count > 0) return errores;

        s.Empresa = empresa;
        s.Contacto = s.Contacto.Trim();
        s.Telefono = s.Telefono.Trim();
        s.Correo = s.Correo.Trim();
        s.SitioWeb = string.IsNullOrWhiteSpace(s.SitioWeb) ? null : s.SitioWeb.Trim();

        if (s.Id == 0) _repo.Insertar(s); else _repo.Actualizar(s);
        return errores;
    }

    /// <inheritdoc/>
    public string Eliminar(int id)
    {
        if (_repoProductos.ObtenerActivos().Any(p => p.SuplidorId == id))
            return "No se puede eliminar: el suplidor tiene productos activos asociados.";
        _repo.Eliminar(id);
        return null;
    }
}
