namespace Blogsite.Models
{
    public class ViewBLogViewModel
    {
        public Blog Blog { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public string CookieResponse { get; set; }
    }
}
