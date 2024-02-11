using Dapper;
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
        public async Task<IActionResult> InsertAdminDapperBench(int count)
        {  

            var admins = AdminGenerator.GenerateAdmins(count);

            long result = InsertAdminsBench.DapperBench(connection, admins);

            return Ok(result);
        }

        [HttpPost("InsertAdminEFBench")]
        public async Task<IActionResult> InsertAdminEFBench(int count)
        {

            var admins = AdminGenerator.GenerateAdmins(count);

            long result = InsertAdminsBench.EFBench(context, admins);

            return Ok(result);
        }

        [HttpPost("BulkInsertAdminDapperBench")]
        public async Task<IActionResult> BulkInsertAdminDapperBench(int count)
        {

            var admins = AdminGenerator.GenerateAdmins(count);

            long result = BulkInsertAdminsBench.DapperBench(connection, admins);

            return Ok(result);
        }

        [HttpPost("BulkInsertAdminEFBench")]
        public async Task<IActionResult> BulkInsertAdminEFBench(int count)
        {

            var admins = AdminGenerator.GenerateAdmins(count);

            long result = BulkInsertAdminsBench.EFBench(context, admins);

            return Ok(result);
        }

        [HttpPost("TransactionBulkInsertAdminDapperBench")]
        public async Task<IActionResult> TransactionBulkInsertAdminDapperBench(int count)
        {

            var admins = AdminGenerator.GenerateAdmins(count);

            long result = TransactionBulkInsertAdminsBench.DapperBench(connection, admins);

            return Ok(result);
        }

        [HttpPost("TransactionBulkInsertAdminEFBench")]
        public async Task<IActionResult> TransactionBulkInsertAdminEFBench(int count)
        {

            var admins = AdminGenerator.GenerateAdmins(count);

            long result = TransactionBulkInsertAdminsBench.EFBench(context, admins);

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
