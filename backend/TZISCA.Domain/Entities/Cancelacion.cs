namespace TZISCA.Domain.Entities;

public class Cancelacion
{
    public long IdCancelacion { get; set; }
    public long IdCita { get; set; }
    public int IdUsuario { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public bool AtribuibleSpa { get; set; }
    public DateTime FechaCancelacion { get; set; } = DateTime.UtcNow;
    public string? Observaciones { get; set; }

    public Cita Cita { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
    public ICollection<Devolucion> Devoluciones { get; set; } = new List<Devolucion>();
}
