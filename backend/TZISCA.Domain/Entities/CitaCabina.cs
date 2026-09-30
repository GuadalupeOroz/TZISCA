namespace TZISCA.Domain.Entities;

public class CitaCabina
{
    public long IdCita { get; set; }
    public int IdCabina { get; set; }
    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
    public bool Activo { get; set; } = true;

    public Cita Cita { get; set; } = null!;
    public Cabina Cabina { get; set; } = null!;
}
