using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class Transaccion
{
    public long IdTransaccion { get; set; }
    public long IdPago { get; set; }
    public string? ReferenciaExterna { get; set; }
    public string ProveedorPago { get; set; } = "STRIPE";
    public TipoTransaccion Tipo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? CodigoRespuesta { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public Pago Pago { get; set; } = null!;
}
