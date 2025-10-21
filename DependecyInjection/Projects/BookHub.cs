
#nullable disable
using DependecyInjection.Test;
using DependecyInjection.Utils;

namespace DependecyInjection.BookHub;


#region INTERFACCE
public interface IProduct
{
    string Name { get; }
    decimal Price { get; }
}



public interface IInvetoryService
{
    bool CheckAndReserveStock(IProduct product);
}


public interface IPricingStrategy
{
    decimal ApplyDiscount(decimal price);
}

#endregion

#region CLASSI CONCRETE 
public class BookDigital : IProduct
{
    public string Name => "E-Book";

    public decimal Price => 7M;
}

public class BookPrint : IProduct
{
    public string Name => "Libro Cartaceo";

    public decimal Price => 12m;
}

public class InventoryService : IInvetoryService
{
    public bool CheckAndReserveStock(IProduct product)
    {
        Console.WriteLine($"[Inventory] Riservato stock per {product.Name}");
        return true;
    }
}
public class StandardPricing : IPricingStrategy
{
    public decimal ApplyDiscount(decimal price) => price;
}

public class DiscountPricing : IPricingStrategy
{
    public decimal ApplyDiscount(decimal price) => price * 0.8m;
}
#endregion

#region FACOTRY
public static class Productfacotry
{
    public static IProduct Create(int code)
    {
        switch (code)
        {
            case 1: return new BookDigital();
            case 2: return new BookPrint();
            default: throw new ArgumentException("Codice non trovato");
        }
    }
}

public static class GetwayFactory
{
    public static IPaymentProcessor Create(int code)
    {
        switch (code)
        {
            case 1: return new PayPalGetway();
            case 2: return new StripeGetway();
            default: throw new ArgumentException("Codice non trovato");
        }
    }
}
#endregion

#region  ORDER SERVICE
public class OrderService
{
    private readonly IInvetoryService _inventoryService;
    private IPaymentProcessor _paymentProcessor;

    // Costruttore Injection
    public OrderService(IInvetoryService invetoryService, IPaymentProcessor paymentGetway)
    {
        _inventoryService = invetoryService; _paymentProcessor = paymentGetway;
    }

    //Setter Injection 
    public INotificationSender NotificationSender { get; set; }
    public IPricingStrategy PricingStrategy { get; set; }



    public void PlaceOrder(IProduct product)
    {
        Logger.Write("Avvio ordine");

        if (!_inventoryService.CheckAndReserveStock(product))
        {
            Logger.Write("Stock non disponibile");
            return;
        }
        decimal finalPrice = PricingStrategy.ApplyDiscount(product.Price);
        if (_paymentProcessor.ProcessPayment(product.Price))
        {
            NotificationSender?.Send($"Ordine per {product.Name} completato. Prezzo finale: {finalPrice}€");
            Logger.Write(" Ordine completato con successo!\n");
        }
    }


}
#endregion

#region PROGRAM
public class BookHubTest : ITest
{
    public string Name => "Test di BookHub";

    public void Run()
    {
        System.Console.WriteLine("==== BookHub: Gestionale Ordini Libreria ====");
        int codeProduct = Input.Read<int>("Scegli prodotto:\n1.E-book\n2.Liro Cartaceo");
        IProduct product = Productfacotry.Create(codeProduct);

        int codeGetWay = Input.Read<int>("Scegli metodo di pagamento: \n1.PayPal\n2-Stripe");
        // Costruttor Injecton
        OrderService order = new OrderService(new InventoryService(), GetwayFactory.Create(codeGetWay));

        // SETTER INJECTION
        order.NotificationSender = new EmailSender();
        order.PricingStrategy = new DiscountPricing();

        // Esecuzione ordine
        order.PlaceOrder(product);

        Console.WriteLine("Premi un tasto per uscire...");
        Console.ReadKey();


    }
}
#endregion