using Dapper;
using dbBench.Application.Services.Benchmarks;
using dbBench.Application.Services.DataGenerators;
using dbBench.Contracts.Enums;
using dbBench.Contracts.Enums.Operations;
using dbBench.Infrastructure.Database.Contexts;
using dbBench.Infrastructure.Services.Benchmarks.Insert;
using dbBench.Infrastructure.Services.Benchmarks.Select;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.Common;

namespace dbBench.Postgres.Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BenchmarkController : ControllerBase
{
    private readonly postgresBenchContext _context;
    private readonly IUserGenerator _userGenerator;
    private readonly IConfiguration _configuration;

    private readonly NpgsqlConnection postgresConnection;
    private readonly postgresBenchContext postgresContext;

    public BenchmarkController(
        IUserGenerator userGenerator, 
        IConfiguration configuration,
        postgresBenchContext context)
    {
        _userGenerator = userGenerator;
        _configuration = configuration;
        _context = context;

        postgresConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")!);
        postgresConnection.Open(); 
    }

    [HttpPost("InsertBench")]
    public async Task<IActionResult> InsertBench(int count, OrmType ormType, InsertType insertType)
    {
        if (count < 0)
        {
            return BadRequest("Count must be 0 or more");
        }

        var users = _userGenerator.GenerateUsers(count);
        long result = default;

        IInsertBenchmark insertBenchmark;

        //Choose type of insert
        switch (insertType)
        {
            case InsertType.Common:
                insertBenchmark = new InsertBench();
                break;
            case InsertType.Bulk:
                insertBenchmark = new BulkInsertBench();
                break;
            case InsertType.TransactionBalk:
                insertBenchmark = new TransactionBulkInsertBench();
                break;
            case InsertType.EFOptimisedExtensionBulkInsert:
                insertBenchmark = new EFOptimisedExtensionBulkInsertBench();
                break;
            default:
                return BadRequest("Unknown insert type");
        }

        //Choose type of orm
        switch (ormType)
        {
            case OrmType.Dapper:
                result = insertBenchmark.DapperBench(postgresConnection, users);
                break;
            case OrmType.Ef:
                result = insertBenchmark.EFBench(_context, users);
                break;
            default:
                return BadRequest("Unknown ORM type");
        }

        if (result < 0)
        {
            return BadRequest("Bad request configuration");
        }

        return Ok(result);
    }

    [HttpGet("SelectBench")]
    public async Task<IActionResult> SelectBench(int count, OrmType ormType)
    {
        if (count < 0)
        {
            return BadRequest("Count must be 0 or more");
        }

        ISelectBenchmark selectBenchmark = new SelectBench();

        long result;
        //Choose type of orm
        switch (ormType)
        {
            case OrmType.Dapper:
                result = selectBenchmark.DapperBench(postgresConnection, count);
                break;
            case OrmType.Ef:
                result = selectBenchmark.EFBench(_context, count);
                break;
            default:
                return BadRequest("Unknown ORM type");
        }

        if (result == -1)
        {
            return BadRequest("Invalid count of selecting, there are not so many objects");
        }

        return Ok(result);
    }

    [HttpDelete("ClearTable")]
    public async Task<IActionResult> ClearTable()
    {
        postgresConnection.Execute("DELETE FROM users");

        return Ok();
    }
}

