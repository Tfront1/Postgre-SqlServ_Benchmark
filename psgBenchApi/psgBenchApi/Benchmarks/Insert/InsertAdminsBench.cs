using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace psgBenchApi.Benchmarks.Insert
{
    public static class InsertAdminsBench
    {
        public static long DapperBench(DbConnection connection, List<Admin> admins) {
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

        public static long EFBench(DbContext context, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var admin in admins)
            {
                context.Add(admin);
                context.SaveChanges();
            }

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }
    }

}
