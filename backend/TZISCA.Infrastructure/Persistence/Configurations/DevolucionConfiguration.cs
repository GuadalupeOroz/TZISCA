using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;
using TZISCA.Domain.Enums;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class DevolucionConfiguration : IEntityTypeConfiguration<Devolucion>
{
    public void Configure(EntityTypeBuilder<Devolucion> builder)
    {
        builder.ToTable("Devolucion", "pagos", table =>
        {
            table.HasCheckConstraint("CK_Devolucion_Tipo", "tipo IN (N'TOTAL', N'PARCIAL', N'SIN_DEVOLUCION')");
            table.HasCheckConstraint("CK_Devolucion_Porcentaje", "porcentaje IN (0, 50, 100)");
            table.HasCheckConstraint("CK_Devolucion_Monto", "monto >= 0");
            table.HasCheckConstraint("CK_Devolucion_Estado", "estado IN (N'PENDIENTE', N'PROCESANDO', N'COMPLETADA', N'FALLIDA', N'CANCELADA')");
            table.HasCheckConstraint("CK_Devolucion_Fechas", "fecha_procesamiento IS NULL OR fecha_procesamiento >= fecha_solicitud");
        });
        builder.HasKey(x => x.IdDevolucion).HasName("PK_Devolucion");
        builder.Property(x => x.IdDevolucion).HasColumnName("id_devolucion");
        builder.Property(x => x.IdPago).HasColumnName("id_pago");
        builder.Property(x => x.IdCancelacion).HasColumnName("id_cancelacion");
        builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(20).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToTipoDevolucion(value)).IsRequired();
        builder.Property(x => x.Porcentaje).HasColumnName("porcentaje").HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.Monto).HasColumnName("monto").HasPrecision(10, 2).HasDefaultValue(0m).IsRequired();
        builder.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToEstadoDevolucion(value)).HasDefaultValue(EstadoDevolucion.Pendiente).IsRequired();
        builder.Property(x => x.FechaSolicitud).HasColumnName("fecha_solicitud").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.FechaProcesamiento).HasColumnName("fecha_procesamiento").HasColumnType("datetime2");
        builder.Property(x => x.IdUsuarioResponsable).HasColumnName("id_usuario_responsable");
        builder.HasIndex(x => x.IdPago).HasDatabaseName("IX_Devolucion_Pago");
        builder.HasIndex(x => x.IdCancelacion).HasDatabaseName("IX_Devolucion_Cancelacion");
        builder.HasOne(x => x.Pago).WithMany(x => x.Devoluciones).HasForeignKey(x => x.IdPago).HasConstraintName("FK_Devolucion_Pago").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Cancelacion).WithMany(x => x.Devoluciones).HasForeignKey(x => x.IdCancelacion).HasConstraintName("FK_Devolucion_Cancelacion").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.UsuarioResponsable).WithMany(x => x.DevolucionesResponsable).HasForeignKey(x => x.IdUsuarioResponsable).HasConstraintName("FK_Devolucion_UsuarioResponsable").OnDelete(DeleteBehavior.NoAction);
    }
}
