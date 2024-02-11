using Dapper;
using Infrastructure.Enums.Operations;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using sqlServBenchApi;
using sqlServBenchApi.Data;
using System.Data;
using System.Diagnostics;
using sqlServBenchApi.Generators;
using sqlServBenchApi.Benchmarks.Insert;

namespace psgBenchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly SqlConnection connection;
        private readonly sqlBenchContext context;

        public AdminController()
        {
            connection = new SqlConnection("Server=DESKTOP-I51U01F\\SQLEXPRESS;Database=sqlBench;Trusted_Connection=True;TrustServerCertificate=True;");
            connection.Open();
            context = new sqlBenchContext();
        }

        [HttpPost("InsertAdminBench")]
        public async Task<IActionResult> InsertAdminBench(int count, OrmEnum ormEnum, InsertTypesEnum insertTypesEnum)
        {
            var admins = AdminGenerator.GenerateAdmins(count);
            long result = default;

            switch (insertTypesEnum)
            {
                case InsertTypesEnum.Common:
                    switch (ormEnum)
                    {
                        case OrmEnum.Dapper:
                            result = InsertAdminsBench.DapperBench(connection, admins);
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
