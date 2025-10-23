// Definizione del delegate
delegate void Saluto(string nome);

class Program
{
    static void Main()
    {
        // Assegno il metodo Ciao al delegate
        Saluto s = Ciao;

        // Invoco il delegate
        s("Mario");
    }

    static void Ciao(string nome)
    {
        Console.WriteLine($"Ciao, {nome}");
    }
}
