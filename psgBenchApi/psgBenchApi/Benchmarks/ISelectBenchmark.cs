using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace psgBenchApi.Benchmarks
{
    public interface ISelectBenchmark
    {
        long DapperBench(DbConnection connection, int count);
        long EFBench(DbContext context, int count);
    }
}
