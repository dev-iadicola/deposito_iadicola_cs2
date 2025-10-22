using System.Data.Common;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using N_tier.Core;


namespace N_tier.Services;
#region Interrfacce 
public interface INotificationService
{
    void Notofy(string message);
}

public class NotifyService : INotificationService
{
    public void Notofy(string message)
    {
        System.Console.WriteLine($"[SERVICE]: {DateTime.Now}| Message: {message}");
    }
}

public interface IService<E>
{
    IEnumerable<E> GetAll();

    void Remove(int id);

}
#endregion

#region BASESERVICE

/**
* La classe astratta BaseService permette diverse cose
1. Risparmiare codice senza dover scrivere sempre <_repo >e <_notofyService> ogni volta che creo un Service
2. Permette di utilizzare metodi generici come <Add> e <Remove> e <GetAll>, che necessitano solo dell'entità e dell'id. 
3. è necessario passare solo l'entità <E> per poter usufruire della repo necesaria
4. Notifica Observer inniettato nel metodo Message: meno codice, più ordine
*/
public abstract class BaseService<E> : IService<E>
{
    // Incapsulamento di interfacce: Irepository (Che permette di poter fare le operazioni come Add(), Remove(), GetAll(), Create() e Update())
    protected readonly IRepository<E>? _repo;

    // Incasulamento di InotificationService. Permette di inserire il metodo di notifica, si aspetta la classe NotifyService 
    protected readonly INotificationService? _notifyService;
    // Un Costruttore, che ci semplifica la vita. Peremtte di instanziare la classe Repo e Notifica in modo più veloce
    public BaseService(IRepository<E> repo, INotificationService notofy)
    {
        _repo = repo; _notifyService = notofy;
    }

    // Incrementale dell'id del nuovo record da creare
    protected int NextId => _repo!.GetAll().Count() + 1;

    // Message: scrittura più pulita con un solo metodo anziché chiamare la proprietà e metodo
    protected void Message(string message)
    {
        _notifyService?.Notofy(message);
    }

   // Ci permette di visualizzare la lista dell'entità 
    public virtual IEnumerable<E> GetAll()
    {
        return _repo!.GetAll();
    }

    // rimuovi record
    public virtual void Remove(int id)
    {
        var exProduct = _repo!.GetById(id);

        _repo?.Remove(id);
    }
}
#endregion
#region PRODUCT SERVICE
class ProductServices : BaseService<Product>
{
    public ProductServices(IRepository<Product> products, INotificationService notofyService) : base(products, notofyService) { }

    public void Create(string name, decimal price)
    {
        Product product = new Product
        {
            Id = NextId,
            Name = name,
            Price = price,
        };
        _repo.Add(product);
        Message($"Product with name: {name} and price: {price} created!");
    }

    public void Update(int id, string name, decimal price)
    {
        Product newProduct = new()
        {
            Id = id,
            Name = name,
            Price = price,
        };
        _repo?.Update(id, newProduct);

        Message($"Product Updated successfully!");
    }
}
#endregion

#region CUSTOMER SERVICE
public class CustomerService : BaseService<Customer>
{

    public CustomerService(IRepository<Customer> repo, INotificationService notify) : base(repo, notify) { }

    // public int Id { get; set; }
    // private string _name = "";
    // public string Name
    public void Create(string name)
    {
        Customer c = new Customer()
        {
            Id = NextId,
            Name = name,
        };
        Message($"Costumer created. {c.ToString()}");
    }

    public void Update(int id, string name)
    {
        Customer c = new Customer()
        {
            Id = id,
            Name = name,
        };

        _repo?.Update(id, c);
        Message($"Customer updated: {c.ToString()}");

    }

}

#endregion

#region ORDER ITEM

public class OrderItemService : BaseService<OrderItem>
{
    public OrderItemService(IRepository<OrderItem> repo, INotificationService notofy) : base(repo, notofy)
    {
    }

    // int Id { get; }
    //     private int _qta = 0;
    //     public Product? Product { get; set; }
    //     public int Quantity
    public void Create(int qta, Product product)
    {
        OrderItem oi = new OrderItem()
        {
            Id = NextId,
            Quantity = qta,
            Product = product,
        };
        _repo?.Add(oi);
        _notifyService?.Notofy($"Order Item created: {oi.ToString()}");
    }

    public void Update(int id, int qta, Product product)
    {
        OrderItem newOi = new OrderItem()
        {
            Id = id,
            Product = product,
            Quantity = qta,
        };

        _repo?.Update(id, newOi);
        Message($"Order Item Updated: {newOi.ToString()}");
    }


}

#endregion
#region ORDERS SERVICE
public class OrderService : BaseService<Order>
{
    public OrderService(IRepository<Order> repo, INotificationService notofyService) : base(repo, notofyService) { }

    // public int Id { get; set; }
    // public Customer? Customer { get; set; }
    // public List<OrderItem> Items { get; } = new();
    // public OrderStatus Status { get; set; } = OrderStatus.New;
    // public decimal Total => Items.Sum(i => i.Total());
    public void Create(Customer? customer, List<OrderItem> listItem, OrderStatus status = OrderStatus.New)
    {

        Order order = new Order()
        {
            Id = NextId,
            Customer = customer ?? throw new NullReferenceException("Costumer not insert"),
            Items = listItem,
            Status = status,
        };
        _repo!.Add(order);
        Message($"Order with name: {customer!.Name} created!");
        Message(order.ToString());
    }


    //TODO: Devo trovare un modo per fare un update più smart. 
    public void Update(int id, Customer customer, List<OrderItem> listItem, OrderStatus status = OrderStatus.Shipped)
    {
        Order order = new Order()
        {
            Id = id,
            Customer = customer,
            Items = listItem,
            Status = status,
        };
        _repo?.Update(id, order);
        Message($"Order with name: {customer.Name} Update!");
        Message(order.ToString());

    }
}
#endregion