using App.Core;
using App.Utils;

System.Console.WriteLine("=== Benvenuto in MiniDB ===");

int scelta;
do
{
    scelta = Input.Read<int>("\n1. Accedi\n2. Iscriviti\n0. Esci");

    switch (scelta)
    {
        case 1:
            Login();
            break;
        case 2:
            Register();
            break;
        case 0:
            Console.WriteLine("Uscita...");
            break;
        default:
            Console.WriteLine("Scelta non valida.");
            break;
    }

} while (scelta != 0);

// -------------------
// Metodi locali
// -------------------

