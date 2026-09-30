namespace TZISCA.Domain.Entities;

public class Tratamiento
{
    public int IdTratamiento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int DuracionMinutos { get; set; }
    public decimal PrecioBase { get; set; }
    public string? RequisitosCabina { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<TratamientoProveedor> TratamientosProveedor { get; set; } = new List<TratamientoProveedor>();
    public ICollection<PaqueteTratamiento> PaquetesTratamiento { get; set; } = new List<PaqueteTratamiento>();
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
}
