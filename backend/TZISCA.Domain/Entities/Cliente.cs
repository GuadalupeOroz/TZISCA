namespace TZISCA.Domain.Entities;

public class Cliente
{
    public int IdCliente { get; set; }
    public int IdUsuario { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public Usuario Usuario { get; set; } = null!;
    public PreferenciaCliente? PreferenciaCliente { get; set; }
    public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
