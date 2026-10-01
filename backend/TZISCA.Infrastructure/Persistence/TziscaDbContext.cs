using Microsoft.EntityFrameworkCore;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence;

public sealed class TziscaDbContext(DbContextOptions<TziscaDbContext> options) : DbContext(options)
{
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<PreferenciaCliente> PreferenciasCliente => Set<PreferenciaCliente>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();
    public DbSet<Carrito> Carritos => Set<Carrito>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<TratamientoProveedor> TratamientosProveedor => Set<TratamientoProveedor>();
    public DbSet<DisponibilidadProveedor> DisponibilidadesProveedor => Set<DisponibilidadProveedor>();
    public DbSet<Paquete> Paquetes => Set<Paquete>();
    public DbSet<PaqueteTratamiento> PaquetesTratamiento => Set<PaqueteTratamiento>();
    public DbSet<Cabina> Cabinas => Set<Cabina>();
    public DbSet<EstadoCabina> EstadosCabina => Set<EstadoCabina>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<CitaCabina> CitasCabina => Set<CitaCabina>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<Cancelacion> Cancelaciones => Set<Cancelacion>();
    public DbSet<Devolucion> Devoluciones => Set<Devolucion>();
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TziscaDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
