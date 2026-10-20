namespace SistemaInventario.Entidades;

/// Usuario que puede iniciar sesión en el sistema
public class Usuario : EntidadBase
{
    public string NombreUsuario { get; set; }
    public string ClaveHash { get; set; }
    public string NombreCompleto { get; set; }
}
