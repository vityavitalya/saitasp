using BookShelf.Database;
using BookShelf.Models;
using BookShelf.ViewModels;
using Microsoft.Data.SqlClient;

namespace BookShelf.Repositories;

public class ItemRepository
{
    public List<Item> GetAll(string search)
    {
        List<Item> items = new List<Item>();

        string sql = "SELECT i.Id, i.Title, i.Author, i.Year, i.Description, i.UserId, i.CreatedAt, u.Name AS UserName " +
                     "FROM Items i JOIN Users u ON i.UserId = u.Id";

        if (!string.IsNullOrEmpty(search))
        {
            sql = sql + " WHERE i.Title LIKE @Search OR i.Author LIKE @Search";
        }

        sql = sql + " ORDER BY i.Id DESC";

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);

            if (!string.IsNullOrEmpty(search))
            {
                command.Parameters.AddWithValue("@Search", "%" + search + "%");
            }

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(ReadItem(reader));
                }
            }
        }

        return items;
    }

    public List<Item> GetLatest(int count)
    {
        List<Item> items = new List<Item>();

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT TOP (@Count) i.Id, i.Title, i.Author, i.Year, i.Description, i.UserId, i.CreatedAt, u.Name AS UserName " +
                         "FROM Items i JOIN Users u ON i.UserId = u.Id " +
                         "ORDER BY i.Id DESC";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Count", count);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(ReadItem(reader));
                }
            }
        }

        return items;
    }

    public Item GetById(int id)
    {
        Item item = null;

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT i.Id, i.Title, i.Author, i.Year, i.Description, i.UserId, i.CreatedAt, u.Name AS UserName " +
                         "FROM Items i JOIN Users u ON i.UserId = u.Id " +
                         "WHERE i.Id = @Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    item = ReadItem(reader);
                }
            }
        }

        return item;
    }

    public List<Item> GetByUser(int userId)
    {
        List<Item> items = new List<Item>();

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT i.Id, i.Title, i.Author, i.Year, i.Description, i.UserId, i.CreatedAt, u.Name AS UserName " +
                         "FROM Items i JOIN Users u ON i.UserId = u.Id " +
                         "WHERE i.UserId = @UserId " +
                         "ORDER BY i.Id DESC";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(ReadItem(reader));
                }
            }
        }

        return items;
    }

    public Item GetByIdAndUser(int id, int userId)
    {
        Item item = null;

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT i.Id, i.Title, i.Author, i.Year, i.Description, i.UserId, i.CreatedAt, u.Name AS UserName " +
                         "FROM Items i JOIN Users u ON i.UserId = u.Id " +
                         "WHERE i.Id = @Id AND i.UserId = @UserId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@UserId", userId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    item = ReadItem(reader);
                }
            }
        }

        return item;
    }

    public int CountByUser(int userId)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Items WHERE UserId = @UserId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            int count = (int)command.ExecuteScalar();
            return count;
        }
    }

    public int CountAll()
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Items";
            SqlCommand command = new SqlCommand(sql, connection);

            int count = (int)command.ExecuteScalar();
            return count;
        }
    }

    public void Create(Item item)
    {
        string description = item.Description;
        if (description == null)
        {
            description = "";
        }

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Items (Title, Author, Year, Description, UserId) " +
                         "VALUES (@Title, @Author, @Year, @Description, @UserId)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Title", item.Title);
            command.Parameters.AddWithValue("@Author", item.Author);
            command.Parameters.AddWithValue("@Year", item.Year);
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@UserId", item.UserId);
            command.ExecuteNonQuery();
        }
    }

    public int Update(Item item, int userId)
    {
        string description = item.Description;
        if (description == null)
        {
            description = "";
        }

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "UPDATE Items SET Title = @Title, Author = @Author, Year = @Year, Description = @Description " +
                         "WHERE Id = @Id AND UserId = @UserId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Title", item.Title);
            command.Parameters.AddWithValue("@Author", item.Author);
            command.Parameters.AddWithValue("@Year", item.Year);
            command.Parameters.AddWithValue("@Description", description);
            command.Parameters.AddWithValue("@Id", item.Id);
            command.Parameters.AddWithValue("@UserId", userId);

            int rows = command.ExecuteNonQuery();
            return rows;
        }
    }

    public int Delete(int id, int userId)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "DELETE FROM Items WHERE Id = @Id AND UserId = @UserId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@UserId", userId);

            int rows = command.ExecuteNonQuery();
            return rows;
        }
    }

    public void DeleteAny(int id)
    {
        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "DELETE FROM Items WHERE Id = @Id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }

    public List<AuthorViewModel> GetAuthors()
    {
        List<AuthorViewModel> authors = new List<AuthorViewModel>();

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();

            string sql = "SELECT Author, COUNT(*) AS BooksCount FROM Items GROUP BY Author ORDER BY Author";
            SqlCommand command = new SqlCommand(sql, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    AuthorViewModel author = new AuthorViewModel();
                    author.Name = reader["Author"].ToString();
                    author.BooksCount = (int)reader["BooksCount"];
                    authors.Add(author);
                }
            }
        }

        return authors;
    }

    private Item ReadItem(SqlDataReader reader)
    {
        Item item = new Item();
        item.Id = (int)reader["Id"];
        item.Title = reader["Title"].ToString();
        item.Author = reader["Author"].ToString();
        item.Year = (int)reader["Year"];
        item.Description = reader["Description"].ToString();
        item.UserId = (int)reader["UserId"];
        item.CreatedAt = (DateTime)reader["CreatedAt"];
        item.UserName = reader["UserName"].ToString();
        return item;
    }
}
