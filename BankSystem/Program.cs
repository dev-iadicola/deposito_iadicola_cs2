using System;
using System.Collections.Generic;

#region INPUT
public class Input
{
    public static T Read<T>(string message)
    {
        while (true)
        {
            Console.WriteLine($"{message}: ");
            Console.Write("> ");

            string? input = Console.ReadLine();

            try
            {
                T value = (T)Convert.ChangeType(input, typeof(T));
                return value;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Errore: inserisci un valore di tipo {typeof(T).Name}");
                Console.ResetColor();
            }
        }
    }
}
#endregion

#region OBSERVER
public interface IObserver
{
    void Update(string message);
}

public interface IObservable
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string message);
}

// Classe base da cui possono ereditare le entity osservabili
public abstract class Observable : IObservable
{
    private List<IObserver> _observers = new();

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (var observer in _observers)
        {
            observer.Update(message);
        }
    }
}

public class Logger : IObserver
{
    public void Update(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[LOG - {DateTime.Now:HH:mm:ss}] {message}");
        Console.ResetColor();
    }
}
#endregion

#region ENTITY
public class Cliente : Observable
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";

    public Cliente(string nome)
    {
        Nome = nome;
    }
}

public abstract class Conto : Observable
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public double Saldo { get; protected set; }
    public ICalcoloInteressi? StrategiaInteresse { get; set; }

    public void Deposita(double importo)
    {
        Saldo += importo;
        Notify($"Deposito di {importo}€ effettuato. Saldo attuale: {Saldo}€");
    }

    public void Preleva(double importo)
    {
        if (Saldo < importo)
        {
            Notify($"Prelievo di {importo}€ fallito. Saldo insufficiente: {Saldo}€");
        }
        else
        {
            Saldo -= importo;
            Notify($"Prelievo di {importo}€ effettuato. Saldo attuale: {Saldo}€");
        }
    }

    public void ApplicaInteresse()
    {
        if (StrategiaInteresse != null)
        {
            double interesse = StrategiaInteresse.Calcola(Saldo);
            Saldo += interesse;
            Notify($"Applicato interesse di {interesse:F2}€. Nuovo saldo: {Saldo:F2}€");
        }
        else
        {
            Notify("Nessuna strategia di interesse impostata.");
        }
    }
}
#endregion

#region STRATEGY
public interface ICalcoloInteressi
{
    double Calcola(double saldo);
}

public class InteressiBase : ICalcoloInteressi
{
    public double Calcola(double saldo)
    {
        return saldo * 0.01; // 1%
    }
}

public class InteressiPremium : ICalcoloInteressi
{
    public double Calcola(double saldo)
    {
        return saldo * 0.02; // 2%
    }
}

public class InteressiStudent : ICalcoloInteressi
{
    public double Calcola(double saldo)
    {
        return saldo * 0.005; // 0.5%
    }
}
#endregion

#region FACTORY
public static class ContoFactory
{
    public static Conto CreaConto(int tipo, int idCliente)
    {
        switch (tipo)
        {
            case 1:
                return new ContoBase { IdCliente = idCliente, StrategiaInteresse = new InteressiBase() };
            case 2:
                return new ContoPremium { IdCliente = idCliente, StrategiaInteresse = new InteressiPremium() };
            case 3:
                return new ContoStudent { IdCliente = idCliente, StrategiaInteresse = new InteressiStudent() };
            default:
                throw new ArgumentException("Tipo conto non valido");
        }
    }
}

public class ContoBase : Conto { }
public class ContoPremium : Conto { }
public class ContoStudent : Conto { }
#endregion

#region SINGLETON: BankContext
public sealed class BankContext : IObserver
{
    private static BankContext? _instance;
    private static readonly object _lock = new();

    public static BankContext Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new BankContext();
            }
        }
    }

    private Dictionary<int, Cliente> _clienti = new();
    private Dictionary<int, Conto> _conti = new();

    private int _nextClienteId = 1;
    private int _nextContoId = 1;

    private readonly Logger _logger = new();

    private BankContext() { }

    public void Update(string message)
    {
        _logger.Update(message);
    }

    public int AggiungiCliente(Cliente cliente)
    {
        int id = _nextClienteId++;
        cliente.Id = id;
        cliente.Attach(this);
        _clienti[id] = cliente;
        _logger.Update($"Nuovo cliente aggiunto: {cliente.Nome} (ID {id})");
        return id;
    }

    public int AggiungiConto(Conto conto)
    {
        int id = _nextContoId++;
        conto.Id = id;
        conto.Attach(this);
        _conti[id] = conto;
        _logger.Update($"Nuovo conto creato per cliente {conto.IdCliente} (ID Conto {id})");
        return id;
    }

    public Conto? TrovaConto(int id)
    {
        return _conti.ContainsKey(id) ? _conti[id] : null;
    }
}
#endregion

#region PROGRAM
internal class Program
{
    private static void Main(string[] args)
    {
        var ctx = BankContext.Instance;
        var logger = new Logger();

        Console.WriteLine("=== Mini Sistema Bancario ===");

        while (true)
        {
            Console.WriteLine("\n1. Registra Cliente");
            Console.WriteLine("2. Crea Conto");
            Console.WriteLine("3. Deposita");
            Console.WriteLine("4. Preleva");
            Console.WriteLine("5. Applica interesse");
            Console.WriteLine("0. Esci");

            int scelta = Input.Read<int>("Scegli un'opzione");

            switch (scelta)
            {
                case 1:
                    string nome = Input.Read<string>("Nome cliente");
                    var cliente = new Cliente(nome);
                    ctx.AggiungiCliente(cliente);
                    break;

                case 2:
                    int idCliente = Input.Read<int>("ID cliente");
                    int tipo = Input.Read<int>("Tipo conto (1.base/2.premium/3.student)");
                    var conto = ContoFactory.CreaConto(tipo, idCliente);
                    ctx.AggiungiConto(conto);
                    break;

                case 3:
                    int idDeposito = Input.Read<int>("ID conto");
                    double importoDep = Input.Read<double>("Importo da depositare");
                    ctx.TrovaConto(idDeposito)?.Deposita(importoDep);
                    break;

                case 4:
                    int idPrelievo = Input.Read<int>("ID conto");
                    double importoPrel = Input.Read<double>("Importo da prelevare");
                    ctx.TrovaConto(idPrelievo)?.Preleva(importoPrel);
                    break;

                case 5:
                    int idInt = Input.Read<int>("ID conto");
                    ctx.TrovaConto(idInt)?.ApplicaInteresse();
                    break;

                case 0:
                    Console.WriteLine("Uscita...");
                    return;

                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }
        }
    }
}
#endregion
