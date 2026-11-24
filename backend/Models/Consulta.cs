namespace ConsultasPlan.Models;

public class Consulta
{
    public int Id { get; set; }
    public int? AfiliadoId { get; set; }
    public Afiliado? Afiliado { get; set; }
    public int ProfesionalId { get; set; }
    public Profesional? Profesional { get; set; }
    public DateTime FechaHora { get; set; }
    public EstadoConsulta Estado { get; set; }
    public ModalidadConsulta Modalidad { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
