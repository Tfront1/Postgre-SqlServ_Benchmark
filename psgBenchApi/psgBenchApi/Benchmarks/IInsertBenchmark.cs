using Microsoft.EntityFrameworkCore;
using psgSqlBenchApi.Models;
using System.Data.Common;

namespace psgSqlBenchApi.Benchmarks
{
    public interface IInsertBenchmark
    {
        long DapperBench(DbConnection connection, List<Admin> admins);
        long EFBench(DbContext context, List<Admin> admins);
    }
}
