using System.Data;
using Microsoft.Data.SqlClient;
using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;
using SistemaInventario.Entidades.Excepciones;

namespace SistemaInventario.AccesoDatos.Repositorios;

public abstract class RepositorioBase<T> : IRepositorio<T> where T : EntidadBase
{
    protected readonly ConexionBD Conexion;

    protected RepositorioBase(ConexionBD conexion)
    {
        Conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
    }

    protected abstract string Tabla { get; }

    protected abstract string SqlSeleccion { get; }

    protected abstract T Mapear(SqlDataReader lector);

    protected abstract int InsertarEn(SqlConnection cn, SqlTransaction tx, T entidad);

    protected abstract void ActualizarEn(SqlConnection cn, SqlTransaction tx, T entidad);

    protected virtual void CargarRelaciones(SqlConnection cn, List<T> items) { }

    public IEnumerable<T> ObtenerActivos() =>
        Consultar(nameof(ObtenerActivos), SqlSeleccion + " WHERE t.Activo = 1 ORDER BY t.Id");

    public IEnumerable<T> ObtenerEliminados() =>
        Consultar(nameof(ObtenerEliminados), SqlSeleccion + " WHERE t.Activo = 0 ORDER BY t.Id");

    public T? ObtenerPorId(int id) =>
        Consultar(nameof(ObtenerPorId), SqlSeleccion + " WHERE t.Id = @id", Param("@id", id)).FirstOrDefault();

    public int Insertar(T entidad) =>
        EjecutarTransaccion(nameof(Insertar), (cn, tx) => InsertarEn(cn, tx, entidad));

    public void Actualizar(T entidad) =>
        EjecutarTransaccion(nameof(Actualizar), (cn, tx) =>
        {
            ActualizarEn(cn, tx, entidad);
            return 0;
        });

    public void EliminarLogico(int id) => CambiarEstado(nameof(EliminarLogico), id, false);

    public void Restaurar(int id) => CambiarEstado(nameof(Restaurar), id, true);

    private void CambiarEstado(string operacion, int id, bool activo)
    {
        Ejecutar(operacion, cn =>
        {
            using var cmd = new SqlCommand(
                $"UPDATE {Tabla} SET Activo = @activo, FechaModificacion = SYSDATETIME() WHERE Id = @id", cn);
            cmd.Parameters.Add(Param("@activo", activo));
            cmd.Parameters.Add(Param("@id", id));
            if (cmd.ExecuteNonQuery() == 0)
                throw new AccesoDatosException("El registro indicado no existe.");
            return 0;
        });
    }

    protected List<T> Consultar(string operacion, string sql, params SqlParameter[] parametros) =>
        Ejecutar(operacion, cn =>
        {
            var lista = new List<T>();
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parametros);
                using var lector = cmd.ExecuteReader();
                while (lector.Read()) lista.Add(Mapear(lector));
            }
            CargarRelaciones(cn, lista);
            return lista;
        });

    protected int ContarEscalar(string operacion, string sql, params SqlParameter[] parametros) =>
        Ejecutar(operacion, cn =>
        {
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddRange(parametros);
            return Convert.ToInt32(cmd.ExecuteScalar());
        });

    protected TResultado Ejecutar<TResultado>(string operacion, Func<SqlConnection, TResultado> accion)
    {
        try
        {
            using var cn = Conexion.CrearConexion();
            cn.Open();
            return accion(cn);
        }
        catch (SqlException ex)
        {
            throw ConvertirExcepcion(operacion, ex);
        }
    }

    protected TResultado EjecutarTransaccion<TResultado>(string operacion,
        Func<SqlConnection, SqlTransaction, TResultado> accion) =>
        Ejecutar(operacion, cn =>
        {
            using var tx = cn.BeginTransaction();
            try
            {
                var resultado = accion(cn, tx);
                tx.Commit();
                return resultado;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        });

    private AccesoDatosException ConvertirExcepcion(string operacion, SqlException ex)
    {
        Logger.Registrar($"{GetType().Name}.{operacion}", ex);
        var mensaje = ex.Number switch
        {
            2627 or 2601 => "Ya existe un registro con el mismo valor único (puede estar en la Papelera).",
            547 => "La operación viola una relación entre registros.",
            -1 or 2 or 53 or 4060 => "No se pudo conectar con la base de datos. Verifique la cadena de conexión.",
            _ => "Ocurrió un error al acceder a la base de datos."
        };
        return new AccesoDatosException(mensaje, ex);
    }

    protected static SqlParameter Param(string nombre, object? valor) => new(nombre, valor ?? DBNull.Value);

    protected static SqlParameter ParamInt(string nombre, int? valor) =>
        new(nombre, SqlDbType.Int) { Value = (object?)valor ?? DBNull.Value };

    protected static SqlParameter ParamMoneda(string nombre, decimal valor) =>
        new(nombre, SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = valor };

    protected static void LeerBase(SqlDataReader r, EntidadBase entidad)
    {
        entidad.Id = r.GetInt32(r.GetOrdinal("Id"));
        entidad.Activo = r.GetBoolean(r.GetOrdinal("Activo"));
        entidad.FechaCreacion = r.GetDateTime(r.GetOrdinal("FechaCreacion"));
        var iMod = r.GetOrdinal("FechaModificacion");
        entidad.FechaModificacion = r.IsDBNull(iMod) ? null : r.GetDateTime(iMod);
    }

    protected static string Texto(SqlDataReader r, string columna) => r.GetString(r.GetOrdinal(columna));

    protected static void ExigirFilaAfectada(int filas) =>
        _ = filas > 0 ? 0 : throw new AccesoDatosException("El registro indicado no existe.");
}