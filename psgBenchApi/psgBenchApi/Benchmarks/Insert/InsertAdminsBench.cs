﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using psgSqlBenchApi.Models;
using System.Data.Common;
using System.Diagnostics;

namespace psgSqlBenchApi.Benchmarks.Insert
{
    public class InsertAdminsBench: IInsertBenchmark
    {
        public long DapperBench(DbConnection connection, List<Admin> admins) {
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

        public long EFBench(DbContext context, List<Admin> admins)
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
