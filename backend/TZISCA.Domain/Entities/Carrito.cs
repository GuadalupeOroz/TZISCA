using TZISCA.Domain.Enums;

namespace TZISCA.Domain.Entities;

public class Carrito
{
    public long IdCarrito { get; set; }
    public int IdCliente { get; set; }
    public EstadoCarrito Estado { get; set; } = EstadoCarrito.Activo;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    public Cliente Cliente { get; set; } = null!;
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
