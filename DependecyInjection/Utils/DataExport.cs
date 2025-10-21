using System.Text.Json;
using System.Xml.Serialization;

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
     JsonSerializerOptions prettyeJson =   new JsonSerializerOptions{ WriteIndented = true}
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

public class DataExportTest
{
    
}