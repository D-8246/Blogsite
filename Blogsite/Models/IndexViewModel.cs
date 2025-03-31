namespace Blogsite.Models
{
    public class IndexViewModel
    {
        public List<Blog> Blogs { get; set; } = new();
        public int Page { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }
    }
}
