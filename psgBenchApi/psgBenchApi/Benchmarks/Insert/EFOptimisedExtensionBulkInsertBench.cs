using Dapper;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using psgSqlBenchApi.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace psgSqlBenchApi.Benchmarks.Insert
{
    public class EFOptimisedExtensionBulkInsertBench : IInsertBenchmark
    {
        public long DapperBench(DbConnection connection, List<Admin> admins)
        {
            return -1;
        }

        public long EFBench(DbContext context, List<Admin> admins)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            context.BulkInsert(admins);
            context.SaveChanges();

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return elapsedTime;
        }
    }
}
