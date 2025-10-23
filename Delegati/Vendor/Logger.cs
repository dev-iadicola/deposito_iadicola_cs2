namespace Delegati.Vendor;

delegate void LoggerDel(string message);
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

    public static void Warning(string message) => consoleLogger.Warning(message);

    public static void Success(string message)
    {
        consoleLogger.Success(message);
    }


    public static void Error(string message)
    {
        consoleLogger.Log($"[ERROR]: {message}");
    }


    public static void Log(string message)
    {
        LoggerDel logConsole = LogToConsole; // logga su console 
        LoggerDel logFile = LogToFile; // log su txt
        LoggerDel log = logConsole + logFile; // sommiamo i due delegate (come se fosse un operazione)
        log($"[LOG]: {DateTime.Now} - {message}");
    }
    static void LogToConsole(string message)
    {
        System.Console.WriteLine($"[CONSOLE]: {DateTime.Now} - {message}");
    }

    static void LogToFile(string message)
    {
        string path = "log.log";
        File.AppendAllText(path, $"[LOG]: {DateTime.Now}: {message} {Environment.NewLine}");
        Console.WriteLine($"Messaggio scritto in {path}");
    }

}



