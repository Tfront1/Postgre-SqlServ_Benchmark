using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;
using System.Diagnostics;

namespace psgBenchApi.Benchmarks.Insert
{
    public static class TransactionBulkInsertAdminsBench
    {
        public static long DapperBench(DbConnection connection, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    connection.Execute("INSERT INTO admins (name, age) VALUES (@Name, @Age);", admins, transaction: transaction);
                    transaction.Commit();
                }
                catch (System.Exception)
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

        public static long EFBench(DbContext context, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.AddRange(admins);
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
}
