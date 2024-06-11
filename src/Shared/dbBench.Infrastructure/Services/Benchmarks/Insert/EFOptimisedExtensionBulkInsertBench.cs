using dbBench.Application.Services.Benchmarks;
using dbBench.Domain.dbo;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;

namespace dbBench.Infrastructure.Services.Benchmarks.Insert;

public class EFOptimisedExtensionBulkInsertBench : IInsertBenchmark
{
    public long DapperBench(DbConnection connection, List<User> users)
    {
        return -1;
    }

    public long EFBench(DbContext context, List<User> users)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        context.BulkInsert(users);
        context.SaveChanges();

        stopwatch.Stop();
        long elapsedTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();

        return elapsedTime;
    }
}
