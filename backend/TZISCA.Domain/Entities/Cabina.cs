using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class Cabina
{
    public int IdCabina { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CapacidadMaxima { get; set; }
    public string? Caracteristicas { get; set; }
    public string? Beneficios { get; set; }
    public int Prioridad { get; set; }
    public bool Activo { get; set; } = true;
    public EstadoCabinaOperativo Estado { get; set; } = EstadoCabinaOperativo.Disponible;

    public ICollection<CitaCabina> CitasCabina { get; set; } = new List<CitaCabina>();
    public ICollection<EstadoCabina> EstadosCabina { get; set; } = new List<EstadoCabina>();
}
