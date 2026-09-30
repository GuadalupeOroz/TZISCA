namespace TZISCA.Domain.Entities;

public class Usuario
{
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
    public string IdentityUserId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public Rol Rol { get; set; } = null!;
    public Cliente? Cliente { get; set; }
    public Proveedor? Proveedor { get; set; }
    public ICollection<EstadoCabina> EstadosCabina { get; set; } = new List<EstadoCabina>();
    public ICollection<Cancelacion> Cancelaciones { get; set; } = new List<Cancelacion>();
    public ICollection<Devolucion> DevolucionesResponsable { get; set; } = new List<Devolucion>();
}
