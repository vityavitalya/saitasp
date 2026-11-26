using BookShelf.Database;
using BookShelf.Models;
using Microsoft.Data.SqlClient;

namespace BookShelf.Repositories;

public class MessageRepository
{
    public void Create(Message message)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Messages (Name, Email, Text) VALUES (@Name, @Email, @Text)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Name", message.Name);
            command.Parameters.AddWithValue("@Email", message.Email);
            command.Parameters.AddWithValue("@Text", message.Text);
            command.ExecuteNonQuery();
        }
    }

    public List<Message> GetAll()
    {
        List<Message> messages = new List<Message>();

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT Id, Name, Email, Text, CreatedAt FROM Messages ORDER BY Id DESC";
            SqlCommand command = new SqlCommand(sql, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Message message = new Message();
                    message.Id = (int)reader["Id"];
                    message.Name = reader["Name"].ToString();
                    message.Email = reader["Email"].ToString();
                    message.Text = reader["Text"].ToString();
                    message.CreatedAt = (DateTime)reader["CreatedAt"];
                    messages.Add(message);
                }
            }
        }

        return messages;
    }
}
