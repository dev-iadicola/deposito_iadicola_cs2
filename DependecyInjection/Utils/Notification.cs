namespace DependecyInjection.Utils;
public interface INotificationSender
{
    void Send(string message);
}
public class EmailSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"[MAIL]: {message}");
    }
}
public class SmsSender : INotificationSender
{
    public void Send(string message)
    {
        Console.WriteLine($"[SMS] {message}");
    }
}