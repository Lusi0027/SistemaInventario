using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// Papelera: registros eliminados lógicamente
public interface IPapeleraServicio
{
    List<ItemPapelera> Listar(string texto);
    /// Restaura un registro. Devuelve un mensaje de error o null si se restauró
    string Restaurar(ItemPapelera item);
}
