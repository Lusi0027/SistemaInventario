using Microsoft.Data.SqlClient;
using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos.Repositorios;

///Repositorio de productos (trae nombres de categoría y suplidor con JOIN).
public class RepositorioProducto : RepositorioBase<Producto>, IProductoRepositorio
{
    public RepositorioProducto(ConexionBD conexion) : base(conexion) { }

    protected override string Tabla => "Productos";

    protected override string SqlSeleccion =>
        "SELECT t.Id, t.Nombre, t.PrecioUnitario, t.CategoriaId, t.SuplidorId, t.Activo, " +
        "t.FechaCreacion, t.FechaModificacion, c.Nombre AS CategoriaNombre, s.NombreEmpresa AS SuplidorNombre " +
        "FROM Productos t " +
        "INNER JOIN Categorias c ON c.Id = t.CategoriaId " +
        "INNER JOIN Suplidores s ON s.Id = t.SuplidorId";

    protected override Producto Mapear(SqlDataReader r)
    {
        var producto = new Producto
        {
            Nombre = Texto(r, "Nombre"),
            PrecioUnitario = r.GetDecimal(r.GetOrdinal("PrecioUnitario")),
            CategoriaId = r.GetInt32(r.GetOrdinal("CategoriaId")),
            SuplidorId = r.GetInt32(r.GetOrdinal("SuplidorId")),
            CategoriaNombre = Texto(r, "CategoriaNombre"),
            SuplidorNombre = Texto(r, "SuplidorNombre")
        };
        LeerBase(r, producto);
        return producto;
    }

    protected override int InsertarEn(SqlConnection cn, SqlTransaction tx, Producto e)
    {
        using var cmd = new SqlCommand(
            "INSERT INTO Productos (Nombre, PrecioUnitario, CategoriaId, SuplidorId) " +
            "OUTPUT INSERTED.Id VALUES (@nombre, @precio, @categoria, @suplidor)", cn, tx);
        AgregarParametros(cmd, e);
        return (int)cmd.ExecuteScalar()!;
    }

    protected override void ActualizarEn(SqlConnection cn, SqlTransaction tx, Producto e)
    {
        using var cmd = new SqlCommand(
            "UPDATE Productos SET Nombre = @nombre, PrecioUnitario = @precio, CategoriaId = @categoria, " +
            "SuplidorId = @suplidor, FechaModificacion = SYSDATETIME() WHERE Id = @id", cn, tx);
        AgregarParametros(cmd, e);
        cmd.Parameters.Add(Param("@id", e.Id));
        ExigirFilaAfectada(cmd.ExecuteNonQuery());
    }

    private static void AgregarParametros(SqlCommand cmd, Producto e)
    {
        cmd.Parameters.Add(Param("@nombre", e.Nombre));
        cmd.Parameters.Add(ParamMoneda("@precio", e.PrecioUnitario));
        cmd.Parameters.Add(Param("@categoria", e.CategoriaId));
        cmd.Parameters.Add(Param("@suplidor", e.SuplidorId));
    }
}
