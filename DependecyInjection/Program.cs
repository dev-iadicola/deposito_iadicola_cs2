#region Esempio
public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"LOG: {message}");
    }
}

public class UserService
{
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;
    }

    public void CreateUser(string name)
    {
        _logger.Log($"User '{name}' created.");
    }
}
#endregion

#region  Igretting Interface


public interface IGreeter
{
    void benvenuto();
}

public class ConsoleGreeter : IGreeter
{
    public void benvenuto()
    {
       System.Console.WriteLine("Welcome to City 17. \nYou have chosen, or been chosen, to relocate to one of our finest remaining urban centers. ");
    }
}
#endregion

#region Greeting service 
public class GreetingService
{
    private readonly IGreeter _cg;
    public GreetingService(IGreeter iGreete)
    {
        _cg = iGreete;
    }
    
    public void Greeting()
    {
        _cg.benvenuto();   
    }
}

#endregion

#region Interfaccia Payment 

#endregion

public class Program
{
    public static void Main(string[] args)
    {
        // IGreeter gr = new ConsoleGreeter();
        // GreetingService grs = new GreetingService(gr);
        // grs.Greeting();

    }


}
