namespace ConsultasPlan.Models;

public class Profesional
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public int SedeId { get; set; }
    public Sede? Sede { get; set; }
}
