namespace SistemaInventario.Entidades.Reportes;

/// <summary>Producto más caro de una categoría.</summary>
public record ProductoMasCaroDto(string Categoria, string Producto, decimal Precio);
