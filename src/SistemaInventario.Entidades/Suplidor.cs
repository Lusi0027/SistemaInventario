namespace SistemaInventario.Entidades;

/// Empresa que suple los productos
public class Suplidor : EntidadBase
{
    public string Empresa { get; set; }
    public string Contacto { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string SitioWeb { get; set; }
}
