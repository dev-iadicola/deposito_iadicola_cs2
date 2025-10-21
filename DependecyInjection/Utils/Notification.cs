using System.Runtime.Serialization;
using DependecyInjection.Test;

namespace DependecyInjection.Utils;
#region Iterfaccia
public interface INotificationSender
{
    void Send(string message);
}
public interface INotifier { void Notofy(string message); }

#endregion
#region CLASSI CHE IMPLEMENTANO INotificationSender
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

#endregion
#region classi che imp. Inotifier

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

public class PushNotifier : INotifier
{

    public void Notofy(string message)
    {
        System.Console.WriteLine($"[PUSH] {message}");
    }
}
#endregion
#region ENUM 

public enum NotifyType { Email = 1, Sms = 2, Push = 3 }

#endregion
#region Facotry
public static class NotifierFacotry
{
    public static INotifier Create(NotifyType type)
    {
        return type switch
        {
            NotifyType.Email => new EmailNotifier(),
            NotifyType.Sms => new SmSNotifier(),
            NotifyType.Push => new PushNotifier(),
            _ => throw new ArgumentException($"Type {type} invalid"),
        };
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
#region Messaggio Service
public class MessageService
{
    private readonly INotifier? _notifier;

    public MessageService(INotifier notofier)
    {
        _notifier = notofier;
    }

    public void Send(string message)
    {
        _notifier?.Notofy(message);
    }
}

#endregion
#region TEST
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

public class NotifyWithDiANdEnumTest : ITest
{
    public string Name => "Test di Notifica con DI e ENUM";

    public void Run()
    {
        do
        {
            Console.WriteLine("Sistema di Notifiche con DI & Enum ");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. SMS");
            Console.WriteLine("3. Push");
            Console.WriteLine("0. Exit");
            int input = Input.Read<int>("Seleziona:");

            // seleziona 
            if (!Enum.IsDefined(typeof(NotifyType), input))
            {
                Logger.Error($"Core {input} not found");
            }

            //Prendiamo il tipo ENUM perché la factory creerà la classe in base a questo e non l'int dell'input
            NotifyType selected = (NotifyType)input;

            // utilizzo facroty 
            INotifier notifyClass = NotifierFacotry.Create(selected);

            MessageService service = new MessageService(notifyClass);

            service.Send(Input.Read<string>("Write the message: "));

            switch(Input.Read<int>("1(or any).Send another message\n0.Exit"))
            {
                case 0: return;
            }


        } while (true);
    }
}
#endregion