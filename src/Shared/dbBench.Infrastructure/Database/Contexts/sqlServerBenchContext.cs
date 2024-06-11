﻿using dbBench.Domain.dbo;
using dbBench.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace dbBench.Infrastructure.Database.Contexts;

public partial class sqlServerBenchContext : DbContext
{
    public sqlServerBenchContext()
    {
    }

    public sqlServerBenchContext(DbContextOptions<sqlServerBenchContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
