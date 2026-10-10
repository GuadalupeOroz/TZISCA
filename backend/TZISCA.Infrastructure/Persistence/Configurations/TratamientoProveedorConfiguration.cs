using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class TratamientoProveedorConfiguration : IEntityTypeConfiguration<TratamientoProveedor>
{
    public void Configure(EntityTypeBuilder<TratamientoProveedor> builder)
    {
        builder.ToTable("TratamientoProveedor", "operacion");
        builder.HasKey(x => new { x.IdTratamiento, x.IdProveedor }).HasName("PK_TratamientoProveedor");
        builder.Property(x => x.IdTratamiento).HasColumnName("id_tratamiento");
        builder.Property(x => x.IdProveedor).HasColumnName("id_proveedor");
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => x.IdProveedor).HasDatabaseName("IX_TratamientoProveedor_Proveedor");
        builder.HasOne(x => x.Tratamiento).WithMany(x => x.TratamientosProveedor).HasForeignKey(x => x.IdTratamiento).HasConstraintName("FK_TratamientoProveedor_Tratamiento").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Proveedor).WithMany(x => x.TratamientosProveedor).HasForeignKey(x => x.IdProveedor).HasConstraintName("FK_TratamientoProveedor_Proveedor").OnDelete(DeleteBehavior.NoAction);
    }
}
