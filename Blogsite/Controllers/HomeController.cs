using Blogsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace Blogsite.Controllers
{
    public class HomeController : Controller
    {
        private static string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Blogsite;Integrated Security=true;TrustServerCertificate=yes;";
        BlogsManager manager = new BlogsManager(_connectionString);

        public IActionResult Index()
        {
            IndexViewModel ivm = new IndexViewModel
            {
                Blogs = manager.GetAllBlogs()
            };
            return View(ivm);
        }

        public IActionResult ViewBlog(int id)
        {
            ViewBLogViewModel vb = new ViewBLogViewModel
            {
                Blog = manager.GetBlogByID(id),
                Comments = manager.GetCommentsForBlog(id),
            };
            vb.SplitBody = vb.Blog.Body.Split(vb.Blog.Body);
            vb.CookieResponse = Request.Cookies["commenter-name"];
            return View(vb);
        }

        public IActionResult MostRecent()
        {

            int scopeId = (manager.GetMostRecent()).Id;
            return Redirect($"/Home/ViewBLog?id={scopeId}");
        }

        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            Response.Cookies.Append("commenter-name", comment.Name);
            int scopeId = manager.AddComment(comment);
            return Redirect($"/Home/ViewBLog?id={scopeId}");
        }



    }
}
