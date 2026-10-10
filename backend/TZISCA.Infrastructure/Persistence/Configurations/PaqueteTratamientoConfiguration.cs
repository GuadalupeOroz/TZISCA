using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TZISCA.Domain.Entities;

namespace TZISCA.Infrastructure.Persistence.Configurations;

internal sealed class PaqueteTratamientoConfiguration : IEntityTypeConfiguration<PaqueteTratamiento>
{
    public void Configure(EntityTypeBuilder<PaqueteTratamiento> builder)
    {
        builder.ToTable("PaqueteTratamiento", "catalogo");
        builder.HasKey(x => new { x.IdPaquete, x.IdTratamiento }).HasName("PK_PaqueteTratamiento");
        builder.Property(x => x.IdPaquete).HasColumnName("id_paquete");
        builder.Property(x => x.IdTratamiento).HasColumnName("id_tratamiento");
        builder.HasIndex(x => x.IdTratamiento).HasDatabaseName("IX_PaqueteTratamiento_Tratamiento");
        builder.HasOne(x => x.Paquete).WithMany(x => x.PaquetesTratamiento).HasForeignKey(x => x.IdPaquete).HasConstraintName("FK_PaqueteTratamiento_Paquete").OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Tratamiento).WithMany(x => x.PaquetesTratamiento).HasForeignKey(x => x.IdTratamiento).HasConstraintName("FK_PaqueteTratamiento_Tratamiento").OnDelete(DeleteBehavior.NoAction);
    }
}
