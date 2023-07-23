

using Npgsql;

namespace MyTelegramBot.DBase
{

    internal class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddState(int id, string type, int amount)
        {
            var con = new NpgsqlConnection(_connectionString);
            con.Open();

        }

        public void GetInfo(int id)
        {
            throw new NotImplementedException();
        }

        public void RemovePreviousState(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateState(int id, int newValue)
        {
            throw new NotImplementedException();
        }
    }
}
