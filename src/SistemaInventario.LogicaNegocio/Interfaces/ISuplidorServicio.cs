using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// <summary>Operaciones de negocio para suplidores.</summary>
public interface ISuplidorServicio
{
    List<Suplidor> Listar(string texto);
    Suplidor ObtenerPorId(int id);
    Dictionary<string, string> Guardar(Suplidor suplidor);
    string Eliminar(int id);
}
