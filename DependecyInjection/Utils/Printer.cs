using System.Data;
using DependecyInjection.Test;
namespace DependecyInjection.Utils;

#region  
class Printer
{
    public ILogger? iLogger { get; set; }


    public void GeneratePrint(string message)
    {
        if (iLogger == null)
        {
            Console.WriteLine("Non è stato istanziato ConsoleWriter");
            return;
        }
        else iLogger.Log(message);
    }
}

public class PrinterTest : ITest
{
    public string Name => "Test della stampante";

    public void Run()
    {
        Console.WriteLine("Esecuzione del test Printer...");
        var logger = new ConsoleLogger();
        var printer = new Printer();
        printer.iLogger = logger;
        printer.GeneratePrint("Ciao mondo!");
        Console.WriteLine("✅ Test completato con successo.\n");
    }
}

#endregion