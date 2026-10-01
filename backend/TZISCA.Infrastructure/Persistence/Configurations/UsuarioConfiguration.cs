using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario", "seguridad", table =>
        {
            table.HasCheckConstraint("CK_Usuario_Nombre_NoVacio", "LEN(LTRIM(RTRIM(nombre))) > 0");
            table.HasCheckConstraint("CK_Usuario_Correo_NoVacio", "LEN(LTRIM(RTRIM(correo))) > 0");
        });
        builder.HasKey(x => x.IdUsuario).HasName("PK_Usuario");
        builder.Property(x => x.IdUsuario).HasColumnName("id_usuario");
        builder.Property(x => x.IdRol).HasColumnName("id_rol");
        builder.Property(x => x.IdentityUserId).HasColumnName("identity_user_id").HasMaxLength(450).IsRequired();
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Correo).HasColumnName("correo").HasMaxLength(256).IsRequired();
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.Property(x => x.FechaRegistro).HasColumnName("fecha_registro").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.HasIndex(x => x.IdRol).HasDatabaseName("IX_Usuario_Rol");
        builder.HasIndex(x => x.IdentityUserId).HasDatabaseName("UQ_Usuario_IdentityUserId").IsUnique();
        builder.HasIndex(x => x.Correo).HasDatabaseName("UQ_Usuario_Correo").IsUnique();
        builder.HasOne(x => x.Rol).WithMany(x => x.Usuarios).HasForeignKey(x => x.IdRol).HasConstraintName("FK_Usuario_Rol").OnDelete(DeleteBehavior.NoAction);
    }
}
