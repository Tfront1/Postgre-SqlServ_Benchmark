using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace psgBenchApi.Benchmarks
{
    public interface IInsertBenchmark
    {
        long DapperBench(DbConnection connection, List<Admin> admins);
        long EFBench(DbContext context, List<Admin> admins);
    }
}
