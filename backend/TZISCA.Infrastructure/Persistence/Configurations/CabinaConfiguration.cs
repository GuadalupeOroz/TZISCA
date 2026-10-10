using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;
using TZISCA.Domain.Enums;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class CabinaConfiguration : IEntityTypeConfiguration<Cabina>
{
    public void Configure(EntityTypeBuilder<Cabina> builder)
    {
        builder.ToTable("Cabina", "operacion", table =>
        {
            table.HasCheckConstraint("CK_Cabina_Nombre_NoVacio", "LEN(LTRIM(RTRIM(nombre))) > 0");
            table.HasCheckConstraint("CK_Cabina_Capacidad", "capacidad_maxima > 0");
            table.HasCheckConstraint("CK_Cabina_Prioridad", "prioridad >= 0");
            table.HasCheckConstraint("CK_Cabina_Estado", "estado IN (N'DISPONIBLE', N'OCUPADA', N'LIMPIEZA', N'MANTENIMIENTO')");
        });
        builder.HasKey(x => x.IdCabina).HasName("PK_Cabina");
        builder.Property(x => x.IdCabina).HasColumnName("id_cabina");
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(120).IsRequired();
        builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(60).IsRequired();
        builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasColumnType("nvarchar(max)");
        builder.Property(x => x.CapacidadMaxima).HasColumnName("capacidad_maxima").IsRequired();
        builder.Property(x => x.Caracteristicas).HasColumnName("caracteristicas").HasColumnType("nvarchar(max)");
        builder.Property(x => x.Beneficios).HasColumnName("beneficios").HasColumnType("nvarchar(max)");
        builder.Property(x => x.Prioridad).HasColumnName("prioridad").HasDefaultValue(0).IsRequired();
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToEstadoCabinaOperativo(value)).HasDefaultValue(EstadoCabinaOperativo.Disponible).IsRequired();
        builder.HasIndex(x => x.Nombre).HasDatabaseName("UQ_Cabina_Nombre").IsUnique();
    }
}
