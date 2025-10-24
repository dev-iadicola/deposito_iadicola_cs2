using System.Reflection.Metadata.Ecma335;
using Delegati.Projects;
using IadicolaCore.Test;
using IadicolaCore.Utils;


#region DELEGATI
delegate int Operazione(int a, int b);

delegate void Logger(string message);
#endregion

#region CLASSI TEST

// TODO: installa la libreria necessaria per esegiore il programma: dotnet add package IadicolaCore --version 1.0.1
public class Esempio : ITest
{
    delegate void Saluto(string nome);

    string ITest.Name => "Esempio di come usare i delegate";
    static Action<string> saluta = nome => System.Console.WriteLine($"ciao, {nome}");
    static Func<int, int, int> somma = (a, b) => a + b;

    static event Action action;
    public void Run()
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

    }
    static void Ciao(string nome)
    {
        Console.WriteLine($"Ciao, {nome}");
    }
}

class Esercizio1 : ITest
{
    string ITest.Name => "Esercizio 1: somma e calcolo";

    public void Run()
    {
        // esercizio 1
        Operazione somma = Somma;
        Operazione moltiplica = Moltiplica;

        System.Console.WriteLine($"Somma: {EseguiOperazione(4, 2, somma)}");
        System.Console.WriteLine($"Moltiplica: {EseguiOperazione(4, 2, moltiplica)}");

    }
    // ESERCIZIO 1
    static int EseguiOperazione(int x, int y, Operazione op)
    {
        return op(x, y);
    }

    static int Somma(int a, int b) => a - b;
    static int Moltiplica(int a, int b) => a * b;
}

public class Esercizio2 : IadicolaCore.Test.ITest
{
    public string Name => "Esercizio 2: Log su console e file log";

    public void Run()
    {
        // Esercizio 2
        Logger logConsole = LogToConsole; // logga su console 
        Logger logFile = LogToFile; // log su txt

        Logger log = logConsole + logFile; // sommiamo i due delegate (come se fosse un operazione)
        log(Input.Read<string>("Scrivi qualcosa su log. Verrà registrato su console e su file log")); // input dell'utente 


    }

    // ESERCIZIO 2
    static void LogToConsole(string message)
    {
        System.Console.WriteLine($"[CONSOLE]: {DateTime.Now} - {message}");
    }

    static void LogToFile(string message)
    {
        string path = "log.log";
        File.AppendAllText(path, $"[LOG]: {DateTime.Now}: {message} {Environment.NewLine}");
       
    }
}
#endregion

#region  PROGRAM
class Program
{
    static void Main()
    {
        List<ITest> list = new List<ITest>(){
            new PaymentTest(),
            new Esempio(),
            new Esercizio1(),
            new Esercizio2(),
        };

        while (true)
        {
            System.Console.WriteLine("\n===LISTA DEI TEST===");

            for(int i = 0; i < list.Count; i++)
            {
                System.Console.WriteLine($"{i + 1} {list[i].Name}"); // stampo i nomi dei test con l'indice da scegliere
            }

            System.Console.WriteLine("0. Exit");

            int input = Input.Read<int>("Seleziona test.");

            // uscita
            if (input == 0)
            {
                System.Console.WriteLine("Uscita dal programma.");
                return;
            }
            //esecuzione programma
            if (input > 0 && input <= list.Count)
            {
                Console.Clear(); // pulizia console
                System.Console.WriteLine($"=== Esecuzione del test: {list[input - 1].Name}");
                list[input - 1].Run();
            }
            else
            {
                Console.Clear();
                System.Console.WriteLine("Scelta non valida!");
            }
            System.Console.WriteLine("Test svolto. premi un tasto per continuare");
            Console.ReadLine();

        }



    }
}
#endregion