using Microsoft.Data.SqlClient;
using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos.Repositorios;

/// Repositorio de suplidores.
public class RepositorioSuplidor : RepositorioBase<Suplidor>, ISuplidorRepositorio
{
    public RepositorioSuplidor(ConexionBD conexion) : base(conexion) { }

    protected override string Tabla => "Suplidores";

    protected override string SqlSeleccion =>
        "SELECT t.Id, t.NombreEmpresa, t.NombreContacto, t.Telefono, t.Correo, t.SitioWeb, " +
        "t.Activo, t.FechaCreacion, t.FechaModificacion FROM Suplidores t";

    protected override Suplidor Mapear(SqlDataReader r)
    {
        var iWeb = r.GetOrdinal("SitioWeb");
        var suplidor = new Suplidor
        {
            NombreEmpresa = Texto(r, "NombreEmpresa"),
            NombreContacto = Texto(r, "NombreContacto"),
            Telefono = Texto(r, "Telefono"),
            Correo = Texto(r, "Correo"),
            SitioWeb = r.IsDBNull(iWeb) ? null : r.GetString(iWeb)
        };
        LeerBase(r, suplidor);
        return suplidor;
    }

    protected override int InsertarEn(SqlConnection cn, SqlTransaction tx, Suplidor e)
    {
        using var cmd = new SqlCommand(
            "INSERT INTO Suplidores (NombreEmpresa, NombreContacto, Telefono, Correo, SitioWeb) " +
            "OUTPUT INSERTED.Id VALUES (@empresa, @contacto, @telefono, @correo, @web)", cn, tx);
        AgregarParametros(cmd, e);
        return (int)cmd.ExecuteScalar()!;
    }

    protected override void ActualizarEn(SqlConnection cn, SqlTransaction tx, Suplidor e)
    {
        using var cmd = new SqlCommand(
            "UPDATE Suplidores SET NombreEmpresa = @empresa, NombreContacto = @contacto, Telefono = @telefono, " +
            "Correo = @correo, SitioWeb = @web, FechaModificacion = SYSDATETIME() WHERE Id = @id", cn, tx);
        AgregarParametros(cmd, e);
        cmd.Parameters.Add(Param("@id", e.Id));
        ExigirFilaAfectada(cmd.ExecuteNonQuery());
    }

    private static void AgregarParametros(SqlCommand cmd, Suplidor e)
    {
        cmd.Parameters.Add(Param("@empresa", e.NombreEmpresa));
        cmd.Parameters.Add(Param("@contacto", e.NombreContacto));
        cmd.Parameters.Add(Param("@telefono", e.Telefono));
        cmd.Parameters.Add(Param("@correo", e.Correo));
        cmd.Parameters.Add(Param("@web", string.IsNullOrWhiteSpace(e.SitioWeb) ? null : e.SitioWeb));
    }

    /// <inheritdoc />
    public bool ExisteNombreEmpresa(string nombreEmpresa, int? excluirId = null) =>
        ContarEscalar(nameof(ExisteNombreEmpresa),
            "SELECT COUNT(1) FROM Suplidores WHERE NombreEmpresa = @nombre AND (@excluir IS NULL OR Id <> @excluir)",
            Param("@nombre", nombreEmpresa), ParamInt("@excluir", excluirId)) > 0;

    /// <inheritdoc />
    public int ContarProductosActivos(int suplidorId) =>
        ContarEscalar(nameof(ContarProductosActivos),
            "SELECT COUNT(1) FROM Productos WHERE SuplidorId = @id AND Activo = 1", Param("@id", suplidorId));
}
