namespace TZISCA.Domain.Entities;

public class PaqueteTratamiento
{
    public int IdPaquete { get; set; }
    public int IdTratamiento { get; set; }

    public Paquete Paquete { get; set; } = null!;
    public Tratamiento Tratamiento { get; set; } = null!;
}
