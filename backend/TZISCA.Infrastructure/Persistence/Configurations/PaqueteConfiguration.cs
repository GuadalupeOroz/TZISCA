using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class PaqueteConfiguration : IEntityTypeConfiguration<Paquete>
{
    public void Configure(EntityTypeBuilder<Paquete> builder)
    {
        builder.ToTable("Paquete", "catalogo", table => table.HasCheckConstraint("CK_Paquete_Nombre_NoVacio", "LEN(LTRIM(RTRIM(nombre))) > 0"));
        builder.HasKey(x => x.IdPaquete).HasName("PK_Paquete");
        builder.Property(x => x.IdPaquete).HasColumnName("id_paquete");
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(120).IsRequired();
        builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("nvarchar(max)");
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => x.Nombre).HasDatabaseName("UQ_Paquete_Nombre").IsUnique();
    }
}
