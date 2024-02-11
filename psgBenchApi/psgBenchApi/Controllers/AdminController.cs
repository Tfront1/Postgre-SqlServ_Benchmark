using Dapper;
using Infrastructure.Enums;
using Infrastructure.Enums.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using psgBenchApi.Benchmarks;
using psgBenchApi.Benchmarks.Insert;
using psgBenchApi.Benchmarks.Select;
using psgBenchApi.Generators;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace psgBenchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly NpgsqlConnection postgreConnection;
        private readonly pgBenchContext postgreContext;
        private readonly SqlConnection sqlServConnection;
        private readonly sqlBenchContext sqlServContext;

        public AdminController()
        {
            postgreConnection = new NpgsqlConnection("Host=localhost;Port=5432;Database=pgBench;Username=postgres;Password=358145358145Qq");
            postgreConnection.Open();

            sqlServConnection = new SqlConnection("Server=DESKTOP-I51U01F\\SQLEXPRESS;Database=sqlBench;Trusted_Connection=True;TrustServerCertificate=True;");
            sqlServConnection.Open();

            postgreContext = new pgBenchContext();
            sqlServContext = new sqlBenchContext();
        }

        [HttpPost("InsertAdminBench")]
        public async Task<IActionResult> InsertAdminBench(int count, DbType dbType, OrmType ormType, InsertType insertType)
        {
            if (count < 0)
            {
                return BadRequest("Count must be 0 or more");
            }

            var admins = AdminGenerator.GenerateAdmins(count);
            long result = default;

            IInsertBenchmark insertBenchmark;
            DbConnection connection;
            DbContext context;

            //Choose type of insert
            switch (insertType) 
            {
                case InsertType.Common:
                    insertBenchmark = new InsertAdminsBench();
                    break;
                case InsertType.Bulk:
                    insertBenchmark = new BulkInsertAdminsBench();
                    break;
                case InsertType.TransactionBalk:
                    insertBenchmark = new TransactionBulkInsertAdminsBench();
                    break;
                default:
                    return BadRequest("Unknown insert type");
            }

            //Choose type of database
            switch (dbType)
            {
                case DbType.Postgre:
                    connection = postgreConnection;
                    context = postgreContext;
                    break;
                case DbType.SqlServer:
                    connection = sqlServConnection;
                    context = sqlServContext;
                    break;
                default:
                    return BadRequest("Unknown db type");
            }

            //Choose type of orm
            switch (ormType)
            {
                case OrmType.Dapper:
                    result = insertBenchmark.DapperBench(connection, admins);
                    break;
                case OrmType.Ef:
                    result = insertBenchmark.EFBench(context, admins);
                    break;
                default:
                    return BadRequest("Unknown ORM type");
            }
            
            return Ok(result);
        }

        [HttpGet("SelectAdminBench")]
        public async Task<IActionResult> SelectAdminBench(int count, DbType dbType, OrmType ormType)
        {
            long result = default;

            if (count < 0)
            {
                return BadRequest("Count must be 0 or more");
            }

            ISelectBenchmark selectBenchmark = new SelectAdminsBench();

            DbConnection connection;
            DbContext context;

            //Choose type of database
            switch (dbType)
            {
                case DbType.Postgre:
                    connection = postgreConnection;
                    context = postgreContext;
                    break;
                case DbType.SqlServer:
                    connection = sqlServConnection;
                    context = sqlServContext;
                    break;
                default:
                    return BadRequest("Unknown db type");
            }

            //Choose type of orm
            switch (ormType)
            {
                case OrmType.Dapper:
                    result = selectBenchmark.DapperBench(connection, count);
                    break;
                case OrmType.Ef:
                    result = selectBenchmark.EFBench(context, count);
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
        public async Task<IActionResult> ClearTable(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Postgre:
                    postgreConnection.Execute("DELETE FROM admins");
                    break;
                case DbType.SqlServer:
                    sqlServConnection.Execute("DELETE FROM admins");
                    break;
                default:
                    return BadRequest("Unknown db type");
            }
            return Ok();
        }
    }
}
