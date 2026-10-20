using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// Operaciones de negocio para suplidores
public interface ISuplidorServicio
{
    List<Suplidor> Listar(string texto);
    Suplidor ObtenerPorId(int id);
    Dictionary<string, string> Guardar(Suplidor suplidor);
    string Eliminar(int id);
}
