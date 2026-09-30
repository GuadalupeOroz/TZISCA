using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class Pago
{
    public long IdPago { get; set; }
    public long IdCita { get; set; }
    public decimal Monto { get; set; }
    public string Moneda { get; set; } = "MXN";
    public string MetodoPago { get; set; } = string.Empty;
    public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;
    public string? Referencia { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaPago { get; set; }

    public Cita Cita { get; set; } = null!;
    public ICollection<Devolucion> Devoluciones { get; set; } = new List<Devolucion>();
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}
