namespace App.Utils;
public static class Input
{
    public static T Read<T>(string message)
    {
        while (true)
        {
            Console.Write($"{message}: ");
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
