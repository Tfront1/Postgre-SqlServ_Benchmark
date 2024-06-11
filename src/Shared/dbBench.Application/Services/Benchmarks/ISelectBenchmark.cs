using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace dbBench.Application.Services.Benchmarks;

public interface ISelectBenchmark
{
    long DapperBench(DbConnection connection, int count);
    long EFBench(DbContext context, int count);
}
