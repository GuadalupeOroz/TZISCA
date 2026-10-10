using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class DisponibilidadProveedorConfiguration : IEntityTypeConfiguration<DisponibilidadProveedor>
{
    public void Configure(EntityTypeBuilder<DisponibilidadProveedor> builder)
    {
        builder.ToTable("DisponibilidadProveedor", "operacion", table =>
        {
            table.HasCheckConstraint("CK_DisponibilidadProveedor_Horario", "fecha_hora_fin > fecha_hora_inicio");
            table.HasCheckConstraint("CK_DisponibilidadProveedor_Tipo", "tipo IN (N'DISPONIBLE', N'NO_DISPONIBLE')");
        });
        builder.HasKey(x => x.IdDisponibilidad).HasName("PK_DisponibilidadProveedor");
        builder.Property(x => x.IdDisponibilidad).HasColumnName("id_disponibilidad");
        builder.Property(x => x.IdProveedor).HasColumnName("id_proveedor");
        builder.Property(x => x.FechaHoraInicio).HasColumnName("fecha_hora_inicio").HasColumnType("datetime2").IsRequired();
        builder.Property(x => x.FechaHoraFin).HasColumnName("fecha_hora_fin").HasColumnType("datetime2").IsRequired();
        builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(20).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToTipoDisponibilidadProveedor(value)).IsRequired();
        builder.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(250);
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => new { x.IdProveedor, x.FechaHoraInicio }).HasDatabaseName("IX_DisponibilidadProveedor_Proveedor_Inicio");
        builder.HasOne(x => x.Proveedor).WithMany(x => x.Disponibilidades).HasForeignKey(x => x.IdProveedor).HasConstraintName("FK_DisponibilidadProveedor_Proveedor").OnDelete(DeleteBehavior.NoAction);
    }
}
