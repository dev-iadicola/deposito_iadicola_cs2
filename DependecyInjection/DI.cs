
using DependecyInjection.Test;
using DependecyInjection.Utils;
#region INPUT
public class Input
{
    public static T Read<T>(string message)
    {
        while (true)
        {
            Console.WriteLine($"{message}: ");
            Console.Write("> ");

            string? input = Console.ReadLine();

            try
            {
                T value = (T)Convert.ChangeType(input, typeof(T));
                return value;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Errore: inserisci un valore di tipo {typeof(T).Name}");
                Console.ResetColor();
            }
        }
    }
}
#endregion
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
public interface IPaymentGetway
{
    string ToString();
}

public class PayPalGetway : IPaymentGetway
{
    public override string ToString()
    {
        return "paypal";
    }
}

public class StripeGetway : IPaymentGetway
{
    public override string ToString() => "Stripe";
}

public class PaymentProcessor
{
    private readonly IPaymentGetway _getway;

    public PaymentProcessor(IPaymentGetway iPG)
    {
        _getway = iPG;
    }
    public override string ToString()
    {
        return _getway.ToString();
    }
}

public class PaymentTest :ITest
{
    public string Name => "Test del metodo di pagamento";

    public void Run()
    {
        IGreeter gr = new ConsoleGreeter();
        GreetingService grs = new GreetingService(gr);
        grs.Greeting();

        int choose = Input.Read<int>("Choose your payment method (1 = Stripe, 2 = PayPal)");

        IPaymentGetway? ipg = null; 
        switch (choose)
        {
            case 1:
                ipg = new StripeGetway();
                break;

            case 2:
                ipg = new PayPalGetway();
                break;

            default:
                Console.WriteLine("Invalid method");
                break;
        }

        if (ipg != null)
        {
            PaymentProcessor pp = new PaymentProcessor(ipg);
            System.Console.WriteLine(pp);
        }
        else
        {
            Console.WriteLine("Operazione annullata: nessun metodo di pagamento valido scelto.");
        }
    }
}

#endregion

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         
//     }



// }
