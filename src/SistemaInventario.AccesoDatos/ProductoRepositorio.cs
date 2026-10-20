using Microsoft.Data.SqlClient;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos;

/// <summary>Repositorio de productos.</summary>
public class ProductoRepositorio : RepositorioBase<Producto>
{
    protected override string Tabla => "Productos";
    protected override string SqlInsertar =>
        "INSERT INTO Productos (Nombre, Precio, CategoriaId, SuplidorId) VALUES (@Nombre, @Precio, @CategoriaId, @SuplidorId)";
    protected override string SqlActualizar =>
        "UPDATE Productos SET Nombre = @Nombre, Precio = @Precio, CategoriaId = @CategoriaId, SuplidorId = @SuplidorId, FechaModificacion = GETDATE() WHERE Id = @Id";

    protected override Producto Mapear(SqlDataReader r)
    {
        var p = new Producto
        {
            Nombre = (string)r["Nombre"],
            Precio = (decimal)r["Precio"],
            CategoriaId = (int)r["CategoriaId"],
            SuplidorId = (int)r["SuplidorId"]
        };
        LeerBase(r, p);
        return p;
    }

    protected override void AsignarParametros(SqlCommand cmd, Producto p)
    {
        cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@Precio", p.Precio);
        cmd.Parameters.AddWithValue("@CategoriaId", p.CategoriaId);
        cmd.Parameters.AddWithValue("@SuplidorId", p.SuplidorId);
    }
}
