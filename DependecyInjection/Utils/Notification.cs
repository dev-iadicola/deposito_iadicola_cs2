using DependecyInjection.Test;

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

#region INOTIFIE

#endregion
#region Iterfaccia
public interface INotifier { void Notofy(string message); }

public class EmailNotifier : INotifier
{
    public void Notofy(string message)
    {
        Console.WriteLine($"[MAIL] {message}");
    }
}
public class SmSNotifier : INotifier
{
    public void Notofy(string message)
    {
        Console.WriteLine($"[SMS] {message}");
    }
}

#endregion

#region SERVIZI DI NOTIFICA
public class NotificationService
{
    public void Send(string user, INotifier notifier)
    {
        notifier.Notofy($"[NOTIFICA]:Ciao {user}, hai ricevuto una notifica");
    }
}
public class AlertService 
{
    public void Send(string message, INotifier notifier)
    {
        notifier.Notofy($"[ALERT]:{message}");
    }

}


#endregion

public class NotifyMethodInjectionTest : ITest
{
    public string Name => "Test notofy con Method Injection"; 

    public void Run()
    {
          Console.WriteLine("=== Esercizio 1: Method Injection - AlertService ===\n");

        var alertService = new AlertService();
        SmSNotifier sms = new SmSNotifier();

            // Passo l'istanza del notifier direttamente al metodo
            alertService.Send("Sistema in manutenzione!", sms);
            alertService.Send("Nuovo aggiornamento disponibile!", new EmailNotifier());
    }
}