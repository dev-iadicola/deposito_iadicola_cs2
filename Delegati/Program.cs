// Definizione del delegate
delegate void Saluto(string nome);

// Esercizio 1
delegate int Operazione(int a, int b); 

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
        System.Console.WriteLine($"Moltiplica: {EseguiOperazione(4,2, moltiplica)}");

    }

    static int EseguiOperazione(int x, int y, Operazione op)
    {
        return op(x, y);
    }

    static int Somma(int a, int b) => a - b;
    static int Moltiplica(int a, int b) => a * b;

    static void Ciao(string nome)
    {
        Console.WriteLine($"Ciao, {nome}");
    }
}
