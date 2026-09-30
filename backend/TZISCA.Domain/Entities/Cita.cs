using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class Cita
{
    public long IdCita { get; set; }
    public int IdCliente { get; set; }
    public int IdTratamiento { get; set; }
    public int? IdProveedor { get; set; }
    public long? IdCarrito { get; set; }
    public int NumeroPersonas { get; set; } = 1;
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;
    public decimal PrecioUnitario { get; set; }
    public decimal Importe { get; set; }
    public DateTime? FechaExpiracionBloqueo { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaConfirmacion { get; set; }
    public string? Observaciones { get; set; }

    public Cliente Cliente { get; set; } = null!;
    public Tratamiento Tratamiento { get; set; } = null!;
    public Proveedor? Proveedor { get; set; }
    public Carrito? Carrito { get; set; }
    public ICollection<CitaCabina> CitasCabina { get; set; } = new List<CitaCabina>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<Cancelacion> Cancelaciones { get; set; } = new List<Cancelacion>();
}
