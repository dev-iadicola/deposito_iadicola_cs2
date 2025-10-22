using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using N_tier.Core;


namespace N_tier.Services;
#region Interrfacce 
public interface INotificationService
{
    void Notofy(string message);
}

public class NotofyService : INotificationService
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

public abstract class BaseService<E> : IService<E>
{
    protected readonly IRepository<E>? _repo;
    protected readonly INotificationService? _notifyService;

    public BaseService(IRepository<E> repo, INotificationService notofy)
    {
        _repo = repo; _notifyService = notofy;
    }

    public virtual IEnumerable<E> GetAll()
    {
        return _repo.GetAll();
    }

    public virtual void Remove(int id)
    {
        _repo.Remove(id);
    }
}

#region PRODUCT SERVICE
class ProductServices : BaseService<Product>
{
    public ProductServices(IRepository<Product> products, INotificationService notofyService):base(products, notofyService){}

    public void Create(string name, decimal price)
    {
        Product product = new Product
        {
            Id = _repo.GetAll().Count() + 1,
            Name = name,
            Price = price,
        };
        _repo.Add(product);
        _notifyService?.Notofy($"Product with name: {name} and price: {price} created!");
    }

    public IEnumerable<Product> GetAll()
    {
        return _repo.GetAll();
    }

    public void Remove(int id)
    {
        var exProduct = _repo?.GetById(id);

        _repo?.Remove(id);
        _notifyService?.Notofy($"Product {exProduct?.Name} woth price {exProduct?.Price} C: delete successfully!");
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

        _notifyService?.Notofy($"Product Updated successfully!");
    }
}
#endregion

#region CUSTOMER SERVICE
public class CustomerService : IService<Customer>
{

    public CustomerService(IRepository<Customer> repo, INotificationService notify)
    {
        
    }
    private readonly IRepository<Customer>? _repo;
    private readonly INotificationService? _notifyService;
    public IEnumerable<Customer> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }
}

#endregion
#region ORDERS SERVICE
public class OrderService : IService<Order>
{
    private readonly IRepository<Order>? _repo;
    public OrderService(IRepository<Order> repo, INotificationService notofyService)
    {
        _repo = repo;
        _notifyService = notofyService;
    }
    private readonly INotificationService? _notifyService;
    // public int Id { get; set; }
    // public Customer? Customer { get; set; }
    // public List<OrderItem> Items { get; } = new();
    // public OrderStatus Status { get; set; } = OrderStatus.New;
    // public decimal Total => Items.Sum(i => i.Total());
    public void Create(Customer customer, List<OrderItem> listItem, OrderStatus status = OrderStatus.New)
    {

        Order order = new Order()
        {
            Id = _repo.GetAll().Count() + 1,
            Customer = customer,
            Items = listItem,
            Status = status,
        };
        _repo.Add(order);
        _notifyService?.Notofy($"Order with name: {customer.Name} created!");
    }


    public IEnumerable<Order> GetAll()
    {
        return _repo.GetAll();
    }

    public void Remove(int id)
    {
        _repo?.Remove(id);
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
    }
}
#endregion