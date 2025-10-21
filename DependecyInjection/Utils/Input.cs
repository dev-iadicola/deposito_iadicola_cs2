
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