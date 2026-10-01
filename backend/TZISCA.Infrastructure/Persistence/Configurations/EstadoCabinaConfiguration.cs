using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class EstadoCabinaConfiguration : IEntityTypeConfiguration<EstadoCabina>
{
    public void Configure(EntityTypeBuilder<EstadoCabina> builder)
    {
        builder.ToTable("EstadoCabina", "operacion", table =>
        {
            table.HasCheckConstraint("CK_EstadoCabina_Anterior", "estado_anterior IS NULL OR estado_anterior IN (N'DISPONIBLE', N'OCUPADA', N'LIMPIEZA', N'MANTENIMIENTO')");
            table.HasCheckConstraint("CK_EstadoCabina_Nuevo", "estado_nuevo IN (N'DISPONIBLE', N'OCUPADA', N'LIMPIEZA', N'MANTENIMIENTO')");
        });
        builder.HasKey(x => x.IdEstadoCabina).HasName("PK_EstadoCabina");
        builder.Property(x => x.IdEstadoCabina).HasColumnName("id_estado_cabina");
        builder.Property(x => x.IdCabina).HasColumnName("id_cabina");
        builder.Property(x => x.EstadoAnterior).HasColumnName("estado_anterior").HasMaxLength(20).HasConversion(value => value.HasValue ? EnumSqlConversions.ToSql(value.Value) : null, value => value == null ? null : EnumSqlConversions.ToEstadoCabinaOperativo(value));
        builder.Property(x => x.EstadoNuevo).HasColumnName("estado_nuevo").HasMaxLength(20).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToEstadoCabinaOperativo(value)).IsRequired();
        builder.Property(x => x.FechaCambio).HasColumnName("fecha_cambio").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.IdUsuario).HasColumnName("id_usuario");
        builder.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(250);
        builder.HasIndex(x => new { x.IdCabina, x.FechaCambio }).HasDatabaseName("IX_EstadoCabina_Cabina_Fecha");
        builder.HasOne(x => x.Cabina).WithMany(x => x.EstadosCabina).HasForeignKey(x => x.IdCabina).HasConstraintName("FK_EstadoCabina_Cabina").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Usuario).WithMany(x => x.EstadosCabina).HasForeignKey(x => x.IdUsuario).HasConstraintName("FK_EstadoCabina_Usuario").OnDelete(DeleteBehavior.NoAction);
    }
}
