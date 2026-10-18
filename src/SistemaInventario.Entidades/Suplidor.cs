namespace SistemaInventario.Entidades;

public class Suplidor : EntidadBase
{
    public string NombreEmpresa { get; set; } = string.Empty;
    public string NombreContacto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? SitioWeb { get; set; }

    public override string ToString() => NombreEmpresa;
}
