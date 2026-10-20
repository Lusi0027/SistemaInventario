namespace SistemaInventario.Entidades;

/// <summary>Usuario que puede iniciar sesión en el sistema.</summary>
public class Usuario : EntidadBase
{
    public string NombreUsuario { get; set; }
    public string ClaveHash { get; set; }
    public string NombreCompleto { get; set; }
}
