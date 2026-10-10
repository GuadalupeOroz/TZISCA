using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;
using TZISCA.Domain.Enums;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("Pago", "pagos", table =>
        {
            table.HasCheckConstraint("CK_Pago_Monto", "monto > 0");
            table.HasCheckConstraint("CK_Pago_Moneda", "LEN(moneda) = 3");
            table.HasCheckConstraint("CK_Pago_Estado", "estado IN (N'PENDIENTE', N'PROCESANDO', N'PAGADO', N'FALLIDO', N'CANCELADO', N'REEMBOLSADO', N'REEMBOLSADO_PARCIALMENTE')");
            table.HasCheckConstraint("CK_Pago_FechaPago", "fecha_pago IS NULL OR fecha_pago >= fecha_creacion");
        });
        builder.HasKey(x => x.IdPago).HasName("PK_Pago");
        builder.Property(x => x.IdPago).HasColumnName("id_pago");
        builder.Property(x => x.IdCita).HasColumnName("id_cita");
        builder.Property(x => x.Monto).HasColumnName("monto").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.Moneda).HasColumnName("moneda").HasColumnType("char(3)").HasMaxLength(3).HasDefaultValue("MXN").IsRequired();
        builder.Property(x => x.MetodoPago).HasColumnName("metodo_pago").HasMaxLength(40).IsRequired();
        builder.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(30).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToEstadoPago(value)).HasDefaultValue(EstadoPago.Pendiente).IsRequired();
        builder.Property(x => x.Referencia).HasColumnName("referencia").HasMaxLength(150);
        builder.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.FechaPago).HasColumnName("fecha_pago").HasColumnType("datetime2");
        builder.HasIndex(x => x.IdCita).HasDatabaseName("IX_Pago_Cita");
        builder.HasIndex(x => x.Estado).HasDatabaseName("IX_Pago_Estado");
        builder.HasIndex(x => x.Referencia).HasDatabaseName("IX_Pago_Referencia").HasFilter("[referencia] IS NOT NULL");
        builder.HasOne(x => x.Cita).WithMany(x => x.Pagos).HasForeignKey(x => x.IdCita).HasConstraintName("FK_Pago_Cita").OnDelete(DeleteBehavior.NoAction);
    }
}
