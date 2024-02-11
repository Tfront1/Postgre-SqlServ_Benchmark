using BenchmarkDotNet.Attributes;
using Dapper;
using Npgsql;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace psgBenchApi.Benchmarks.Insert
{
    public static class InsertAdminsBench
    {
        public static long DapperBench(NpgsqlConnection connection, List<Admin> admins) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var admin in admins) {
                connection.Execute("INSERT INTO admins (name, age) VALUES (@Name, @Age);", admin);
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }

        public static long EFBench(pgBenchContext context, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var admin in admins)
            {
                context.Admins.Add(admin);
                context.SaveChanges();
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }
    }

}
