

public class Input
{
    public static T Read<T>(string message)
    {
        while (true)
        {
            Console.WriteLine($"{message}: ");
            Console.Write($"> ");

            string? input = Console.ReadLine();

            try
            {
                // Usa Convert.ChangeType per i tipi base
                T value = (T)Convert.ChangeType(input, typeof(T));
                return value;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Input non valido. Atteso tipo {typeof(T).Name}.");
                Console.ResetColor();
            }
        }
    }
}

#region Entity: Cliente, Conto, Operazione

public class Cliente
{
    protected string Name { get; set; }
}
public class Conto
{
    protected double Cifra { get; set; }
    protected int IDCliente { get; set; }
}
public enum ActionType { DEPOSITO, PRELIEVO }

public class Operazioni
{
    protected ActionType type { get; set; }
    protected int IDConto { get; set; }
}


#endregion

#region BankContext 
public class BankContext
{
    private static readonly object _lock = new object();
    private static BankContext? _instance;
    private Dictionary<int, Cliente> _clients = new Dictionary<int, Cliente>();
    private Dictionary<int, Conto> _conti = new Dictionary<int, Conto>();
    private Dictionary<int, List<Operazioni>> _operations = new Dictionary<int, List<Operazioni>>();


    private BankContext() { }

    public static BankContext Instance()
    {
        lock (_lock)
        {
            return _instance ??= new BankContext();
        }
    }

}
#endregion
#region  observer logger
public interface IObserver
{
    void Update(string message, ConsoleColor color = ConsoleColor.Yellow);
}

public interface IObservable
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string message, ConsoleColor color = ConsoleColor.Yellow);
}

public sealed class Logger : IObservable
{
    private static readonly Lazy<Logger> _instance = new(() => new Logger());
    public static Logger Instance => _instance.Value;

    private readonly List<IObserver> _observers = new();

    private Logger() { }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string message, ConsoleColor color = ConsoleColor.Yellow)
    {
        foreach (var obs in _observers)
            obs.Update(message, color);
    }

    public void Error(string message)
    {
        Notify(message, ConsoleColor.Red);
    }
}

public class ConsoleLogger : IObserver
{
    public void Update(string message, ConsoleColor color = ConsoleColor.Yellow)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"[LOG {DateTime.Now:HH:mm:ss}] {message}");
        Console.ResetColor();
    }
}
#endregion
#region Program
internal class Program
{
    private static void Main(string[] args)
    {
        // Attacco l'osservatore al logger
        Logger.Instance.Attach(new ConsoleLogger());

        Console.WriteLine("=== Benvenuto in MiniDB ===");

        int scelta;
        do
        {
            scelta = Input.Read<int>("\n1. Accedi\n2. Iscriviti\n0. Esci");

            switch (scelta)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 0:
                    Console.WriteLine("Uscita...");
                    break;
                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }

        } while (scelta != 0);
    }


}
#endregion
