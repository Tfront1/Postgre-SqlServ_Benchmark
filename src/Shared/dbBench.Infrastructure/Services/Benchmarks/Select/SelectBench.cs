using Dapper;
using dbBench.Application.Services.Benchmarks;
using dbBench.Domain.dbo;
using dbBench.Infrastructure.Database.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Diagnostics;

namespace dbBench.Infrastructure.Services.Benchmarks.Select;

public class SelectBench : ISelectBenchmark
{
    public long DapperBench(DbConnection connection, int count)
    {
        Stopwatch stopwatch = new Stopwatch();
        string query = default;

        if (connection is NpgsqlConnection)
        {
            query = $"SELECT * FROM users LIMIT {count}";
        }
        else if (connection is SqlConnection)
        {
            query = $"SELECT TOP {count} * FROM users";
        }
        else
        {
            throw new ArgumentException("Unknown database connection type");
        }

        stopwatch.Start();

        var users = connection.Query<User>(query).ToList();

        stopwatch.Stop();

        if (users.Count < count)
        {
            return -1;
        }

        long elapsedTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();

        return elapsedTime;
    }

    public long EFBench(DbContext context, int count)
    {
        Stopwatch stopwatch = new Stopwatch();
        List<User> users = default;

        if (context is postgresBenchContext)
        {
            postgresBenchContext tempContext = (postgresBenchContext)context;
            stopwatch.Start();
            users = tempContext.Users.Take(count).ToList();
            stopwatch.Stop();
        }
        else if (context is sqlServerBenchContext)
        {
            sqlServerBenchContext tempContext = (sqlServerBenchContext)context;
            stopwatch.Start();
            users = tempContext.Users.Take(count).ToList();
            stopwatch.Stop();
        }
        else
        {
            throw new ArgumentException("Unknown database context type");
        }

        if (users.Count < count)
        {
            return -1;
        }

        long elapsedTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();

        return elapsedTime;
    }
}
