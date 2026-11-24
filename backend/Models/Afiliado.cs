namespace ConsultasPlan.Models;

public class Afiliado
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Plan { get; set; } = "Plan Integral";
    public int ConsultasUsadas { get; set; }
    public bool Bloqueado { get; set; }
    public DateTime? FechaBloqueo { get; set; }
    public DateTime createdAt { get; set; }
    public bool isActive { get; set; }
}
