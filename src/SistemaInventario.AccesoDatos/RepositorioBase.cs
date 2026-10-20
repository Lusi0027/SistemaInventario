using Microsoft.Data.SqlClient;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos;

/// Implementación genérica de IRepositorio. Cada repositorio concreto solo define


public abstract class RepositorioBase<T> : IRepositorio<T> where T : EntidadBase
{
    protected abstract string Tabla { get; }
    protected abstract string SqlInsertar { get; }
    protected abstract string SqlActualizar { get; }
    protected abstract T Mapear(SqlDataReader r);
    protected abstract void AsignarParametros(SqlCommand cmd, T entidad);

    /// Lee los campos comunes (Id, Activo, fechas) de una fila
    protected static void LeerBase(SqlDataReader r, EntidadBase e)
    {
        e.Id = (int)r["Id"];
        e.Activo = (bool)r["Activo"];
        e.FechaCreacion = (DateTime)r["FechaCreacion"];
        e.FechaModificacion = (DateTime)r["FechaModificacion"];
    }

    /// Método genérico: abre la conexión, ejecuta la acción y maneja SqlException
    protected TResultado Ejecutar<TResultado>(Func<SqlConnection, TResultado> accion, string contexto)
    {
        try
        {
            using var cn = ConexionBD.Instancia.CrearConexion();
            cn.Open();
            return accion(cn);
        }
        catch (SqlException ex)
        {
            Logger.Registrar(ex, contexto);
            throw new Exception("Error en la base de datos: " + ex.Message, ex);
        }
    }

    public List<T> ObtenerActivos() => Listar(true);

    public List<T> ObtenerEliminados() => Listar(false);

    private List<T> Listar(bool activo) => Ejecutar(cn =>
    {
        using var cmd = new SqlCommand($"SELECT * FROM {Tabla} WHERE Activo = @a ORDER BY Id", cn);
        cmd.Parameters.AddWithValue("@a", activo);
        using var r = cmd.ExecuteReader();
        var lista = new List<T>();
        while (r.Read()) lista.Add(Mapear(r));
        return lista;
    }, "Listar " + Tabla);

    public virtual T ObtenerPorId(int id) => Ejecutar(cn =>
    {
        using var cmd = new SqlCommand($"SELECT * FROM {Tabla} WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);
        using var r = cmd.ExecuteReader();
        return r.Read() ? Mapear(r) : null;
    }, "ObtenerPorId " + Tabla);

    public virtual int Insertar(T entidad) => Ejecutar(cn =>
    {
        using var cmd = new SqlCommand(SqlInsertar + "; SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);
        AsignarParametros(cmd, entidad);
        entidad.Id = (int)cmd.ExecuteScalar();
        return entidad.Id;
    }, "Insertar " + Tabla);

    public virtual void Actualizar(T entidad) => Ejecutar(cn =>
    {
        using var cmd = new SqlCommand(SqlActualizar, cn);
        AsignarParametros(cmd, entidad);
        cmd.Parameters.AddWithValue("@Id", entidad.Id);
        return cmd.ExecuteNonQuery();
    }, "Actualizar " + Tabla);

    public void Eliminar(int id) => CambiarEstado(id, false);

    public void Restaurar(int id) => CambiarEstado(id, true);

    private void CambiarEstado(int id, bool activo) => Ejecutar(cn =>
    {
        using var cmd = new SqlCommand($"UPDATE {Tabla} SET Activo = @a, FechaModificacion = GETDATE() WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@a", activo);
        cmd.Parameters.AddWithValue("@id", id);
        return cmd.ExecuteNonQuery();
    }, "CambiarEstado " + Tabla);
}
