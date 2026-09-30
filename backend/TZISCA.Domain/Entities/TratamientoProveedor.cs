namespace TZISCA.Domain.Entities;

public class TratamientoProveedor
{
    public int IdTratamiento { get; set; }
    public int IdProveedor { get; set; }
    public bool Activo { get; set; } = true;

    public Tratamiento Tratamiento { get; set; } = null!;
    public Proveedor Proveedor { get; set; } = null!;
}
