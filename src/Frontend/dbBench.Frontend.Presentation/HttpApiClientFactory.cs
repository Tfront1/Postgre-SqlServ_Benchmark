namespace dbBench.Frontend.Presentation;

public class HttpApiClientFactory : IHttpApiClientFactory
{
    public async Task<SqlServer.Client.SqlServerClient> GetSqlServerHttpClientAsync()
    {
        return new SqlServer.Client.SqlServerClient("https://localhost:7045", new HttpClient());
    }

    public async Task<Postgres.Client.PostgresClient> GetPostgresHttpClientAsync()
    {
        return new Postgres.Client.PostgresClient("https://localhost:7274", new HttpClient());
    }
}
