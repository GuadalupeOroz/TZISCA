using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class Devolucion
{
    public long IdDevolucion { get; set; }
    public long IdPago { get; set; }
    public long IdCancelacion { get; set; }
    public TipoDevolucion Tipo { get; set; }
    public decimal Porcentaje { get; set; }
    public decimal Monto { get; set; }
    public EstadoDevolucion Estado { get; set; } = EstadoDevolucion.Pendiente;
    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
    public DateTime? FechaProcesamiento { get; set; }
    public int? IdUsuarioResponsable { get; set; }

    public Pago Pago { get; set; } = null!;
    public Cancelacion Cancelacion { get; set; } = null!;
    public Usuario? UsuarioResponsable { get; set; }
}
