using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// <summary>Autenticación de usuarios.</summary>
public interface IUsuarioServicio
{
    /// <summary>Devuelve el usuario si las credenciales son correctas; si no, null.</summary>
    Usuario Autenticar(string nombreUsuario, string clave);
}
