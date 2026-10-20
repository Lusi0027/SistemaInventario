using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// Autenticación de usuarios
public interface IUsuarioServicio
{
    /// Devuelve el usuario si las credenciales son correctas; si no, null
    Usuario Autenticar(string nombreUsuario, string clave);
}
