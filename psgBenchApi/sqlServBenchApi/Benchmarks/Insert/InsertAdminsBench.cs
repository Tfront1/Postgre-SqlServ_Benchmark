using Dapper;
using Microsoft.Data.SqlClient;
using sqlServBenchApi.Data;
using System.Diagnostics;

namespace sqlServBenchApi.Benchmarks.Insert
{
    public static class InsertAdminsBench
    {
        public static long DapperBench(SqlConnection connection, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var admin in admins)
            {
                connection.Execute("INSERT INTO admins (name, age) VALUES (@Name, @Age)", admin);
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }

        public static long EFBench(sqlBenchContext context, List<Admin> admins)
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
