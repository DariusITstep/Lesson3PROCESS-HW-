using static System.Console;

namespace LinkWorker;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2 || !int.TryParse(args[0], out int a) || !int.TryParse(args[1], out int b) || a > b)
        {
            Environment.Exit(1);
        }

        else { 

            int count = 0;
            for (int number = a; number <= b; number++)
            {
                if (IsPrime(number)) count++;
            }

            WriteLine(count);
            Environment.Exit(0);
        }
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;

        for (int i = 2; i * i <= number; i++)
        {
            if (number % i == 0) return false;
        }

        return true;
    }
}