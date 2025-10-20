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

}
#endregion

#region Greeting service 
public class GreetingService
{
    public void benvenuto();
}

#endregion

public class Program
{
    public static void Main(string[] args)
    {
        ILogger logger = new ConsoleLogger();
        UserService userService = new UserService(logger);
        userService.CreateUser("Alice");

    }


}
