namespace TicketBooking.Infrastructure.Utils;

public static class PathParser
{
    public static string GetPath(string fileName)
    {
        var directPath = Path.Combine(AppContext.BaseDirectory, "csv", fileName);
        var consolePath = Path.Combine(Directory.GetCurrentDirectory(), "csv", fileName);
        var riderPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName ?? "", "TicketBooking", "csv", fileName);

        if (File.Exists(directPath))
            return directPath;
        if (File.Exists(consolePath))
            return consolePath;
        if (File.Exists(riderPath))
            return riderPath;
    
        throw new FileNotFoundException($"CSV file not found in either expected location: {directPath}, {consolePath} nor {riderPath}");
    }
}