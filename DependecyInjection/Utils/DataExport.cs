using System.Text.Json;
using System.Xml.Serialization;
using DependecyInjection.Test;

namespace DependecyInjection.Utils;

#region Dati da esportare ( entita)
public class Data
{
    public string? Name { get; set; }
    public int Quantity { get; set; }

    public decimal Price { get; set; }

}
#endregion
#region Interfaccia IExportFormatter (strategy)
public interface IExportFormatter
{
    string Format(Data data);
}

#endregion

#region Classi Fondamentali
// return json
public class JsonFormater : IExportFormatter
{
    public string Format(Data data)
    {
        JsonSerializerOptions prettyeJson = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(data, prettyeJson );
    }

}

// return xml 
public class XmlFormatter : IExportFormatter
{
    public string Format(Data data)
    {
        using var sw = new StringWriter();
        new XmlSerializer(typeof(Data)).Serialize(sw, data);
        return sw.ToString();
    }
}

#endregion

#region Context DATAEXPORT
public class DataExport
{
    private IExportFormatter _formatter;

    public DataExport(IExportFormatter formatter)
    {
        _formatter = formatter;
    }

    // Cambia a runtime
    public void SetFormatter(IExportFormatter formatter)
    {
        _formatter = formatter;
    }

    public void Export(Data data)
    {
        string result = _formatter.Format(data);
        System.Console.WriteLine("==== RISULTATO ====");
        System.Console.WriteLine(result);

    }




}
#endregion

public class DataExportTest : ITest
{
    public string Name => "Test sulla conversione dei dati da classe a JSON o XML";

    public void Run()
    {
        JsonFormater json = new JsonFormater();
        XmlFormatter xml = new XmlFormatter();
        //List<Data> data;      volevo converitr la lista :'(
        // context iniziale
            var exporter = new DataExport(json);

        while (true)
        {
             var data = new Data
            {
                Name = Input.Read<string>("Inserisci nome"),
                Quantity = Input.Read<int>("Inserisci quanità"),
                Price = Input.Read<decimal>("Insersisci Prezzo"),
            };

              int input = Input.Read<int>(
            "Converti i dati:\n1.JSON\n2.XML\n0.Esci"
            );
            switch (input)
            {
                case 1: exporter.SetFormatter(json); exporter.Export(data); break;
                case 2: exporter.SetFormatter(xml); exporter.Export(data); break;
                case 0: Logger.Write("Uscita dal programma"); return;
                default: throw new InvalidDataException($"Codice {input} non valido");
            }

        }
       
    }
}