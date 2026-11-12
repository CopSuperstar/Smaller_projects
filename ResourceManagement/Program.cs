using System.Text;

public class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the original file address:");
        string? oldPath = Console.ReadLine()?.Trim('"');

        Console.WriteLine("Enter the filtered file address:");
        string? newPath = Console.ReadLine()?.Trim('"');

        if (string.IsNullOrWhiteSpace(oldPath) || !File.Exists(oldPath))
        {
            Console.WriteLine("Source file doesn't exist.");
            return;
        }

        if (string.IsNullOrWhiteSpace(newPath))
        {
            Console.WriteLine("New file path is empty.");
            return;
        }

        int count = 0;

        using var reader = new StreamReader(oldPath, Encoding.UTF8);
        using var writer = new StreamWriter(newPath, false, Encoding.UTF8);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                writer.WriteLine(line);
                count++;
            }
        }

        Console.WriteLine($"Written lines: {count}");
    }
}
