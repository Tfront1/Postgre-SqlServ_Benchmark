using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Diagnostics;

namespace psgBenchApi.Benchmarks.Select
{
    public class SelectAdminsBench: ISelectBenchmark
    {
        public long DapperBench(DbConnection connection, int count)
        {
            Stopwatch stopwatch = new Stopwatch();
            string query = default;

            if (connection is NpgsqlConnection)
            {
                query = $"SELECT * FROM admins LIMIT {count}";
            }
            else if (connection is SqlConnection)
            {
                query = $"SELECT TOP {count} * FROM admins";
            }
            else 
            {
                throw new ArgumentException("Unknown database connection type");
            }

            stopwatch.Start();

            var admins = connection.Query<Admin>(query).ToList();

            stopwatch.Stop();

            if (admins.Count < count)
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
            List<Admin> admins = default;

            if (context is pgBenchContext)
            {
                pgBenchContext tempContext = (pgBenchContext)context;
                stopwatch.Start();
                admins = tempContext.Admins.Take(count).ToList();
                stopwatch.Stop();
            }
            else if (context is sqlBenchContext)
            {
                sqlBenchContext tempContext = (sqlBenchContext)context;
                stopwatch.Start();
                admins = tempContext.Admins.Take(count).ToList();
                stopwatch.Stop();
            }
            else
            {
                throw new ArgumentException("Unknown database context type");
            }

            if (admins.Count < count)
            {
                return -1;
            }

            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }
    }
}
