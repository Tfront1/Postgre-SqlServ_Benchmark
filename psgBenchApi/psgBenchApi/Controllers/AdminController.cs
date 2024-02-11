using Dapper;
using Infrastructure.Enums;
using Infrastructure.Enums.Operations;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using psgBenchApi.Benchmarks.Insert;
using psgBenchApi.Generators;

namespace psgBenchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly NpgsqlConnection connection;
        private readonly pgBenchContext context;

        public AdminController()
        {
            connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=pgBench;Username=postgres;Password=358145358145Qq");
            connection.Open();
            context = new pgBenchContext();
        }

        [HttpPost("InsertAdminDapperBench")]
        public async Task<IActionResult> InsertAdminDapperBench(int count, OrmEnum ormEnum, InsertTypesEnum insertTypesEnum)
        {  

            var admins = AdminGenerator.GenerateAdmins(count);
            long result = default;

            switch (insertTypesEnum)
            {
                case InsertTypesEnum.Common:
                    switch (ormEnum)
                    {
                        case OrmEnum.Dapper:
                            result = InsertAdminsBench.DapperBench(connection , admins);
                            break;
                        case OrmEnum.Ef:
                            result = InsertAdminsBench.EFBench(context, admins);
                            break;
                        default:
                            return BadRequest("Unknown ORM type");
                    }
                    break;
                case InsertTypesEnum.Bulk:
                    switch (ormEnum)
                    {
                        case OrmEnum.Dapper:
                            result = BulkInsertAdminsBench.DapperBench(connection, admins);
                            break;
                        case OrmEnum.Ef:
                            result = BulkInsertAdminsBench.EFBench(context, admins);
                            break;
                        default:
                            return BadRequest("Unknown ORM type");
                    }
                    break;
                case InsertTypesEnum.TransactionBalk:
                    switch (ormEnum)
                    {
                        case OrmEnum.Dapper:
                            result = TransactionBulkInsertAdminsBench.DapperBench(connection, admins);
                            break;
                        case OrmEnum.Ef:
                            result = TransactionBulkInsertAdminsBench.EFBench(context, admins);
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

        [HttpDelete("ClearTable")]
        public async Task<IActionResult> ClearTable()
        {

            connection.Execute("DELETE FROM admins");

            return Ok();
        }
    }
}
