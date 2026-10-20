using SistemaInventario.AccesoDatos;
using SistemaInventario.Entidades;
using SistemaInventario.LogicaNegocio.Interfaces;

namespace SistemaInventario.LogicaNegocio;

///Login de usuarios comparando el hash SHA-256 de la clave
public class UsuarioServicio : IUsuarioServicio
{
    private readonly IRepositorio<Usuario> _repo;

    public UsuarioServicio(IRepositorio<Usuario> repo) => _repo = repo;

 
    public Usuario Autenticar(string nombreUsuario, string clave)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrEmpty(clave)) return null;
        string hash = Utilidades.Hash(clave);
        return _repo.ObtenerActivos().FirstOrDefault(u =>
            u.NombreUsuario.Equals(nombreUsuario.Trim(), StringComparison.OrdinalIgnoreCase) &&
            u.ClaveHash.Equals(hash, StringComparison.OrdinalIgnoreCase));
    }
}
