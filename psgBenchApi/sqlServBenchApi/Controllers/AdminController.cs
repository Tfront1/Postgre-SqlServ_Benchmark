using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using sqlServBenchApi;
using sqlServBenchApi.Data;
using System.Data;
using System.Diagnostics;

namespace psgBenchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost("CreateAdminDapper")]
        public async Task<IActionResult> CreateAdminDapperBench(int count, Admin admin)
        {

            var connection = new SqlConnection("Server=DESKTOP-I51U01F\\SQLEXPRESS;Database=sqlBench;Trusted_Connection=True;TrustServerCertificate=True;");
            connection.Open();


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < count; i++)
            {
                connection.Execute("INSERT INTO admins (name, age) VALUES (@Name, @Age)",admin);
            }

            stopwatch.Stop();

            connection.Close();

            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return Ok(elapsedTime);
        }

        [HttpPost("CreateAdminEf")]
        public async Task<IActionResult> CreateAdminEfBench(int count, Admin admin)
        {
            sqlBenchContext db = new sqlBenchContext();

            Stopwatch stopwatch = new Stopwatch();


            var admins = new List<Admin>();
            for (int i = 0; i < count; i++)
            {
                admins.Add(new Admin { Name = admin.Name, Age = admin.Age });
            }

            stopwatch.Start();

            db.Admins.AddRange(admins);

            db.SaveChangesAsync();

            stopwatch.Stop();

            long elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            return Ok(elapsedTime);
        }

        [HttpDelete("ClearTable")]
        public async Task<IActionResult> ClearTable()
        {
            var connectionString = "Server=DESKTOP-I51U01F\\SQLEXPRESS;Database=sqlBench;Trusted_Connection=True;TrustServerCertificate=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.Execute("DELETE FROM admins");

                connection.Close();
            }

            return Ok();
        }
    }
}
