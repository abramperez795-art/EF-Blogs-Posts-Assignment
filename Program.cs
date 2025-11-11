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
