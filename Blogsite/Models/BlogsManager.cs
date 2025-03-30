using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using static System.Net.Mime.MediaTypeNames;

namespace Blogsite.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string[] SplitBody { get; set; }
        public DateTime Date { get; set; }
        public string Preview { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int BlogId { get; set; }
    }

    public class BlogsManager
    {
        private readonly string _connectionString;

        public BlogsManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Blog> GetAllBlogs()
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Blogs";
            connection.Open();
            var reader = command.ExecuteReader();
            List<Blog> blogs = new();
            while (reader.Read())
            {
                Blog b = new Blog
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    Body = (string)reader["Body"],
                    Date = (DateTime)reader["Date"],
                };
                string body = (string)reader["Body"];
                if (body.Length > 200)
                {
                    string bodySub = body.Substring(0, 200);
                    b.Preview = bodySub + "...";
                }
                else
                {
                    b.Preview = body;
                }
                blogs.Add(b);
            }
            return blogs;
        }

        public int AddBlog(Blog blog)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Blogs VALUES (@title, @body, @date) " +
                "SELECT SCOPE_IDENTITY() AS ScopeId";
            command.Parameters.AddWithValue("@title", blog.Title);
            command.Parameters.AddWithValue("@body", blog.Body);
            command.Parameters.AddWithValue("@date", blog.Date);
            connection.Open();
            command.ExecuteNonQuery();
            return (int)(decimal)command.ExecuteScalar();
        }

        public Blog GetBlogByID(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Blogs WHERE ID = @id ";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (new Blog
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    Body = (string)reader["Body"],
                    Date = (DateTime)reader["Date"],
                });
            }
            return null;
        }

        public Blog GetMostRecent()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Blogs ORDER BY DATE DESC";
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (new Blog
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    Body = (string)reader["Body"],
                    Date = (DateTime)reader["Date"],
                });
            }
            return null;
        }

        public string InsertLineBreaks(string text)
        {
            var split = text.Split(new[] { '\n' });
            var addPtag = split.Select(p => $"<p>{p}<p>)");
            return string.Join("", addPtag);
        }

        public int AddComment(Comment comment)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Comments VALUES (@Name, @Content, @BlogId, @Date) " +
                "SELECT @BlogId AS ScopeId";
            command.Parameters.AddWithValue("@Name", comment.Name);
            command.Parameters.AddWithValue("@Content", comment.Content);
            command.Parameters.AddWithValue("@BlogId", comment.BlogId);
            command.Parameters.AddWithValue("@Date", DateTime.Now);
            connection.Open();
            return (int)command.ExecuteScalar();
        }

        public List<Comment> GetCommentsForBlog(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Comments WHERE BlogId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            var comments = new List<Comment>();
            while (reader.Read())
            {
                comments.Add(new Comment
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Content = (string)reader["Comment"],
                    Date = (DateTime)reader["Date"],
                });
            }
            return comments;
        }

        public string[] SplitContent(string text)
        {
            return text.Split('\n');
        }


    }
}
