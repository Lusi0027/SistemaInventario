using SistemaInventario.Entidades;

namespace SistemaInventario.LogicaNegocio.Validadores;

/// Reglas de validación de Producto (las de existencia/estado van en el servicio).
public class ProductoValidador : Validador<Producto>
{
    protected override void Reglas(Producto p, IDictionary<string, string> errores)
    {
        TextoObligatorio(errores, nameof(Producto.Nombre), "El nombre del producto", p.Nombre, 1, 150);

        if (p.PrecioUnitario <= 0)
            errores[nameof(Producto.PrecioUnitario)] = "El precio debe ser mayor a 0.";
        else if (decimal.Round(p.PrecioUnitario, 2) != p.PrecioUnitario)
            errores[nameof(Producto.PrecioUnitario)] = "El precio admite máximo 2 decimales.";
        else if (p.PrecioUnitario > 9_999_999_999_999.99m)
            errores[nameof(Producto.PrecioUnitario)] = "El precio es demasiado grande.";

        if (p.CategoriaId <= 0)
            errores[nameof(Producto.CategoriaId)] = "Debe seleccionar una categoría.";
        if (p.SuplidorId <= 0)
            errores[nameof(Producto.SuplidorId)] = "Debe seleccionar un suplidor.";
    }
}
