
using DependecyInjection.Utils;
using DependecyInjection.Test;
public class Program
{
    public static void Main(string[] args)

    {
        List<ITest> tests = new()
            {
                new PrinterTest(),
                new PaymentTest(),

            };

        while (true)
        {
            Console.WriteLine("\n=== Seleziona un test da eseguire ===");
            for (int i = 0; i < tests.Count; i++)
                Console.WriteLine($"{i + 1}. {tests[i].Name}");
            Console.WriteLine("0. Esci");

            Console.Write("Scelta: ");
            if (!int.TryParse(Console.ReadLine(), out int scelta) || scelta < 0 || scelta > tests.Count)
            {
                Console.WriteLine("❌ Scelta non valida.");
                continue;
            }

            if (scelta == 0)
            {
                Console.Clear();
                Console.WriteLine("Uscita...");

                break;
            }


            tests[scelta - 1].Run();
        }
    }




}