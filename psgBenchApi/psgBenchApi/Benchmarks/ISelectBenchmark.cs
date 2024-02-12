using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace psgSqlBenchApi.Benchmarks
{
    public interface ISelectBenchmark
    {
        long DapperBench(DbConnection connection, int count);
        long EFBench(DbContext context, int count);
    }
}
