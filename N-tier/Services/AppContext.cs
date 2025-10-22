using N_tier.Services;
using N_tier.Vendor.Test;

namespace N_tier.Core;

public class AppContext
{
    private static AppContext? _instance;
    // per mt
    private static readonly object _lock = new();

     
    public INotificationService Notify { get; }
    public IRepository<Product> ProductRepo { get; }
    public IRepository<Customer> CustomerRepo { get; }
    public IRepository<Order> OrderRepo { get; }
    public IRepository<OrderItem> OrderItemRepo { get; }
    private AppContext()
    {
        Notify = new NotifyService();

        // Permette di avere la gestione delle entità
        ProductRepo = new InMemoryRepository<Product>();
        CustomerRepo = new InMemoryRepository<Customer>();
        OrderRepo = new InMemoryRepository<Order>();
        OrderItemRepo = new InMemoryRepository<OrderItem>();
    }

    public static AppContext GetInstance()
    {
        lock (_lock)
        {

            return _instance ??= new AppContext();
        }
    }

}

public class AppContextTest : ITest
{
    public string Name => "Test dinamico riguardo lo sviluppo del magazzino";

    public void Run()
    {
        Console.WriteLine("SYSTEM MANAGER ORDINI - TEST COMPLETI");
        var app = AppContext.GetInstance(); //Is.

        // Servizi
        var productService = new ProductServices(app.ProductRepo, app.Notify);
        var customerService = new CustomerService(app.CustomerRepo, app.Notify);
        var orderItemService = new OrderItemService(app.OrderItemRepo, app.Notify);
        var orderService = new OrderService(app.OrderRepo, app.Notify);


        // SEZIONE 1: TEST PRODOTTI
        Section("TEST PRODOTTI");

        productService.Create("Monitor 27''", 249.99m);
        productService.Create("Mouse Logitech", 39.90m);
        productService.Create("Tastiera Meccanica", 79.90m);
        ShowList("Tutti i prodotti", app.ProductRepo.GetAll());

        productService.Update(2, "Mouse Logitech MX Master 3", 99.90m);
        ShowList("Prodotti dopo update", app.ProductRepo.GetAll());

        productService.Remove(3);
        ShowList("Prodotti dopo rimozione", app.ProductRepo.GetAll()); Section("🧾 TEST ORDER ITEM");

        var product1 = app.ProductRepo.GetById(1)!;
        var product2 = app.ProductRepo.GetById(2)!;

        orderItemService.Create(1, product1);
        orderItemService.Create(2, product2);
        ShowList("Tutti gli OrderItem", app.OrderItemRepo.GetAll());

        orderItemService.Update(2, 3, product2);
        ShowList("OrderItem dopo update quantità", app.OrderItemRepo.GetAll());

        // ---------------------SEZIONE 4: TEST ORDINI
       
        Section("TEST ORDINI");
        customerService.Create("name");
        var customer = app.CustomerRepo.GetById(1)!;
        var items = app.OrderItemRepo.GetAll().ToList();

        orderService.Create(customer, items);
        ShowList("Tutti gli ordini", app.OrderRepo.GetAll());

        // Test Update Ordine (cambio stato)
        orderService.Update(1, customer, items, OrderStatus.Paid);
        ShowList("Ordini dopo cambio stato a 'Paid'", app.OrderRepo.GetAll());

        // Test Stato Shipped
        orderService.Update(1, customer, items, OrderStatus.Shipped);
        ShowList("Ordini dopo cambio stato a 'Shipped'", app.OrderRepo.GetAll());

        // ------TEST RIMOZIONE ORDINI
        Section("TEST RIMOZIONE ORDINE");
        orderService.Remove(1);
        ShowList("Ordini dopo rimozione", app.OrderRepo.GetAll());


    }

    static void Section(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n=== {title} ===");
        Console.ResetColor();
    }

    static void ShowList<T>(string title, IEnumerable<T> list)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"\n--- {title} ---");
        Console.ResetColor();

        if (!list.Any())
        {
            Console.WriteLine("(vuoto)");
            return;
        }

        int i = 1;
        foreach (var item in list)
        {
            Console.WriteLine($"{i++}. {item}");
        }
    }
}