namespace TZISCA.Domain.Entities;

public class PreferenciaCliente
{
    public int IdPreferencia { get; set; }
    public int IdCliente { get; set; }
    public string? TipoExperiencia { get; set; }
    public string? Caracteristicas { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    public Cliente Cliente { get; set; } = null!;
}
