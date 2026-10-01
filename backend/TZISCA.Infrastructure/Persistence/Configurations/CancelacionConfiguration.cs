using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class CancelacionConfiguration : IEntityTypeConfiguration<Cancelacion>
{
    public void Configure(EntityTypeBuilder<Cancelacion> builder)
    {
        builder.ToTable("Cancelacion", "pagos", table => table.HasCheckConstraint("CK_Cancelacion_Motivo_NoVacio", "LEN(LTRIM(RTRIM(motivo))) > 0"));
        builder.HasKey(x => x.IdCancelacion).HasName("PK_Cancelacion");
        builder.Property(x => x.IdCancelacion).HasColumnName("id_cancelacion");
        builder.Property(x => x.IdCita).HasColumnName("id_cita");
        builder.Property(x => x.IdUsuario).HasColumnName("id_usuario");
        builder.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(500).IsRequired();
        builder.Property(x => x.AtribuibleSpa).HasColumnName("atribuible_spa").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.FechaCancelacion).HasColumnName("fecha_cancelacion").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.Observaciones).HasColumnName("observaciones").HasMaxLength(500);
        builder.HasIndex(x => x.IdCita).HasDatabaseName("IX_Cancelacion_Cita");
        builder.HasIndex(x => x.IdUsuario).HasDatabaseName("IX_Cancelacion_Usuario");
        builder.HasOne(x => x.Cita).WithMany(x => x.Cancelaciones).HasForeignKey(x => x.IdCita).HasConstraintName("FK_Cancelacion_Cita").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Usuario).WithMany(x => x.Cancelaciones).HasForeignKey(x => x.IdUsuario).HasConstraintName("FK_Cancelacion_Usuario").OnDelete(DeleteBehavior.NoAction);
    }
}
