namespace SistemaInventario.Entidades;

/// <summary>Empresa que suple los productos.</summary>
public class Suplidor : EntidadBase
{
    public string Empresa { get; set; }
    public string Contacto { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string SitioWeb { get; set; }
}
