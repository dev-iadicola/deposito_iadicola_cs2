namespace N_tier.Vendor;

public interface ILogger
{
    void Log(string message);

}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG]: {DateTime.Now} {message}");
    }
    public void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[SUCCESS]: {DateTime.Now} {message}");
        Console.ResetColor();

    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Error]: {DateTime.Now} {message}");
        Console.ResetColor();

    }

    public void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING]: {DateTime.Now} {message}");
        Console.ResetColor();

    }
}

public static class Logger
{
    private static readonly ConsoleLogger consoleLogger = new ConsoleLogger();

    public static void Write(string message)
    {
        consoleLogger.Log(message);
    }

    public static void Success(string message)
    {
        consoleLogger.Success(message);
    }


    public static void Error(string message)
    {
        consoleLogger.Log($"[ERROR]: {message}");
    }
}

