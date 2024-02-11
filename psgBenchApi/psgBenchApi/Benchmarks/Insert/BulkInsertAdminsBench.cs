using Dapper;
using Npgsql;
using System.Diagnostics;

namespace psgBenchApi.Benchmarks.Insert
{
    public static class BulkInsertAdminsBench
    {
        public static long DapperBench(NpgsqlConnection connection, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            connection.Execute("INSERT INTO admins (name, age) VALUES (@Name, @Age);", admins);

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }

        public static long EFBench(pgBenchContext context, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            context.Admins.AddRange(admins);
            context.SaveChanges();

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }
    }
}
