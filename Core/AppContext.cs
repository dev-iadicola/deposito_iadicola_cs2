
namespace App.Core;
public class AppCtx
{
    private static AppContext? _instance;
    private static readonly object _lock = new();

    private Dictionary<int, User> _users { get; } = new();
    private Dictionary<int, List<ActionLog>> _actionsByUser { get; } = new();

  

    private int _nextId = 1;
    private AppContext() { }
    public static AppContext Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new AppContext();
            }
        }
    }

    private int GetNextId() => _nextId++;

    public AddUser(User user)
    {
        this._user.Add(GetNextId(), user);
    }

    public User? GetUser(string email, string password)
    {   
        if(_users.Sum == 0)
        {
            Log.Error("Utente non trovato");
        }
        return _users.Values.FirstOrDefault(u =>
        u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
        u.password == password
        );
    }
}
