using Dapper;
using Infrastructure.Enums;
using Infrastructure.Enums.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Npgsql;
using psgBenchApi.Benchmarks.Insert;
using psgBenchApi.Benchmarks.Select;
using psgBenchApi.Generators;
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

            switch (insertType)
            {
                case InsertType.Common:
                    switch (ormType)
                    {
                        case OrmType.Dapper:
                            switch (dbType)
                            {
                                case DbType.Postgre:
                                    result = InsertAdminsBench.DapperBench(postgreConnection, admins);
                                    break;
                                case DbType.SqlServer:
                                    result = InsertAdminsBench.DapperBench(sqlServConnection, admins);
                                    break;
                                default:
                                    return BadRequest("Unknown db type");
                            }                        
                            break;
                        case OrmType.Ef:
                            switch (dbType)
                            {
                                case DbType.Postgre:
                                    result = InsertAdminsBench.EFBench(postgreContext, admins);
                                    break;
                                case DbType.SqlServer:
                                    result = InsertAdminsBench.EFBench(sqlServContext, admins);
                                    break;
                                default:
                                    return BadRequest("Unknown db type");
                            }
                            break;
                        default:
                            return BadRequest("Unknown ORM type");
                    }
                    break;
                case InsertType.Bulk:
                    switch (ormType)
                    {
                        case OrmType.Dapper:
                            switch (dbType)
                            {
                                case DbType.Postgre:
                                    result = BulkInsertAdminsBench.DapperBench(postgreConnection, admins);
                                    break;
                                case DbType.SqlServer:
                                    result = BulkInsertAdminsBench.DapperBench(sqlServConnection, admins);
                                    break;
                                default:
                                    return BadRequest("Unknown db type");
                            }
                            break;
                        case OrmType.Ef:
                            switch (dbType)
                            {
                                case DbType.Postgre:
                                    result = BulkInsertAdminsBench.EFBench(postgreContext, admins);
                                    break;
                                case DbType.SqlServer:
                                    result = BulkInsertAdminsBench.EFBench(sqlServContext, admins);
                                    break;
                                default:
                                    return BadRequest("Unknown db type");
                            }
                            break;
                        default:
                            return BadRequest("Unknown ORM type");
                    }
                    break;
                case InsertType.TransactionBalk:
                    switch (ormType)
                    {
                        case OrmType.Dapper:
                            switch (dbType)
                            {
                                case DbType.Postgre:
                                    result = TransactionBulkInsertAdminsBench.DapperBench(postgreConnection, admins);
                                    break;
                                case DbType.SqlServer:
                                    result = TransactionBulkInsertAdminsBench.DapperBench(sqlServConnection, admins);
                                    break;
                                default:
                                    return BadRequest("Unknown db type");
                            }
                            break;
                        case OrmType.Ef:
                            switch (dbType)
                            {
                                case DbType.Postgre:
                                    result = TransactionBulkInsertAdminsBench.EFBench(postgreContext, admins);
                                    break;
                                case DbType.SqlServer:
                                    result = TransactionBulkInsertAdminsBench.EFBench(sqlServContext, admins);
                                    break;
                                default:
                                    return BadRequest("Unknown db type");
                            }
                            break;
                        default:
                            return BadRequest("Unknown ORM type");
                    }
                    break;
                default:
                    return BadRequest("Unknown insert type");
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
            switch (ormType)
            {
                case OrmType.Dapper:
                    switch (dbType)
                    {
                        case DbType.Postgre:
                            result = SelectAdminsBench.DapperBench(postgreConnection, count);
                            break;
                        case DbType.SqlServer:
                            result = SelectAdminsBench.DapperBench(sqlServConnection, count);
                            break;
                        default:
                            return BadRequest("Unknown db type");
                    }
                    break;
                case OrmType.Ef:
                    switch (dbType)
                    {
                        case DbType.Postgre:
                            result = SelectAdminsBench.EFBench(postgreContext, count);
                            break;
                        case DbType.SqlServer:
                            result = SelectAdminsBench.EFBench(sqlServContext, count);
                            break;
                        default:
                            return BadRequest("Unknown db type");
                    }
                    break;
                default:
                    return BadRequest("Unknown ORM type");
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
