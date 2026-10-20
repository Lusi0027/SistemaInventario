using Microsoft.Data.SqlClient;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos;

/// <summary>Repositorio de usuarios (para el login).</summary>
public class UsuarioRepositorio : RepositorioBase<Usuario>
{
    protected override string Tabla => "Usuarios";
    protected override string SqlInsertar =>
        "INSERT INTO Usuarios (NombreUsuario, ClaveHash, NombreCompleto) VALUES (@NombreUsuario, @ClaveHash, @NombreCompleto)";
    protected override string SqlActualizar =>
        "UPDATE Usuarios SET NombreUsuario = @NombreUsuario, ClaveHash = @ClaveHash, NombreCompleto = @NombreCompleto, FechaModificacion = GETDATE() WHERE Id = @Id";

    protected override Usuario Mapear(SqlDataReader r)
    {
        var u = new Usuario
        {
            NombreUsuario = (string)r["NombreUsuario"],
            ClaveHash = (string)r["ClaveHash"],
            NombreCompleto = (string)r["NombreCompleto"]
        };
        LeerBase(r, u);
        return u;
    }

    protected override void AsignarParametros(SqlCommand cmd, Usuario u)
    {
        cmd.Parameters.AddWithValue("@NombreUsuario", u.NombreUsuario);
        cmd.Parameters.AddWithValue("@ClaveHash", u.ClaveHash);
        cmd.Parameters.AddWithValue("@NombreCompleto", u.NombreCompleto);
    }
}
