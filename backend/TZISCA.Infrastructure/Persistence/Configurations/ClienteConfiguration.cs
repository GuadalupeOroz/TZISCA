using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Cliente", "seguridad");
        builder.HasKey(x => x.IdCliente).HasName("PK_Cliente");
        builder.Property(x => x.IdCliente).HasColumnName("id_cliente");
        builder.Property(x => x.IdUsuario).HasColumnName("id_usuario");
        builder.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(25);
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.FechaRegistro).HasColumnName("fecha_registro").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.HasIndex(x => x.IdUsuario).HasDatabaseName("UQ_Cliente_Usuario").IsUnique();
        builder.HasOne(x => x.Usuario).WithOne(x => x.Cliente).HasForeignKey<Cliente>(x => x.IdUsuario).HasConstraintName("FK_Cliente_Usuario").OnDelete(DeleteBehavior.NoAction);
    }
}
