namespace TZISCA.Domain.Entities;

public class Paquete
{
    public int IdPaquete { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<PaqueteTratamiento> PaquetesTratamiento { get; set; } = new List<PaqueteTratamiento>();
}
