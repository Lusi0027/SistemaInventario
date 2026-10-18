using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Validadores;

/// Reglas de validación de Categoria.
public class CategoriaValidador : Validador<Categoria>
{
    /// Máximo de fotos por categoría.
    public const int MaximoFotos = 5;

    protected override void Reglas(Categoria c, IDictionary<string, string> errores)
    {
        TextoObligatorio(errores, nameof(Categoria.Nombre), "El nombre", c.Nombre, 3, 100);

        var descripcion = c.Descripcion?.Trim() ?? string.Empty;
        if (descripcion.Length == 0)
            errores[nameof(Categoria.Descripcion)] = "La descripción es obligatoria.";
        else if (descripcion.Length > 500)
            errores[nameof(Categoria.Descripcion)] = "La descripción no puede superar 500 caracteres.";

        if (c.Fotos.Count > MaximoFotos)
        {
            errores[nameof(Categoria.Fotos)] = $"Una categoría admite máximo {MaximoFotos} fotos.";
            return;
        }

        var problemas = c.Fotos.Select(f => FotoValidador.Validar(f.RutaArchivo)).Where(m => m != null).ToList();
        if (problemas.Count > 0)
            errores[nameof(Categoria.Fotos)] = string.Join(" ", problemas);
    }
}