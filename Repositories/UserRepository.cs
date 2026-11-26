using BookShelf.Database;
using BookShelf.Models;
using Microsoft.Data.SqlClient;

namespace BookShelf.Repositories;

public class UserRepository
{
    public User GetByEmail(string email)
    {
        User user = null;

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT Id, Name, Email, PasswordHash, Role, CreatedAt FROM Users WHERE Email = @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = ReadUser(reader);
                }
            }
        }

        return user;
    }

    public User GetById(int id)
    {
        User user = null;

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT Id, Name, Email, PasswordHash, Role, CreatedAt FROM Users WHERE Id = @Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = ReadUser(reader);
                }
            }
        }

        return user;
    }

    public List<User> GetAll()
    {
        List<User> users = new List<User>();

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT Id, Name, Email, PasswordHash, Role, CreatedAt FROM Users ORDER BY Id";
            SqlCommand command = new SqlCommand(sql, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(ReadUser(reader));
                }
            }
        }

        return users;
    }

    public bool EmailExists(string email, int exceptUserId)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Id <> @Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Id", exceptUserId);

            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }

    public int Create(User user)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Users (Name, Email, PasswordHash, Role) " +
                         "VALUES (@Name, @Email, @PasswordHash, @Role); " +
                         "SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@Role", user.Role);

            object result = command.ExecuteScalar();
            int newId = Convert.ToInt32(result);
            return newId;
        }
    }

    public void UpdateProfile(int id, string name, string email)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "UPDATE Users SET Name = @Name, Email = @Email WHERE Id = @Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }

    public void UpdatePassword(int id, string passwordHash)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }

    private User ReadUser(SqlDataReader reader)
    {
        User user = new User();
        user.Id = (int)reader["Id"];
        user.Name = reader["Name"].ToString();
        user.Email = reader["Email"].ToString();
        user.PasswordHash = reader["PasswordHash"].ToString();
        user.Role = reader["Role"].ToString();
        user.CreatedAt = (DateTime)reader["CreatedAt"];
        return user;
    }
}
