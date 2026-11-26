using BookShelf.Helpers;
using BookShelf.Models;
using BookShelf.Repositories;
using Microsoft.Data.SqlClient;

namespace BookShelf.Database;

public static class DbInitializer
{
    public static void Initialize()
    {
        CreateDatabase();
        CreateTables();
        AddTestData();
    }

    private static void CreateDatabase()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Db.ConnectionString);
        string dbName = builder.InitialCatalog;

        builder.InitialCatalog = "master";

        using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
        {
            connection.Open();

            string sql = "IF DB_ID('" + dbName + "') IS NULL CREATE DATABASE [" + dbName + "]";
            SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }

    private static void CreateTables()
    {
        string sql = @"
            IF OBJECT_ID('Users') IS NULL
            CREATE TABLE Users (
                Id           INT IDENTITY(1,1) PRIMARY KEY,
                Name         NVARCHAR(50)  NOT NULL,
                Email        NVARCHAR(100) NOT NULL UNIQUE,
                PasswordHash NVARCHAR(100) NOT NULL,
                Role         NVARCHAR(20)  NOT NULL DEFAULT 'User',
                CreatedAt    DATETIME      NOT NULL DEFAULT GETDATE()
            );

            IF OBJECT_ID('Items') IS NULL
            CREATE TABLE Items (
                Id          INT IDENTITY(1,1) PRIMARY KEY,
                Title       NVARCHAR(200)  NOT NULL,
                Author      NVARCHAR(100)  NOT NULL,
                Year        INT            NOT NULL,
                Description NVARCHAR(2000) NULL,
                UserId      INT            NOT NULL FOREIGN KEY REFERENCES Users(Id),
                CreatedAt   DATETIME       NOT NULL DEFAULT GETDATE()
            );

            IF OBJECT_ID('Messages') IS NULL
            CREATE TABLE Messages (
                Id        INT IDENTITY(1,1) PRIMARY KEY,
                Name      NVARCHAR(50)   NOT NULL,
                Email     NVARCHAR(100)  NOT NULL,
                Text      NVARCHAR(2000) NOT NULL,
                CreatedAt DATETIME       NOT NULL DEFAULT GETDATE()
            );
        ";

        using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }

    private static void AddTestData()
    {
        UserRepository userRepository = new UserRepository();

        if (userRepository.GetByEmail("admin@mail.ru") != null)
        {
            return;
        }

        User admin = new User();
        admin.Name = "Администратор";
        admin.Email = "admin@mail.ru";
        admin.PasswordHash = PasswordHelper.HashPassword("admin123");
        admin.Role = "Admin";
        int adminId = userRepository.Create(admin);

        User ivan = new User();
        ivan.Name = "Иван";
        ivan.Email = "user@mail.ru";
        ivan.PasswordHash = PasswordHelper.HashPassword("user123");
        ivan.Role = "User";
        int ivanId = userRepository.Create(ivan);

        ItemRepository itemRepository = new ItemRepository();

        itemRepository.Create(new Item
        {
            Title = "Мастер и Маргарита",
            Author = "Михаил Булгаков",
            Year = 1967,
            Description = "Роман о том, как в Москву 1930-х приезжает дьявол со своей свитой, и о любви Мастера и Маргариты.",
            UserId = ivanId
        });

        itemRepository.Create(new Item
        {
            Title = "Преступление и наказание",
            Author = "Фёдор Достоевский",
            Year = 1866,
            Description = "Бедный студент Раскольников решается на преступление, а потом мучается от того, что сделал.",
            UserId = ivanId
        });

        itemRepository.Create(new Item
        {
            Title = "Война и мир",
            Author = "Лев Толстой",
            Year = 1869,
            Description = "Огромный роман о войне 1812 года и о жизни нескольких дворянских семей.",
            UserId = adminId
        });

        itemRepository.Create(new Item
        {
            Title = "Пикник на обочине",
            Author = "Аркадий и Борис Стругацкие",
            Year = 1972,
            Description = "Фантастика про Зону, которую оставили пришельцы, и сталкеров, которые ходят туда за находками.",
            UserId = ivanId
        });

        itemRepository.Create(new Item
        {
            Title = "1984",
            Author = "Джордж Оруэлл",
            Year = 1949,
            Description = "Антиутопия о государстве, которое следит за каждым человеком и решает, что ему думать.",
            UserId = adminId
        });

        itemRepository.Create(new Item
        {
            Title = "Маленький принц",
            Author = "Антуан де Сент-Экзюпери",
            Year = 1943,
            Description = "Короткая сказка для взрослых о мальчике с маленькой планеты, о дружбе и ответственности.",
            UserId = ivanId
        });

        itemRepository.Create(new Item
        {
            Title = "Три товарища",
            Author = "Эрих Мария Ремарк",
            Year = 1936,
            Description = "История трёх друзей в Германии после Первой мировой войны: дружба, любовь и автомастерская.",
            UserId = adminId
        });
    }
}
