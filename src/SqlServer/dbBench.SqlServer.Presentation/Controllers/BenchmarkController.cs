﻿using Dapper;
using dbBench.Application.Services.Benchmarks;
using dbBench.Application.Services.DataGenerators;
using dbBench.Contracts.Enums;
using dbBench.Contracts.Enums.Operations;
using dbBench.Infrastructure.Database.Contexts;
using dbBench.Infrastructure.Services.Benchmarks.Insert;
using dbBench.Infrastructure.Services.Benchmarks.Select;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace dbBench.SqlServer.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BenchmarkController : ControllerBase
{
    private readonly sqlServerBenchContext _context;
    private readonly IUserGenerator _userGenerator;
    private readonly IConfiguration _configuration;

    private readonly SqlConnection sqlServerConnection;
    private readonly sqlServerBenchContext sqlServerContext;

    public BenchmarkController(
        IUserGenerator userGenerator, 
        IConfiguration configuration,
        sqlServerBenchContext context)
    {
        _userGenerator = userGenerator;
        _configuration = configuration;
        _context = context;

        sqlServerConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")!);
        sqlServerConnection.Open();
    }

    [HttpPost("InsertBench")]
    public async Task<long> InsertBench(int count, OrmType ormType, InsertType insertType)
    {
        if (count < 0)
        {
            return 0;
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
                return 0;
        }

        //Choose type of orm
        switch (ormType)
        {
            case OrmType.Dapper:
                result = insertBenchmark.DapperBench(sqlServerConnection, users);
                break;
            case OrmType.Ef:
                result = insertBenchmark.EFBench(_context, users);
                break;
            default:
                return 0;
        }

        if (result < 0)
        {
            return 0;
        }

        return result;
    }

    [HttpGet("SelectBench")]
    public async Task<long> SelectBench(int count, OrmType ormType)
    {
        if (count < 0)
        {
            return 0;
        }

        ISelectBenchmark selectBenchmark = new SelectBench();

        long result;
        //Choose type of orm
        switch (ormType)
        {
            case OrmType.Dapper:
                result = selectBenchmark.DapperBench(sqlServerConnection, count);
                break;
            case OrmType.Ef:
                result = selectBenchmark.EFBench(_context, count);
                break;
            default:
                return 0;
        }

        if (result == -1)
        {
            return 0;
        }

        return result;
    }

    [HttpDelete("ClearTable")]
    public async Task<IActionResult> ClearTable()
    {
        sqlServerConnection.Execute("DELETE FROM users");

        return Ok();
    }
}
