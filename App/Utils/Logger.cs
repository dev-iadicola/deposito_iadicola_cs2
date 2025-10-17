public interface IObserver { void Update(string message); }

public class Logger 
{
    private static Logger? _log;
    public void Messagge(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss} - {message}");
        Console.ResetColor();
    }

      public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss} - {message}");
        Console.ResetColor();
    }

    private Logger()
    {

    }

    public static Logger Instance => _log = new Logger();
}

 class Log : IObserver
{
    public void Update(string message)
    {
        Logger.Instance.Messagge(message);
    }

    public void Error(string message)
    {
        Logger.Instance.Messagge(message);
    }
}