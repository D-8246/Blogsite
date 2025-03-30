using Blogsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blogsite.Controllers
{
    public class AdminController : Controller
    {
        private static string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Blogsite;Integrated Security=true;TrustServerCertificate=yes;";
        BlogsManager manager = new BlogsManager(_connectionString);
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitPost(string title, string content)
        {
            Blog b = new Blog
            {
                Title = title,
                Body = content,
                Date = DateTime.Now
            };
            int scopeId = manager.AddBlog(b);
            return Redirect($"/Home/ViewBLog?id={scopeId}");
        }
    }
}
