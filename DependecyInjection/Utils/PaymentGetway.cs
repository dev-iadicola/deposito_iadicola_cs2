namespace DependecyInjection.Utils;

using DependecyInjection.Test;
public interface IPaymentGetway
{

    string ToString();

}

public interface IPaymentProcessor
{
    public bool ProcessPayment(decimal amount);
}
public class PayPalGetway : IPaymentGetway, IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Logger.Write($"Meotdo[{this}]: pagamenti di {amount}");
        return true;
    }

    public override string ToString()
    {
        return "paypal";
    }
}

public class StripeGetway : IPaymentGetway, IPaymentProcessor
{
     public bool ProcessPayment(decimal amount)
    {
        Logger.Write($"Meotdo[{this}]: pagamento di {amount}");
        return true;
    }
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
