namespace Delegati.Vendor.Utils; 

public class Str
{

    public static string RemoveSpaces(string str)
    {
        string stringWithoutSpace = "";
        foreach (char c in str)
        {
            if (!char.IsWhiteSpace(c))
            {
                stringWithoutSpace += c;
            }
        }
        return stringWithoutSpace;
    }


    public static string Random(string? charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", int length = 20)
    {
        Random random = new Random();
        return new string(Enumerable.Repeat(charSet, length)
        .Select(s => s[random.Next(s.Length)])
        .ToArray());
    }

    public static int CountVocal(string str)
    {
       
        string vocals = "aeiou";
        int count = 0;
        foreach (char c in str)
        {
            char cLower = char.ToLower(c);

            foreach (char vocal in vocals)
            {
                if (c == vocal) count++;
            }
        }

        return count;

    }

    /**
    * Deve contenere:
     Lettera Maiuscola, una cifra, lunghezza 8, 
*/
    public static bool CheckPassword(string password)
    {
        TrimStarAndEnd(password);
        bool isValid = false;

        foreach (char c in password)
        {

        }

        return isValid;
    }

    public static string TrimStarAndEnd(string str)
    {

        string clear = "";
        char[] charArray = str.ToCharArray();

        for (int i = 0; i < str.Length; i++)
        {
            char c = charArray[i];
            if (char.IsWhiteSpace(c))
            {
                clear += c;
            }
            else
            {
                continue;
            }

        }

        char[] charArray2 = clear.ToCharArray();

        for (int i = clear.Length; i <= 0; i--)
        {
            char c = charArray2[i];

            if (char.IsWhiteSpace(c))
            {
                clear += c;
            }
            else
            {
                continue;
            }
        }

        return clear;
    }

    // Ritorna il valore della stringa senza spazi
    public static string Analyze(string str)
    {
        string check = "";

        foreach (char c in str)
        {
            if (!char.IsWhiteSpace(c))
            {
                check += c;
            }
        }

        return check;
    }

    // ritorna il valore del contatore di lettere presenti nella stringa
    public static int Analyze(string str, char contain)
    {
        int count = 0;

        foreach (char c in str)
        {
            if (c == contain)
            {
                count++;
            }
        }
        return count;
    }

    // Ritorna numero vocal o consonanti secondo il booleano
    public static int Analyze(string str, bool conutVocal = false)
    {

        if (conutVocal)
        {
            return CountVocal(str);
        }

        return Analyze(str).Length - CountVocal(str);

    }



}