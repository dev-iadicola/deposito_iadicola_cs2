using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using N_tier.Vendor;

namespace N_tier.Core;
#region ENTITA
public class Product : IEntity
{
    private string? _name = "";
    private decimal _price = 0m;
    public int Id { get; set; }
    public string Name
    {
        get => _name;
        set
        {

            Notify.Update(_price, value);
            _name = value;
        }
    }
    public decimal Price
    {
        get => _price;
        set
        {
            Notify.Update(_price, value);
            _price = value;
        }
    }
}

public class Customer : IEntity
{
    public int Id { get; set; }
    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            Notify.Update(_name, value);
            _name = value;
        }
    }
}
public class Order
{
    public int Id { get; set; }
    public Customer? Customer { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.New;
    public decimal Total => Items.Sum(i => i.Total());
}
public enum OrderStatus { New, Paid, Shipped, Cancelled }

public class OrderItem : IEntity
{
    private int _qta = 0;
    public Product? Product { get; set; }
    public int Quantity
    {
        get => _qta; set
        {
            Notify.Update(_qta, value);
            _qta = value;

        }
    }
    public decimal Total()
    {
        if (Product != null)
            return Product.Price * Quantity;
        else throw new ArgumentNullException("Product non è settato");
    }
}


#endregion

#region INTERFACCE
public interface IEntity
{

}
public interface IRepository<T>
{

    void Add(T t);
    IEnumerable<T> GetAll();

    void Update(int id, T t);

    T? GetById(int id);

    void Remove(int id);
}



#endregion

#region INFRASTRUTTURA DI MEMORIA 
public class InMemoryRepository<E> : IRepository<E> where E : IEntity // Forza ad essere una entità
{
    protected readonly List<E> _entities = new();
    public void Add(E entity) => _entities.Add(entity);
    public IEnumerable<E> GetAll()
    {
        return _entities;
    }

    
    public E? GetById(int id)
    {
        var prop = typeof(E).GetProperty("Id");

        if (prop == null)
        {
            throw new ArgumentNullException($"[WARN] L'entità {typeof(E).Name} non contiene una proprietà 'Id'.");
        }

        var entity = _entities.FirstOrDefault(e =>
        {
            var value = prop.GetValue(e);
            return value is int intValue && intValue == id;
        });

        if (entity == null)
        {
            Logger.Write($" Nessuna entità trovata con Id = {id} in {typeof(E).Name}.");
        }

        return entity;
    }

    public void Update(int id, E e)
    {
        var prop = typeof(E).GetProperty("Id");
         var entity = _entities.FirstOrDefault(e => (int)prop!.GetValue(e)! == id);
        if (entity == null)
        {
            Logger.Write($"[INFO] Nessuna entità trovata con Id = {id}, impossibile rimuovere.");
            return;
        }
        Remove(id);
        _entities.Add(e);
        Notify.Update($" Entità {typeof(E).Name} con Id: {id} aggiornata con successo.");

    }

    public void Remove(int id)
    {
        var prop = typeof(E).GetProperty("Id");
        var entity = _entities.FirstOrDefault(e => (int)prop!.GetValue(e)! == id);
        if (entity == null)
        {
            Logger.Write($"[INFO] Nessuna entità trovata con Id = {id}, impossibile rimuovere.");
            return;
        }
        _entities.Remove(entity);
       Notify.Update($" Entità {typeof(E).Name} con Id={id} rimossa con successo.");
    }
}

#endregion

#region NOTIFICA + INTERFACCIA
public interface INotificationEntity
{
    static void Entity(string message) { }
}
public class Notify : INotificationEntity
{
    public static void Update(dynamic newValue, dynamic oldValue)
    {
        System.Console.WriteLine($"[UPDATE-ENITTY]: {DateTime.Now}| Message: old value:{oldValue}, new value {newValue} ");
    }

    public static void Update(string message)
    {
        System.Console.WriteLine($"[UPDATE-ENITTY]: {DateTime.Now}| Message: {message} ");
    }
}


#endregion