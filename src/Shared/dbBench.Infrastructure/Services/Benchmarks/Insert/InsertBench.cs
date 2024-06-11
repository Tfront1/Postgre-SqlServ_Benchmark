using Dapper;
using dbBench.Application.Services.Benchmarks;
using dbBench.Domain.dbo;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;

namespace dbBench.Infrastructure.Services.Benchmarks.Insert;

public class InsertBench : IInsertBenchmark
{
    public long DapperBench(DbConnection connection, List<User> users)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (var user in users)
        {
            connection.Execute("INSERT INTO users (name, age) VALUES (@Name, @Age);", user);
        }

        stopwatch.Stop();
        long elapsedTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();

        return elapsedTime;
    }

    public long EFBench(DbContext context, List<User> users)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (var user in users)
        {
            context.Add(user);
            context.SaveChanges();
        }

        stopwatch.Stop();
        long elapsedTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();

        return elapsedTime;
    }
}
