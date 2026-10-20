using Microsoft.Data.SqlClient;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos;

/// <summary>Repositorio de suplidores.</summary>
public class SuplidorRepositorio : RepositorioBase<Suplidor>
{
    protected override string Tabla => "Suplidores";
    protected override string SqlInsertar =>
        "INSERT INTO Suplidores (Empresa, Contacto, Telefono, Correo, SitioWeb) VALUES (@Empresa, @Contacto, @Telefono, @Correo, @SitioWeb)";
    protected override string SqlActualizar =>
        "UPDATE Suplidores SET Empresa = @Empresa, Contacto = @Contacto, Telefono = @Telefono, Correo = @Correo, SitioWeb = @SitioWeb, FechaModificacion = GETDATE() WHERE Id = @Id";

    protected override Suplidor Mapear(SqlDataReader r)
    {
        var s = new Suplidor
        {
            Empresa = (string)r["Empresa"],
            Contacto = (string)r["Contacto"],
            Telefono = (string)r["Telefono"],
            Correo = (string)r["Correo"],
            SitioWeb = r["SitioWeb"] as string
        };
        LeerBase(r, s);
        return s;
    }

    protected override void AsignarParametros(SqlCommand cmd, Suplidor s)
    {
        cmd.Parameters.AddWithValue("@Empresa", s.Empresa);
        cmd.Parameters.AddWithValue("@Contacto", s.Contacto);
        cmd.Parameters.AddWithValue("@Telefono", s.Telefono);
        cmd.Parameters.AddWithValue("@Correo", s.Correo);
        cmd.Parameters.AddWithValue("@SitioWeb", string.IsNullOrWhiteSpace(s.SitioWeb) ? DBNull.Value : s.SitioWeb);
    }
}
