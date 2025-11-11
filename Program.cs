using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

var db = new DataContext();

logger.Info("Program started");

while (true)
{
    Console.WriteLine("\nChoose an option:");
    Console.WriteLine("1. Display all blogs");
    Console.WriteLine("2. Add Blog");
    Console.WriteLine("3. Create Post");
    Console.WriteLine("4. Display Posts");

    Console.Write("Enter option (1-4) or 'q' to Exit ");
    string input = Console.ReadLine();

    if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exiting program...");
        break;
    }

    if (!int.TryParse(input, out int selection) || selection < 1 || selection > 4)
    {
        Console.WriteLine("Error Invalid selection. Please try again.");
        continue;
    }
    switch (selection)
    {
        case 1:
            DisplayAllBlogs();
            break;
        case 2:
            AddBlog();
            break;
        case 3:
            CreatePost();
            break;
        case 4:
            DisplayPosts();
            break;
    }

}
logger.Info("Program ended");

List<string> GetAllBlogNames()
{
    var blogs = db.Blogs.OrderBy(b => b.Name).ToList();
    if (!blogs.Any())
        return new List<string>();
    return blogs.Select(b => b.Name).ToList();
}

void DisplayAllBlogs()
{
    var names = GetAllBlogNames();
    if (!names.Any())
        Console.WriteLine("No blogs found.");
    else
    {
        Console.WriteLine("\nAll blogs in the database:");
        foreach (var n in names)
            Console.WriteLine($"- {n}");
    }
}

bool TryAddBlog(string? nameToAdd)
{
    if (string.IsNullOrWhiteSpace(nameToAdd))
        return false;

    var blogToAdd = new Blog { Name = nameToAdd };
    db.AddBlog(blogToAdd);
    return true;
}

void AddBlog()
{
    Console.Write("Enter a name for the new blog: ");
    string? newName = Console.ReadLine();

    if (TryAddBlog(newName))
        Console.WriteLine($"Blog '{newName}' added successfully.");
    else
        Console.WriteLine("Blog name cannot be empty.");
}

void CreatePost()
{
    var blogs = db.Blogs.OrderBy(b => b.Name).ToList();

    if (!blogs.Any())
    {
        Console.WriteLine("No blogs available.");
        return;
    }

    Console.WriteLine("Select the blog to post to:");
    for (int i = 0; i < blogs.Count; i++)
        Console.WriteLine($"{i + 1}. {blogs[i].Name}");
    string blogInput = Console.ReadLine();
    if (!int.TryParse(blogInput, out int blogChoice) || blogChoice < 1 || blogChoice > blogs.Count)
    {
        Console.WriteLine("Invalid blog ID entered.");
        return;
    }
    var selectedBlog = blogs[blogChoice - 1];

    Console.Write("Enter post title: ");
    var title = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Title cannot be empty.");
        return;
    }
    Console.Write("Enter post content: ");
    var content = Console.ReadLine();

    var post = new Post { Title = title, Content = content, BlogId = selectedBlog.BlogId };
    db.Posts.Add(post);
    db.SaveChanges();

    Console.WriteLine("Post added successfully.");
}

Dictionary<string, List<Post>> GetPostsByBlog(out bool success)
{
    var blogs = db.Blogs.OrderBy(b => b.Name).ToList();

    success = true;
    if (!blogs.Any())
    {
        success = false;
        return new Dictionary<string, List<Post>>();
    }

    var result = new Dictionary<string, List<Post>>();
    foreach (var blog in blogs)
    {
        var posts = db.Posts.Where(p => p.BlogId == blog.BlogId).ToList();
        result[blog.Name] = posts;
    }
    return result;
}