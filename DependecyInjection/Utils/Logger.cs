namespace DependecyInjection.Utils;

public interface ILogger
{
    void Log(string message);

}

public class ConsoleLogger : ILogger
{

    public void Log(string message)
    {
        Console.WriteLine($"LOG: {DateTime.Now} {message}");
    }


}

public static class Logger
{
    private static readonly ConsoleLogger consoleLogger = new ConsoleLogger();

    public static void Write(string message)
    {
        consoleLogger.Log(message);
    }

    public static void Error(string message)
    {
        consoleLogger.Log($"[ERROR]: {message}");
    }
}

