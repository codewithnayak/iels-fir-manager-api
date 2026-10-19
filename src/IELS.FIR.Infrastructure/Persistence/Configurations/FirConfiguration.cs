
using IELS.FIR.Domain.Entities;
using IELS.FIR.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IELS.FIR.Infrastructure.Persistence.Configurations;

public sealed class FirConfiguration : IEntityTypeConfiguration<Fir>
{
    public void Configure(EntityTypeBuilder<Fir> builder)
    {
        builder.ToTable("Firs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.FirNumber)
            .HasConversion(
                number => number.Value,
                value => FirNumber.Create(value))
            .HasMaxLength(30)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.PoliceStationId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DistrictId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.IncidentDate)
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(x => x.ComplainantName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.RegisteredAtUtc)
            .HasColumnType("datetimeoffset")
            .IsRequired();

        // Incident location value object
        builder.OwnsOne(x => x.IncidentLocation, location =>
        {
            location.Property(x => x.AddressLine1)
                .HasMaxLength(250)
                .IsRequired();

            location.Property(x => x.AddressLine2)
                .HasMaxLength(250);

            location.Property(x => x.VillageOrCity)
                .HasMaxLength(100)
                .IsRequired();

            location.Property(x => x.District)
                .HasMaxLength(100)
                .IsRequired();

            location.Property(x => x.State)
                .HasMaxLength(100)
                .IsRequired();

            location.Property(x => x.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false);

            location.Property(x => x.Latitude)
                .HasPrecision(9, 6);

            location.Property(x => x.Longitude)
                .HasPrecision(9, 6);
        });

        builder.Navigation(x => x.IncidentLocation)
            .IsRequired();

        // FIR number unique within a police station
        builder.HasIndex(x => new
        {
            x.PoliceStationId,
            x.FirNumber
        })
        .IsUnique()
        .HasDatabaseName("UX_Firs_PoliceStation_FirNumber");

        builder.HasIndex(x => x.RegisteredAtUtc)
            .HasDatabaseName("IX_Firs_RegisteredAtUtc");
    }
}
