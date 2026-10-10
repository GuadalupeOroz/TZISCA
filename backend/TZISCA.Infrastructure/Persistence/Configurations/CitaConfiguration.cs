using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;
using TZISCA.Domain.Enums;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class CitaConfiguration : IEntityTypeConfiguration<Cita>
{
    public void Configure(EntityTypeBuilder<Cita> builder)
    {
        builder.ToTable("Cita", "reservas", table =>
        {
            table.HasCheckConstraint("CK_Cita_NumeroPersonas", "numero_personas > 0");
            table.HasCheckConstraint("CK_Cita_Horario", "fecha_hora_fin > fecha_hora_inicio");
            table.HasCheckConstraint("CK_Cita_Inicio_Intervalo30", "DATEPART(SECOND, fecha_hora_inicio) = 0 AND DATEPART(MILLISECOND, fecha_hora_inicio) = 0 AND DATEPART(MINUTE, fecha_hora_inicio) IN (0, 30)");
            table.HasCheckConstraint("CK_Cita_Estado", "estado IN (N'PENDIENTE', N'CONFIRMADA', N'EN_ATENCION', N'COMPLETADA', N'CANCELADA', N'EXPIRADA')");
            table.HasCheckConstraint("CK_Cita_PrecioUnitario", "precio_unitario >= 0");
            table.HasCheckConstraint("CK_Cita_Importe", "importe >= 0");
            table.HasCheckConstraint("CK_Cita_Importe_Calculo", "importe = precio_unitario * numero_personas");
            table.HasCheckConstraint("CK_Cita_FechaConfirmacion", "fecha_confirmacion IS NULL OR fecha_confirmacion >= fecha_creacion");
        });
        builder.HasKey(x => x.IdCita).HasName("PK_Cita");
        builder.Property(x => x.IdCita).HasColumnName("id_cita");
        builder.Property(x => x.IdCliente).HasColumnName("id_cliente");
        builder.Property(x => x.IdTratamiento).HasColumnName("id_tratamiento");
        builder.Property(x => x.IdProveedor).HasColumnName("id_proveedor");
        builder.Property(x => x.IdCarrito).HasColumnName("id_carrito");
        builder.Property(x => x.NumeroPersonas).HasColumnName("numero_personas").HasDefaultValue(1).IsRequired();
        builder.Property(x => x.FechaHoraInicio).HasColumnName("fecha_hora_inicio").HasColumnType("datetime2").IsRequired();
        builder.Property(x => x.FechaHoraFin).HasColumnName("fecha_hora_fin").HasColumnType("datetime2").IsRequired();
        builder.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToEstadoCita(value)).HasDefaultValue(EstadoCita.Pendiente).IsRequired();
        builder.Property(x => x.PrecioUnitario).HasColumnName("precio_unitario").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Importe).HasColumnName("importe").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.FechaExpiracionBloqueo).HasColumnName("fecha_expiracion_bloqueo").HasColumnType("datetime2");
        builder.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.FechaConfirmacion).HasColumnName("fecha_confirmacion").HasColumnType("datetime2");
        builder.Property(x => x.Observaciones).HasColumnName("observaciones").HasMaxLength(500);
        builder.HasIndex(x => x.IdCliente).HasDatabaseName("IX_Cita_Cliente");
        builder.HasIndex(x => x.IdTratamiento).HasDatabaseName("IX_Cita_Tratamiento");
        builder.HasIndex(x => new { x.IdProveedor, x.FechaHoraInicio }).HasDatabaseName("IX_Cita_Proveedor_Inicio");
        builder.HasIndex(x => x.IdCarrito).HasDatabaseName("IX_Cita_Carrito");
        builder.HasIndex(x => new { x.Estado, x.FechaHoraInicio }).HasDatabaseName("IX_Cita_Estado_Inicio");
        builder.HasOne(x => x.Cliente).WithMany(x => x.Citas).HasForeignKey(x => x.IdCliente).HasConstraintName("FK_Cita_Cliente").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Tratamiento).WithMany(x => x.Citas).HasForeignKey(x => x.IdTratamiento).HasConstraintName("FK_Cita_Tratamiento").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Proveedor).WithMany(x => x.Citas).HasForeignKey(x => x.IdProveedor).HasConstraintName("FK_Cita_Proveedor").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Carrito).WithMany(x => x.Citas).HasForeignKey(x => x.IdCarrito).HasConstraintName("FK_Cita_Carrito").OnDelete(DeleteBehavior.NoAction);
    }
}
