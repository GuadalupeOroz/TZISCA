using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;
using TZISCA.Domain.Enums;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class TransaccionConfiguration : IEntityTypeConfiguration<Transaccion>
{
    public void Configure(EntityTypeBuilder<Transaccion> builder)
    {
        builder.ToTable("Transaccion", "pagos", table => table.HasCheckConstraint("CK_Transaccion_Tipo", "tipo IN (N'PAGO', N'REINTENTO', N'DEVOLUCION')"));
        builder.HasKey(x => x.IdTransaccion).HasName("PK_Transaccion");
        builder.Property(x => x.IdTransaccion).HasColumnName("id_transaccion");
        builder.Property(x => x.IdPago).HasColumnName("id_pago");
        builder.Property(x => x.ReferenciaExterna).HasColumnName("referencia_externa").HasMaxLength(150);
        builder.Property(x => x.ProveedorPago).HasColumnName("proveedor_pago").HasMaxLength(40).HasDefaultValue("STRIPE").IsRequired();
        builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(30).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToTipoTransaccion(value)).IsRequired();
        builder.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(30).IsRequired();
        builder.Property(x => x.CodigoRespuesta).HasColumnName("codigo_respuesta").HasMaxLength(100);
        builder.Property(x => x.Fecha).HasColumnName("fecha").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.HasIndex(x => x.IdPago).HasDatabaseName("IX_Transaccion_Pago");
        builder.HasIndex(x => x.ReferenciaExterna).HasDatabaseName("IX_Transaccion_ReferenciaExterna").HasFilter("[referencia_externa] IS NOT NULL");
        builder.HasOne(x => x.Pago).WithMany(x => x.Transacciones).HasForeignKey(x => x.IdPago).HasConstraintName("FK_Transaccion_Pago").OnDelete(DeleteBehavior.NoAction);
    }
}
