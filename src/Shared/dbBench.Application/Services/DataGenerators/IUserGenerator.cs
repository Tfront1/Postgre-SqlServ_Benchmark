using dbBench.Domain.dbo;

namespace dbBench.Application.Services.DataGenerators;

public interface IUserGenerator
{
    List<User> GenerateUsers(int count);
}
