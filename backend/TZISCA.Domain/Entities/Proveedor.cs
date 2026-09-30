namespace TZISCA.Domain.Entities;

public class Proveedor
{
    public int IdProveedor { get; set; }
    public int IdUsuario { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaAlta { get; set; } = DateTime.UtcNow;

    public Usuario Usuario { get; set; } = null!;
    public ICollection<TratamientoProveedor> TratamientosProveedor { get; set; } = new List<TratamientoProveedor>();
    public ICollection<DisponibilidadProveedor> Disponibilidades { get; set; } = new List<DisponibilidadProveedor>();
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
