using System.Security.Cryptography.X509Certificates;
using N_tier.Services;
using N_tier.Vendor.Test;

namespace N_tier.Core;

public class AppContext
{
    private static AppContext? _instance;
    // per mt
    private static readonly object _lock = new();

    private INotificationService Notify = new NotifyService();

    private IRepository<Product> _productRepo = new InMemoryRepository<Product>();
    private IRepository<Customer> _customerRepo = new InMemoryRepository<Customer>();
    private IRepository<Order> _orderRepo = new InMemoryRepository<Order>();
    private IRepository<OrderItem> _orderItemRepo = new InMemoryRepository<OrderItem>();

    public ProductServices productService;
    public CustomerService customerService;
    public OrderItemService orderItemService;

    public OrderService orderService;



    private AppContext()
    {

         productService = new ProductServices(_productRepo, Notify);
         customerService = new CustomerService(_customerRepo, Notify);
         orderItemService = new OrderItemService(_orderItemRepo, Notify);
         orderService = new OrderService(_orderRepo, Notify);

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


        // SEZIONE 1: TEST PRODOTTI
        Section("TEST PRODOTTI");

        app.productService.Create("Monitor 27''", 249.99m);
        app.productService.Create("Mouse Logitech", 39.90m);
        app.productService.Create("Tastiera Meccanica", 79.90m);
        ShowList("Tutti i prodotti", app.productService.GetAll());

        app.productService.Update(2, "Mouse Logitech MX Master 3", 99.90m);
        ShowList("Prodotti dopo update", app.productService.GetAll());

        app.productService.Remove(3);
        ShowList("Prodotti dopo rimozione", app.productService.GetAll()); Section("🧾 TEST ORDER ITEM");

        var product1 = app.productService.GetById(1)!;
        var product2 = app.productService.GetById(2)!;

        app.orderItemService.Create(1, product1);
        app.orderItemService.Create(2, product2);
        ShowList("Tutti gli OrderItem", app.orderItemService.GetAll());

        app.orderItemService.Update(2, 3, product2);
        ShowList("OrderItem dopo update quantità", app.orderItemService.GetAll());

        // ---------------------SEZIONE 4: TEST ORDINI

        Section("TEST ORDINI");
        app.customerService.Create("name");
        var customer = app.customerService.GetById(1)!;
        var items = app.orderItemService.GetAll().ToList();

        app.orderService.Create(customer, items);
        ShowList("Tutti gli ordini", app.orderService.GetAll());

        // Test Update Ordine (cambio stato)
        app.orderService.Update(1, customer, items, OrderStatus.Paid);
        ShowList("Ordini dopo cambio stato a 'Paid'", app.orderService.GetAll());

        // Test Stato Shipped
        app.orderService.Update(1, customer, items, OrderStatus.Shipped);
        ShowList("Ordini dopo cambio stato a 'Shipped'", app.orderService.GetAll());

        // ------TEST RIMOZIONE ORDINI
        Section("TEST RIMOZIONE ORDINE");
        app.orderService.Remove(1);
        ShowList("Ordini dopo rimozione", app.orderService.GetAll());


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