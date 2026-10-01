using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class CitaCabinaConfiguration : IEntityTypeConfiguration<CitaCabina>
{
    public void Configure(EntityTypeBuilder<CitaCabina> builder)
    {
        builder.ToTable("CitaCabina", "reservas");
        builder.HasKey(x => new { x.IdCita, x.IdCabina }).HasName("PK_CitaCabina");
        builder.Property(x => x.IdCita).HasColumnName("id_cita");
        builder.Property(x => x.IdCabina).HasColumnName("id_cabina");
        builder.Property(x => x.FechaAsignacion).HasColumnName("fecha_asignacion").HasColumnType("datetime2").HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true).IsRequired();
        builder.HasIndex(x => x.IdCabina).HasDatabaseName("IX_CitaCabina_Cabina");
        builder.HasOne(x => x.Cita).WithMany(x => x.CitasCabina).HasForeignKey(x => x.IdCita).HasConstraintName("FK_CitaCabina_Cita").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Cabina).WithMany(x => x.CitasCabina).HasForeignKey(x => x.IdCabina).HasConstraintName("FK_CitaCabina_Cabina").OnDelete(DeleteBehavior.NoAction);
    }
}
