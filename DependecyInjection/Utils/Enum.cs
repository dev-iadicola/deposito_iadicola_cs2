using DependecyInjection.Test;

namespace DependecyInjection.Utils;

public enum LevelAccess { Ospite, Utente, Amministratore }

public enum TipoTransazione { Acquisto = 1, Rimborso = 2, Trasferimento = 3 }

public class EnumTest : ITest
{

    public string Name => "Test degli ENUM Ruoli";

    public void Run()
    {
        do
        {
            int input = Input.Read<int>(
                  "Scelgi tra L'esercizio" +
                  "\n1. sui privilegi dell'utente \n" +
                  "2. esercizio tipo Transazione (1)\n" +
                  "0. esci (1)\n"
                  );
            switch (input)
            {
                case 1: testRuoli(); break;
                case 2: testTransizioni(); break;
                case 0: return;
                default: Logger.Error($"Codice {input} non valido!\nRiprova"); break;
            }
        } while (true);

    }

    public void testRuoli()
    {
        LevelAccess[] livelli = (LevelAccess[])Enum.GetValues(typeof(LevelAccess));
        foreach (LevelAccess lv in livelli)
        {
            System.Console.WriteLine(lv);
        }
    }

    public void testTransizioni()
    {
        System.Console.WriteLine("1.Acquisto, 1.Rimborso,3.Trasferimento\n0.Esci");
        System.Console.WriteLine("Tipo di Transazione");
        foreach (TipoTransazione tipo in Enum.GetValues(typeof(TipoTransazione)))
        {
            Console.WriteLine($"{(int)tipo}. {tipo}");
        }
        Console.WriteLine("0. Esci");

        int codice = Input.Read<int>("Scegli il tipo di transazione");
        if (codice == 0) return;

        if (!Enum.IsDefined(typeof(TipoTransazione), codice))
        {
            Logger.Error("Codice non valido!");
            return;
        }

        TipoTransazione tipoTransazione = (TipoTransazione)codice;
        decimal importo = Input.Read<decimal>("Inserisci l'importo (€)");

        decimal commissione = CalcolaCommissione(tipoTransazione, importo);

        Console.WriteLine($"\nTipo: {tipoTransazione}");
        Console.WriteLine($"Importo: {importo:C}");
        Console.WriteLine($"Commissione: {commissione:C}");
        Console.WriteLine($"Totale da addebitare: {importo + commissione:C}\n");
    }

    private decimal CalcolaCommissione(TipoTransazione tipo, decimal importo)
    {
        return tipo switch
        {
            TipoTransazione.Acquisto => importo * 0.02m,        // 2%
            TipoTransazione.Rimborso => 1.00m,                 // 1€ fisso
            TipoTransazione.Trasferimento => importo * 0.01m,  // 1%
            _ => 0
        };
    }
}