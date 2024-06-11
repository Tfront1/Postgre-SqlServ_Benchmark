using dbBench.Domain.dbo;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace dbBench.Application.Services.Benchmarks;

public interface IInsertBenchmark
{
    long DapperBench(DbConnection connection, List<User> users);
    long EFBench(DbContext context, List<User> users);
}
