
namespace Common
{
    public static class AdminGenerator
    {
        private static Random random = new Random();
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public static List<psgBenchApi.Admin> GeneratePsgAdmins(int count) {
            List<psgBenchApi.Admin> admins = new List<psgBenchApi.Admin>();
            for (int i = 0; i < count; i++) {
                psgBenchApi.Admin admin = new psgBenchApi.Admin();
                admin.Name = GenerateName();
                admin.Age = random.Next(1, 100);
                admins.Add(admin);
            }
            return admins;
        }
        public static List<sqlServBenchApi.Admin> GenerateSqlAdmins(int count)
        {
            List<sqlServBenchApi.Admin> admins = new List<sqlServBenchApi.Admin>();
            for (int i = 0; i < count; i++)
            {
                sqlServBenchApi.Admin admin = new sqlServBenchApi.Admin();
                admin.Name = GenerateName();
                admin.Age = random.Next(1, 100);
                admins.Add(admin);
            }
            return admins;
        }

        private static string GenerateName() {
            char[] word = new char[5];
            for (int i = 0; i < 5; i++)
            {
                word[i] = alphabet[random.Next(alphabet.Length)];
            }
            return new string(word);
        }

    }
}
