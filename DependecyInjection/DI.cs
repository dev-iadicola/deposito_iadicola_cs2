
using DependecyInjection.Test;
using DependecyInjection.Utils;

#region Esempio

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

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         
//     }



// }
