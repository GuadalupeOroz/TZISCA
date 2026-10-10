using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class TratamientoConfiguration : IEntityTypeConfiguration<Tratamiento>
{
    public void Configure(EntityTypeBuilder<Tratamiento> builder)
    {
        builder.ToTable("Tratamiento", "catalogo", table =>
        {
            table.HasCheckConstraint("CK_Tratamiento_Nombre_NoVacio", "LEN(LTRIM(RTRIM(nombre))) > 0");
            table.HasCheckConstraint("CK_Tratamiento_Duracion", "duracion_minutos > 0");
            table.HasCheckConstraint("CK_Tratamiento_PrecioBase", "precio_base >= 0");
        });
        builder.HasKey(x => x.IdTratamiento).HasName("PK_Tratamiento");
        builder.Property(x => x.IdTratamiento).HasColumnName("id_tratamiento");
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(120).IsRequired();
        builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(x => x.DuracionMinutos).HasColumnName("duracion_minutos").IsRequired();
        builder.Property(x => x.PrecioBase).HasColumnName("precio_base").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.RequisitosCabina).HasColumnName("requisitos_cabina").HasMaxLength(500);
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => x.Nombre).HasDatabaseName("UQ_Tratamiento_Nombre").IsUnique();
    }
}
