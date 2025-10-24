namespace Delegati.Projects;

using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using IadicolaCore.Test;
using IadicolaCore.Utils;
using IadicolaCore.Log;
using IadicolaCore.Vendor.Utils;

#region  ENUM E INTERFACCE 
public enum TypePayment
{
    CARD,
    PAYPAL,
    BANK_TRANSFER
}

public interface IPayment
{
    void Pay(string id, decimal import);
}

public interface IDiscountPolicy
{
    decimal Applicate(decimal import, int percentile);
    decimal Applicate(int import, int percentile);

}

#endregion

#region IMP. PAGAMENTO
public class Payment : IPayment
{


    TypePayment? Type { get; }


    public Payment(TypePayment Type)
    {
        this.Type = Type;
    }

    public void Pay(string id, decimal amount)
    {
        Logger.Log($"[{Type}] Payment Completed: {id}, amount {amount}");
    }


}


#endregion
#region EXCEPTION 
public class ExceptionLog : Exception
{
    private readonly string _message;
    private readonly Exception? _inner;
    private readonly bool _exit;

    // Costruttore principale (usa un'eccezione interna opzionale)
    public ExceptionLog(string message, Exception? inner = null, bool exit = true)
        : base(message, inner)
    {
        _message = message;
        _inner = inner;
        _exit = exit;
        Print();
    }

    //  Costruttore semplificato (solo messaggio e opzione uscita)
    public ExceptionLog(string message, bool exit = false)
        : base(message)
    {
        _message = message;
        _exit = exit;
        _inner = null;
        Print();
    }

    private void Print()
    {
        if (_inner != null)
        {

            Logger.Error($"[ERROR] {_message}");
            Logger.Log($"Type Error: {_inner.GetType().Name}\nMessage: {_inner.Message}\nStackTrace: {_inner.StackTrace}");
        }
        else
        {
            Logger.Warning($"[WARNING] {_message}");
        }

        if (_exit)
        {

            Environment.Exit(1);
        }
    }
}

#endregion
#region discount
public class DiscountPercentile : IDiscountPolicy
{
    public decimal Applicate(decimal import, int percentile)
    {
        if (import <= 0)
        {
            throw new ExceptionLog($"The import is minus than 0. improt: {import}", true);
        }
        if (percentile <= 0 || percentile > 100)
        {
            throw new ExceptionLog($"the Percentile is major 100 or minus than 0. percentle: {percentile}", true);
        }

        decimal sconto = import * percentile / 100m;
        decimal totale = import - sconto;

        Logger.Log($"Discunt applicated: {percentile}%. Total: {totale:C}");
        return totale;
    }

    public decimal Applicate(int import, int percentile)
    {
        if (import <= 0)
        {
            throw new ExceptionLog($"The import is minus than 0. improt: {import}", true);
        }
        if (percentile <= 0 || percentile > 100)
        {
            throw new ExceptionLog($"the Percentile is major 100 or minus than 0. percentle: {percentile}", true);
        }

        decimal sconto = import * percentile / 100m;
        decimal totale = import - sconto;

        Logger.Log($"Discunt applicated: {percentile}%. Total: {totale:C}");
        return totale;
    }
}

public class DiscountMinus : IDiscountPolicy
{
    public decimal Applicate(decimal amount, int minus)
    {
        return amount - minus;
    }
    public decimal Applicate(int amount, int minus)
    {
        return amount - minus;
    }
}
#endregion
#region  FACOTRY 

// FACOTRY PER LA GESTIONE DEGLI SCONTI (BUONI)

public static class FactoryDiscount
{

    public static IDiscountPolicy? Create(int code)
    {
        switch (code)
        {
            case 1: return new DiscountPercentile();
            case 2: return new DiscountMinus();
            default: return null;
        }
    }

}
#endregion

#region service
public class PaymentService
{
    private readonly IPayment? payment; // incapsulament infc 

    private readonly IDiscountPolicy? discount; // incaplusamnt intrfcc

    public delegate void PaymentMethodHandler(string id, decimal totale);

    public PaymentMethodHandler? OnPayCompleted;

    public PaymentService(IPayment payment, IDiscountPolicy? discount = null)
    {
        this.payment = payment; this.discount = discount;
    }

    public void Execute(string id, decimal import, int? percentileDiscount = null)
    {
        decimal total = import;
        // inseriamo lo sconto solo se i due parametri esistono
        if (discount != null && percentileDiscount != null)
        {
            total = discount!.Applicate(import, (int)percentileDiscount!);
        }

        payment!.Pay(id, total);

        if (OnPayCompleted != null)
        {
            OnPayCompleted(id, total);
        }
    }


}

public class PaymentTest : ITest
{
    public string Name => "Pagamenti con Facotry";

    public void Run()
    {
        do
        {
            Console.WriteLine("===Payment Architecture===");

            // prezzo che paga il cliente per il prodotto "x"
            decimal amount = randomDecimal();
            System.Console.WriteLine($"The client want buy your product for {amount}");



            // Permettiamo di inserire il tipo di discount
            int inputDiscountTye = Input.Read<int>(
                "Insert Type of Discount\n" +
                "1. Discount Percentile\n" +
                "2. Discount Minus\n" +
                "any. No Discount for this purchase\n"
            );



            // Prendo il discount class dal factory
            IDiscountPolicy discountPolicy = FactoryDiscount.Create(inputDiscountTye);

            // Prendo il metodo di pagamento
            TypePayment paymentMethod = this.AskForPaymentMethod();

            // Inserisco il metodo di pagamento
            Payment pMethod = new Payment(paymentMethod);

            int? discount = null;
            if (discountPolicy != null)
            {
                discount = Input.Read<int>("Insert discount");

            }
            // instanzio il servizio
            PaymentService paymentService = new PaymentService(pMethod, discountPolicy);

            paymentService.Execute(Str.Random(), amount, discount);

            return;


        } while (true);

    }

    public TypePayment AskForPaymentMethod()
    {
        Console.WriteLine("- Selct cient payment method:");
        TypePayment[] typePaymentenums =
        [
            TypePayment.CARD, TypePayment.PAYPAL, TypePayment.BANK_TRANSFER
        ];

        // mostra a video la lista dei metodi di pagament
        for (int i = 0; i < typePaymentenums.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {typePaymentenums[i]}");
        }

        // Input per selezionare i metodi di pagamento. (oppure se esce dal prg)
        int input = Input.Read<int>("");
        if (input <= 0 || input > typePaymentenums.Length)
            throw new ExceptionLog("Payment Method now allowed", true);

        // prendiamo il metodo di pagamento selezionato
        return typePaymentenums[input - 1];
    }
    private static decimal randomDecimal()
    {
        Random random = new Random();
        decimal min = 5.5M;
        decimal max = 100M;

        decimal randomDecimal = min + (random.NextInt64() * (max - min));
        return randomDecimal;
    }
}
#endregion