using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using N_tier.Vendor;

namespace N_tier.Core;
#region ENTITA
public class Product : IEntity
{
    public int Id { get; set; }

    private string? _name = "";
    private decimal _price = 0m;
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

    public override string ToString()
    {
        return $"Product: {Name}, Price {Price}";
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
    public override string ToString()
    {
        return $"Customer: {Name}";
    }
}
public class Order: IEntity
{
    public int Id { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.New;
    public decimal Total => Items.Sum(i => i.Total());

    public override string ToString()
    {
        return $"Order: ID {Id}, Customer {Customer!.Name}, Status {Status}, Items QTY {Items.Count()}, Total {Total}";
    }
}
public enum OrderStatus { New, Paid, Shipped, Cancelled }

public class OrderItem : IEntity
{
    public int Id { get; set; }
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

    public override string ToString()
    {
        return $"Order Item: ProductName {Product!.Name}, Qty {Quantity}, Total {Total()} ";
    }
}


#endregion

#region INTERFACCE

// Permette di forzare l'entità nelle repository (vedi sotto InMemoryRepository)
public interface IEntity
{
    int Id { get; set; }
    string ToString();
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
    protected void Add(E entity) => _entities.Add(entity);
    protected IEnumerable<E> GetAll()
    {
        return _entities;
    }


    public E? GetById(int id)
    {
        // prendo l'entità
        var prop = typeof(E).GetProperty("Id");
        
        var entity = _entities.FirstOrDefault(e =>
        {
            var value = prop!.GetValue(e);
            return value is int intValue && intValue == id;
        });

        // L'ENTITà NON ESISTE
        if (entity == null)
        {
             throw new ArgumentNullException($"[WARN] Nessuna entità trovata con Id = {id} in {typeof(E).Name}.");
        }

        // ritorna l'entità
        return entity;
    }

    public void Update(int id, E e)
    {
        // vedi se l'entità ha l'id
        var prop = typeof(E).GetProperty("Id");

        // vedi se l'entità esiste, se non esiste, solo log di errore
        var entity = _entities.FirstOrDefault(e => (int)prop!.GetValue(e)! == id);
        if (entity == null)
        {
            Logger.Error($"[INFO] Nessuna entità trovata con Id = {id}, impossibile rimuovere.");
            return;
        }

        // TODO: vedere come aggiornare realmente
        // rimuove
        Remove(id);
        // crea
        _entities.Add(e);

        // Notifica
        Notify.Update($" Entità {typeof(E).Name} con Id: {id} aggiornata con successo.");

    }

    public void Remove(int id)
    {
        // Vedo se l'entità ha l'id
        var prop = typeof(E).GetProperty("Id");
        // Tramite lambda trovo l'esistenza dell'elemento 
        var entity = _entities.FirstOrDefault(e => (int)prop!.GetValue(e)! == id);
        // se non esiste non fa nulla
        if (entity == null)
        {
            Logger.Write($"[INFO] Nessuna entità trovata con Id = {id}, impossibile rimuovere.");
            return;
        }
        // rimuove l'entità
        _entities.Remove(entity);
        // Notifica
        Notify.Update($" Entità {typeof(E).Name} con Id={id} rimossa con successo.");
    }

    void IRepository<E>.Add(E t)
    {
        // Aggiunge
        Add(t);
    }

    IEnumerable<E> IRepository<E>.GetAll()
    {
        // Mostra
        return GetAll();
    }
}

#endregion

//TODO: Probabilmente eliminero utilizzando il NotofiService su ApplicationService (devo vedere)
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