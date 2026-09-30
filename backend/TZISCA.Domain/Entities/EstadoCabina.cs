using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class EstadoCabina
{
    public long IdEstadoCabina { get; set; }
    public int IdCabina { get; set; }
    public EstadoCabinaOperativo? EstadoAnterior { get; set; }
    public EstadoCabinaOperativo EstadoNuevo { get; set; }
    public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
    public int? IdUsuario { get; set; }
    public string? Motivo { get; set; }

    public Cabina Cabina { get; set; } = null!;
    public Usuario? Usuario { get; set; }
}
