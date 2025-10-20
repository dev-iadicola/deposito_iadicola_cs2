#region  ENTITY
public class Applicazione
{
    public string? Nome { get; set; }

    private string? _stato;
    public string? Stato
    {
        get => _stato; set
        {
            Messaggi.SetStatoOrdine(value!, this);
            _stato = value;
        }
    }

}

#endregion


#region ORDER SERVICE E MESSAGGI
public class OrderService
{
    private readonly ILogger? _log;

    public OrderService(ILogger log)
    {
        _log = log;
    }

    public void statoOrdine(string statoOrdine, Applicazione app)
    {
        _log?.Log($"l'ordine {app.Nome} ha cambato stato: {statoOrdine}");
    }

}

public class Messaggi
{
    private static ILogger clogger = new ConsoleLogger();

    public static void SetStatoOrdine(string stato, Applicazione app)
    {
        OrderService os = new OrderService(clogger);
        os.statoOrdine(stato, app);
    }
}

#endregion
#region APP CONFIG
public class AppConfig
{
    private static AppConfig? _instance;

    private Dictionary<int, Applicazione> _apps = new Dictionary<int, Applicazione>();

    private string _valuta; private double _iva;

    private int _id = 1;

    private int NextAppId => _id++;




    private AppConfig(string valuta, double IVA)
    {
        _valuta = valuta;
        _iva = IVA;
    }

    public static AppConfig Instance(string valuta, double IVA) => _instance ??= new AppConfig(valuta, IVA);

    public void AddApp(Applicazione app)
    {
        _apps.Add(NextAppId, app);
    }

    public void removeById(int id)
    {
        if (_apps.ContainsKey(id))
            _apps.Remove(id);
        else System.Console.WriteLine($"Non esiste l'app con id {id}");
    }

    public void RemoveById(int id)
    {
        if (_apps.Remove(id))
            Console.WriteLine($"Applicazione con ID {id} rimossa.");
        else
            Console.WriteLine($"Nessuna applicazione trovata con ID {id}.");
    }

    public Applicazione? GetById(int id)
    {
        _apps.TryGetValue(id, out var app);
        return app;
    }

    public void PrintAll()
    {
        Console.WriteLine("\n-- ELENCO APPLICAZIONI --");
        if (_apps.Count == 0)
        {
            Console.WriteLine("Nessuna applicazione registrata.");
            return;
        }

        foreach (var kvp in _apps)
        {
            Console.WriteLine($"ID: {kvp.Key} | Nome: {kvp.Value.Nome} | Stato: {kvp.Value.Stato}");
        }
    }


}
#endregion


public class Program
{
    public static void Main(string[] args)

    {
        var config = AppConfig.Instance("€", 22);

        int scelta;
        do
        {
            Console.WriteLine("\n=== GESTIONE ORDINI ===");

            scelta = Input.Read<int>("1.Crea nuovo ordine" +
            "\n2. Cambia stato ordine" +
            "\n3. Visualizza ordini" +
            "\n4. Rimuovi ordine" +
            "\n0. Esci"
            );

            switch (scelta)
            {
                case 1:
                    Console.Write("Inserisci nome ordine: ");
                    string nome = Console.ReadLine() ?? "Senza nome";
                    var app = new Applicazione { Nome = nome, Stato = "Creato" };
                    config.AddApp(app);
                    break;

                case 2:
                    Console.Write("Inserisci ID ordine: ");
                    if (int.TryParse(Console.ReadLine(), out int idChange))
                    {
                        var ordine = config.GetById(idChange);
                        if (ordine == null)
                        {
                            Console.WriteLine("Ordine non trovato.");
                            break;
                        }

                        Console.Write("Inserisci nuovo stato: ");
                        string nuovoStato = Console.ReadLine() ?? "Sconosciuto";
                        ordine.Stato = nuovoStato;
                    }
                    break;

                case 3:
                    config.PrintAll();
                    break;

                case 4:
                    Console.Write("Inserisci ID da rimuovere: ");
                    if (int.TryParse(Console.ReadLine(), out int idRemove))
                        config.RemoveById(idRemove);
                    break;

                case 0:
                    Console.WriteLine("Uscita...");
                    return;

                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }

        } while (true);

    }



}
