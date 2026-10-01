using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Rol", "seguridad", table => table.HasCheckConstraint("CK_Rol_Nombre_NoVacio", "LEN(LTRIM(RTRIM(nombre))) > 0"));
        builder.HasKey(x => x.IdRol).HasName("PK_Rol");
        builder.Property(x => x.IdRol).HasColumnName("id_rol");
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(250);
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => x.Nombre).HasDatabaseName("UQ_Rol_Nombre").IsUnique();
    }
}
