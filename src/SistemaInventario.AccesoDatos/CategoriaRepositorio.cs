using Microsoft.Data.SqlClient;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos;

/// <summary>Repositorio de categorías (también guarda sus fotos).</summary>
public class CategoriaRepositorio : RepositorioBase<Categoria>
{
    protected override string Tabla => "Categorias";
    protected override string SqlInsertar => "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
    protected override string SqlActualizar => "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion, FechaModificacion = GETDATE() WHERE Id = @Id";

    protected override Categoria Mapear(SqlDataReader r)
    {
        var c = new Categoria { Nombre = (string)r["Nombre"], Descripcion = (string)r["Descripcion"] };
        LeerBase(r, c);
        return c;
    }

    protected override void AsignarParametros(SqlCommand cmd, Categoria c)
    {
        cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", c.Descripcion);
    }

    public override Categoria ObtenerPorId(int id)
    {
        var c = base.ObtenerPorId(id);
        if (c != null)
        {
            c.Fotos = Ejecutar(cn =>
            {
                using var cmd = new SqlCommand("SELECT Ruta FROM CategoriaFotos WHERE CategoriaId = @id ORDER BY Id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                using var r = cmd.ExecuteReader();
                var rutas = new List<string>();
                while (r.Read()) rutas.Add((string)r["Ruta"]);
                return rutas;
            }, "Fotos de categoría");
        }
        return c;
    }

    public override int Insertar(Categoria c)
    {
        base.Insertar(c);
        GuardarFotos(c);
        return c.Id;
    }

    public override void Actualizar(Categoria c)
    {
        base.Actualizar(c);
        GuardarFotos(c);
    }

    /// <summary>Reemplaza las fotos guardadas de la categoría por las actuales.</summary>
    private void GuardarFotos(Categoria c) => Ejecutar(cn =>
    {
        using (var borrar = new SqlCommand("DELETE FROM CategoriaFotos WHERE CategoriaId = @id", cn))
        {
            borrar.Parameters.AddWithValue("@id", c.Id);
            borrar.ExecuteNonQuery();
        }
        foreach (string ruta in c.Fotos)
        {
            using var ins = new SqlCommand("INSERT INTO CategoriaFotos (CategoriaId, Ruta) VALUES (@id, @ruta)", cn);
            ins.Parameters.AddWithValue("@id", c.Id);
            ins.Parameters.AddWithValue("@ruta", ruta);
            ins.ExecuteNonQuery();
        }
        return 0;
    }, "Guardar fotos");
}
