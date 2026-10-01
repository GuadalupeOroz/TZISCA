using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;
using TZISCA.Domain.Enums;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class CarritoConfiguration : IEntityTypeConfiguration<Carrito>
{
    public void Configure(EntityTypeBuilder<Carrito> builder)
    {
        builder.ToTable("Carrito", "reservas", table => table.HasCheckConstraint("CK_Carrito_Estado", "estado IN (N'ACTIVO', N'CONVERTIDO', N'ABANDONADO', N'EXPIRADO')"));
        builder.HasKey(x => x.IdCarrito).HasName("PK_Carrito");
        builder.Property(x => x.IdCarrito).HasColumnName("id_carrito");
        builder.Property(x => x.IdCliente).HasColumnName("id_cliente");
        builder.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20).HasConversion(value => EnumSqlConversions.ToSql(value), value => EnumSqlConversions.ToEstadoCarrito(value)).HasDefaultValue(EstadoCarrito.Activo).IsRequired();
        builder.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.FechaActualizacion).HasColumnName("fecha_actualizacion").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.HasIndex(x => x.IdCliente).HasDatabaseName("IX_Carrito_Cliente");
        builder.HasOne(x => x.Cliente).WithMany(x => x.Carritos).HasForeignKey(x => x.IdCliente).HasConstraintName("FK_Carrito_Cliente").OnDelete(DeleteBehavior.NoAction);
    }
}
