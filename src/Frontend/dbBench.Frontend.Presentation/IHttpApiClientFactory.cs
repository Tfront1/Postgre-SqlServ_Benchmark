namespace dbBench.Frontend.Presentation;

public interface IHttpApiClientFactory
{
    Task<SqlServer.Client.SqlServerClient> GetSqlServerHttpClientAsync();
    Task<Postgres.Client.PostgresClient> GetPostgresHttpClientAsync();
}
