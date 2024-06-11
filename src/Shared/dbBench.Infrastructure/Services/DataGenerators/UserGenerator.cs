using dbBench.Application.Services.DataGenerators;
using dbBench.Domain.dbo;

namespace dbBench.Infrastructure.Services.DataGenerators;

public class UserGenerator : IUserGenerator
{
    private readonly Random random = new Random();
    private readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";
    public List<User> GenerateUsers(int count)
    {
        List<User> users = new List<User>();
        for (int i = 0; i < count; i++)
        {
            User user = new User();
            user.Name = GenerateName();
            user.Age = random.Next(1, 100);
            users.Add(user);
        }
        return users;
    }

    private string GenerateName()
    {
        char[] word = new char[5];
        for (int i = 0; i < 5; i++)
        {
            word[i] = alphabet[random.Next(alphabet.Length)];
        }
        return new string(word);
    }
}