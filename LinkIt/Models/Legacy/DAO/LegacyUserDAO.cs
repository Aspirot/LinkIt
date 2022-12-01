using LinkIt.Models.Entities;
using MySql.Data.MySqlClient;

namespace LinkIt.Models.DAO
{
    public class LegacyUserDAO
    {
        private LegacyDbContext context;
        public LegacyUserDAO(IConfiguration configuration)
        {
            context = new LegacyDbContext(configuration);
        }

        public bool CreateUser(User user)
        {
            bool result = false;
            MySqlConnection sqlConnection = new MySqlConnection(context.GetConnectionString());
            try
            {
                sqlConnection.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO User (Username, Email, Password) VALUES (@username, @email, @password)", sqlConnection);
                command.Parameters.Add(new MySqlParameter("@username", user.Username));
                command.Parameters.Add(new MySqlParameter("@email", user.Email));
                command.Parameters.Add(new MySqlParameter("@password", user.Password));
                int rows = command.ExecuteNonQuery();
                if (rows > 0)
                    result = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return result;
        }

        public bool CheckUsernameAndEmail(User user)
        {
            List<User> result = new List<User>();
            MySqlConnection sqlConnection = new MySqlConnection(context.GetConnectionString());
            try
            {
                sqlConnection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM User WHERE Email=@email OR USERNAME=@username", sqlConnection);
                command.Parameters.Add(new MySqlParameter("@email", user.Email));
                command.Parameters.Add(new MySqlParameter("@username", user.Username));
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    User foundUser = new User();
                    foundUser.Id = dataReader.GetInt32("ID");
                    foundUser.Username = dataReader.GetString("Username");
                    foundUser.Email = dataReader.GetString("Email");
                    result.Add(foundUser);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return result.Count == 0;
        }

        public User? FindUser(User user)
        {
            List<User> result = new List<User>();
            MySqlConnection sqlConnection = new MySqlConnection(context.GetConnectionString());
            try
            {
                sqlConnection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM User WHERE Email=@email AND Password=@password", sqlConnection);
                command.Parameters.Add(new MySqlParameter("@email", user.Email));
                command.Parameters.Add(new MySqlParameter("@password", user.Password));
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    User foundUser = new User();
                    foundUser.Id = dataReader.GetInt32("ID");
                    foundUser.Username = dataReader.GetString("Username");
                    foundUser.Email = dataReader.GetString("Email");
                    result.Add(foundUser);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
            return result.FirstOrDefault();
        }
    }
}
