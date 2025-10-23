// Definizione del delegate
using N_tier.Vendor;

delegate void Saluto(string nome);

// Esercizio 1
delegate int Operazione(int a, int b);

delegate void Logger(string message);

class Program
{
    static Action<string> saluta = nome => System.Console.WriteLine($"ciao, {nome}");
    static Func<int, int, int> somma = (a, b) => a + b;

    static event Action action;
    static void Main()
    {
        // Assegno il metodo Ciao al delegate
        Saluto s = Ciao;

        // Invoco il delegate
        s("Mario");
        saluta("Marco");
        // System.Console.WriteLine(somma(1, 1));
        action += () => Console.WriteLine("Evento eseguito!");

        // Invocazione dell'evento (solo se non è null)
        action?.Invoke();

        // esercizio 1
        Operazione somma = Somma;
        Operazione moltiplica = Moltiplica;

        System.Console.WriteLine($"Somma: {EseguiOperazione(4, 2, somma)}");
        System.Console.WriteLine($"Moltiplica: {EseguiOperazione(4, 2, moltiplica)}");

        // Esercizio 2
        Logger logConsole = LogToConsole; // logga su file 
        Logger logFile = LogToFile; // log su txt

        Logger log = logConsole + logFile;
        log(Input.Read<string>("Scrivi qualcosa su log. Verrà registrato su console e su file log"));

    }
    // ESEMPIO 
  static void Ciao(string nome)
    {
        Console.WriteLine($"Ciao, {nome}");
    }

    // ESERCIZIO 1
    static int EseguiOperazione(int x, int y, Operazione op)
    {
        return op(x, y);
    }

    static int Somma(int a, int b) => a - b;
    static int Moltiplica(int a, int b) => a * b;

  // ESERCIZIO 2
    static void LogToConsole(string message)
    {
        System.Console.WriteLine($"[CONSOLE]: {DateTime.Now} - {message}");
    }

    static void LogToFile(string message)
    {
        string path = "log.txt";
        File.AppendAllText(path, $"[LOG]: {DateTime.Now}: {message} {Environment.NewLine}");
        Console.WriteLine($"Messaggio scritto in {path}");
    }
}
