using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Interfaces;

/// <summary>Papelera: registros eliminados lógicamente.</summary>
public interface IPapeleraServicio
{
    List<ItemPapelera> Listar(string texto);
    /// <summary>Restaura un registro. Devuelve un mensaje de error o null si se restauró.</summary>
    string Restaurar(ItemPapelera item);
}
