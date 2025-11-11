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


}
logger.Info("Program ended");