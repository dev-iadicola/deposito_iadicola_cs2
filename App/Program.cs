using System;
using System.Collections.Generic;
using System.Linq;
using App.Utils;

#region Context Singleton + Repository
public sealed class AppContext
{
    private static readonly Lazy<AppContext> _instance = new(() => new AppContext());
    public static AppContext Instance => _instance.Value;

    private readonly Dictionary<int, User> _users = new();
    private readonly Dictionary<int, List<ActionLog>> _logs = new();
    private int _nextId = 1;

    private AppContext() { }

    private int GetNextId() => _nextId++;

    public void AddUser(User user)
    {
        user.Id = GetNextId();
        _users[user.Id] = user;
        Logger.Instance.Notify($"Utente registrato: {user.Username}");
    }

    public User? GetUser(string email, string password)
    {
        if (!_users.Any())
        {
            Logger.Instance.Error("Nessun utente registrato.");
            return null;
        }

        var user = _users.Values.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password
        );

        if (user != null)
        {
            AddAction(user.Id, ActionType.LOGIN, "Accesso effettuato");
            Logger.Instance.Notify($"Accesso effettuato da {user.Username}");
        }
        else
        {
            Logger.Instance.Error("Credenziali non valide.");
        }

        return user;
    }

    public void AddAction(int userId, ActionType type, string metadata)
    {
        if (!_logs.ContainsKey(userId))
            _logs[userId] = new List<ActionLog>();

        _logs[userId].Add(new ActionLog
        {
            Type = type,
            Metadata = metadata
        });
    }
}
#endregion

#region Model
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum ActionType { LOGIN, LOGOUT, CREATE, UPDATE, DELETE, VIEW }

public class ActionLog
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public ActionType Type { get; set; }
    public string Metadata { get; set; } = "";
}
#endregion

#region Logger (Observer pattern)
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

    static void Register()
    {
        Console.WriteLine("\n--- Registrazione ---");
        string username = Input.Read<string>("Inserisci username");
        string email = Input.Read<string>("Inserisci email");
        string password = Input.Read<string>("Inserisci password");

        var user = new User
        {
            Username = username,
            Email = email,
            Password = password
        };

        AppContext.Instance.AddUser(user);
    }

    static void Login()
    {
        Console.WriteLine("\n--- Login ---");
        string email = Input.Read<string>("Email");
        string password = Input.Read<string>("Password");

        AppContext.Instance.GetUser(email, password);
    }
}
#endregion
