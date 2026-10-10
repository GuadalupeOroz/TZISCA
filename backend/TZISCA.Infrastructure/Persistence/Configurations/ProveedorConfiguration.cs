using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        builder.ToTable("Proveedor", "operacion");
        builder.HasKey(x => x.IdProveedor).HasName("PK_Proveedor");
        builder.Property(x => x.IdProveedor).HasColumnName("id_proveedor");
        builder.Property(x => x.IdUsuario).HasColumnName("id_usuario");
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.FechaAlta).HasColumnName("fecha_alta").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.HasIndex(x => x.IdUsuario).HasDatabaseName("UQ_Proveedor_Usuario").IsUnique();
        builder.HasOne(x => x.Usuario).WithOne(x => x.Proveedor).HasForeignKey<Proveedor>(x => x.IdUsuario).HasConstraintName("FK_Proveedor_Usuario").OnDelete(DeleteBehavior.NoAction);
    }
}
