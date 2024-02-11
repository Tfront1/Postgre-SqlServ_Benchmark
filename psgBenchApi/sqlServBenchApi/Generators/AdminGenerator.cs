namespace sqlServBenchApi.Generators
{
    public static class AdminGenerator
    {
        private static Random random = new Random();
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static List<Admin> GenerateSqlAdmins(int count)
        {
            List<Admin> admins = new List<Admin>();
            for (int i = 0; i < count; i++)
            {
                Admin admin = new Admin();
                admin.Name = GenerateName();
                admin.Age = random.Next(1, 100);
                admins.Add(admin);
            }
            return admins;
        }

        private static string GenerateName()
        {
            char[] word = new char[5];
            for (int i = 0; i < 5; i++)
            {
                word[i] = alphabet[random.Next(alphabet.Length)];
            }
            return new string(word);
        }
    }
}
