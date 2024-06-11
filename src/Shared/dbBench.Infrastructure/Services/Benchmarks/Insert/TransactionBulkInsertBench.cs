using Dapper;
using dbBench.Application.Services.Benchmarks;
using dbBench.Domain.dbo;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;

namespace dbBench.Infrastructure.Services.Benchmarks.Insert;

public class TransactionBulkInsertBench : IInsertBenchmark
{
    public long DapperBench(DbConnection connection, List<User> users)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                connection.Execute("INSERT INTO users (name, age) VALUES (@Name, @Age);", users, transaction: transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
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

        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                context.AddRange(users);
                context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        stopwatch.Stop();
        long elapsedTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();

        return elapsedTime;
    }
}
