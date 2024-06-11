using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using dbBench.Domain.dbo;

namespace dbBench.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.Age).HasColumnName("age");

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
    }
}