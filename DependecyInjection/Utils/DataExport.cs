namespace DependecyInjection.Utils;

#region Dati da esportare ( entita)
public class Data
{
    public string? Name { get; set; }
    public int Quantity { get; set; }

    public decimal Price { get; set; }

}
#endregion
#region Interfaccia IExportFormatter
public interface IExportFormatter
{
    string Format(Data data);
}

#endregion

#region Classi Fondamentali
public class JsonFormater : IExportFormatter
{
    
}

#endregion