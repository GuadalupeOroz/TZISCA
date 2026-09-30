using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class DisponibilidadProveedor
{
    public long IdDisponibilidad { get; set; }
    public int IdProveedor { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public TipoDisponibilidadProveedor Tipo { get; set; }
    public string? Motivo { get; set; }
    public bool Activo { get; set; } = true;

    public Proveedor Proveedor { get; set; } = null!;
}
