using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class PreferenciaClienteConfiguration : IEntityTypeConfiguration<PreferenciaCliente>
{
    public void Configure(EntityTypeBuilder<PreferenciaCliente> builder)
    {
        builder.ToTable("PreferenciaCliente", "seguridad");
        builder.HasKey(x => x.IdPreferencia).HasName("PK_PreferenciaCliente");
        builder.Property(x => x.IdPreferencia).HasColumnName("id_preferencia");
        builder.Property(x => x.IdCliente).HasColumnName("id_cliente");
        builder.Property(x => x.TipoExperiencia).HasColumnName("tipo_experiencia").HasMaxLength(100);
        builder.Property(x => x.Caracteristicas).HasColumnName("caracteristicas").HasMaxLength(500);
        builder.Property(x => x.Observaciones).HasColumnName("observaciones").HasMaxLength(500);
        builder.Property(x => x.FechaActualizacion).HasColumnName("fecha_actualizacion").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.HasIndex(x => x.IdCliente).HasDatabaseName("UQ_PreferenciaCliente_Cliente").IsUnique();
        builder.HasOne(x => x.Cliente).WithOne(x => x.PreferenciaCliente).HasForeignKey<PreferenciaCliente>(x => x.IdCliente).HasConstraintName("FK_PreferenciaCliente_Cliente").OnDelete(DeleteBehavior.NoAction);
    }
}
