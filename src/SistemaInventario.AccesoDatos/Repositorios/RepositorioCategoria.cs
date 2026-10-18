using Microsoft.Data.SqlClient;
using SistemaInventario.AccesoDatos.Interfaces;
using SistemaInventario.Entidades;

namespace SistemaInventario.AccesoDatos.Repositorios;

/// Repositorio de categorías.
public class RepositorioCategoria : RepositorioBase<Categoria>, ICategoriaRepositorio
{
    public RepositorioCategoria(ConexionBD conexion) : base(conexion) { }

    protected override string Tabla => "Categorias";

    protected override string SqlSeleccion =>
        "SELECT t.Id, t.Nombre, t.Descripcion, t.Activo, t.FechaCreacion, t.FechaModificacion FROM Categorias t";

    protected override Categoria Mapear(SqlDataReader r)
    {
        var categoria = new Categoria
        {
            Nombre = Texto(r, "Nombre"),
            Descripcion = Texto(r, "Descripcion")
        };
        LeerBase(r, categoria);
        return categoria;
    }

    protected override int InsertarEn(SqlConnection cn, SqlTransaction tx, Categoria e)
    {
        using var cmd = new SqlCommand(
            "INSERT INTO Categorias (Nombre, Descripcion) OUTPUT INSERTED.Id VALUES (@nombre, @descripcion)", cn, tx);
        cmd.Parameters.Add(Param("@nombre", e.Nombre));
        cmd.Parameters.Add(Param("@descripcion", e.Descripcion));
        var id = (int)cmd.ExecuteScalar()!;
        GuardarFotos(cn, tx, id, e.Fotos);
        return id;
    }

    protected override void ActualizarEn(SqlConnection cn, SqlTransaction tx, Categoria e)
    {
        using (var cmd = new SqlCommand(
            "UPDATE Categorias SET Nombre = @nombre, Descripcion = @descripcion, " +
            "FechaModificacion = SYSDATETIME() WHERE Id = @id", cn, tx))
        {
            cmd.Parameters.Add(Param("@nombre", e.Nombre));
            cmd.Parameters.Add(Param("@descripcion", e.Descripcion));
            cmd.Parameters.Add(Param("@id", e.Id));
            ExigirFilaAfectada(cmd.ExecuteNonQuery());
        }

        using (var borrar = new SqlCommand("DELETE FROM FotosCategoria WHERE CategoriaId = @id", cn, tx))
        {
            borrar.Parameters.Add(Param("@id", e.Id));
            borrar.ExecuteNonQuery();
        }
        GuardarFotos(cn, tx, e.Id, e.Fotos);
    }

    private static void GuardarFotos(SqlConnection cn, SqlTransaction tx, int categoriaId,
        IEnumerable<FotoCategoria> fotos)
    {
        foreach (var foto in fotos)
        {
            using var cmd = new SqlCommand(
                "INSERT INTO FotosCategoria (CategoriaId, RutaArchivo) VALUES (@categoriaId, @ruta)", cn, tx);
            cmd.Parameters.Add(Param("@categoriaId", categoriaId));
            cmd.Parameters.Add(Param("@ruta", foto.RutaArchivo));
            cmd.ExecuteNonQuery();
        }
    }

    protected override void CargarRelaciones(SqlConnection cn, List<Categoria> categorias)
    {
        if (categorias.Count == 0) return;

        var porId = categorias.ToDictionary(c => c.Id);
        var ids = string.Join(",", porId.Keys); // enteros: sin riesgo de inyección
        using var cmd = new SqlCommand(
            $"SELECT Id, CategoriaId, RutaArchivo FROM FotosCategoria WHERE CategoriaId IN ({ids}) ORDER BY Id", cn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            if (porId.TryGetValue(r.GetInt32(1), out var categoria))
            {
                categoria.Fotos.Add(new FotoCategoria
                {
                    Id = r.GetInt32(0),
                    CategoriaId = r.GetInt32(1),
                    RutaArchivo = r.GetString(2)
                });
            }
        }
    }


    public bool ExisteNombre(string nombre, int? excluirId = null) =>
        ContarEscalar(nameof(ExisteNombre),
            "SELECT COUNT(1) FROM Categorias WHERE Nombre = @nombre AND (@excluir IS NULL OR Id <> @excluir)",
            Param("@nombre", nombre), ParamInt("@excluir", excluirId)) > 0;

 
    public int ContarProductosActivos(int categoriaId) =>
        ContarEscalar(nameof(ContarProductosActivos),
            "SELECT COUNT(1) FROM Productos WHERE CategoriaId = @id AND Activo = 1", Param("@id", categoriaId));
}
