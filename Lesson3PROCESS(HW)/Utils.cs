namespace Lesson3PROCESS_HW_;

internal static class Utils
{
    public static int[] GetArrNumbers(int size, int min, int max)
    {
        Random rnd = new Random();
        int[] arr = new int[size];

        for (int i = 0; i < size; i++)
        {
            arr[i] = rnd.Next(min, max + 1);
        }
        return arr;
    }

    public static char[] GetArrLetters(int size)
    {
        Random rnd = new Random();
        char[] arr = new char[size];

        for (int i = 0; i < size; i++)
        {
            arr[i] = (char)rnd.Next('A', 'Z' + 1);
        }
        return arr;
    }

    public static bool IsInt(string input) => int.TryParse(input, out _);

    public static void Stoper()
    {
        Console.Write("Press any key to continue..."); Console.ReadKey();
    }

    public static bool IsPrime(int n)
    {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0 || n % 3 == 0) return false;

        for (int i = 5; i * i <= n; i += 6)
        {
            if (n % i == 0 || n % (i + 2) == 0) return false;
        }

        return true;
    }       

    public static int[] GetArrPrimes(int[] arr)
    {
        List<int> primes = new List<int>();

        for (int i = 0; i < arr.Length; i++)
        {
            if (IsPrime(arr[i])) primes.Add(arr[i]);
        }

        return primes.ToArray();
    }    
    
    public static bool IsFibonacci(int n)
    {
        if (n < 0) return false;
        int a = 0, b = 1;
        while (b < n)
        {
            int temp = b;
            b = a + b;
            a = temp;
        }
        return b == n || n == 0;
    }

    public static int[] GetArrFibonacci(int[] arr, int n)
    {
        List<int> fibonacciNumbers = new List<int>();

        for (int i = 0; i < arr.Length; i++)
        {
            if (IsFibonacci(arr[i]) && arr[i] <= n)
            {
                fibonacciNumbers.Add(arr[i]);
            }
        }
        return fibonacciNumbers.ToArray();
    }

    public static int SumDigits(int number)
    {
        number = Math.Abs(number);

        int sum = 0;

        while (number > 0)
        {
            sum += number % 10;
            number /= 10;
        }

        return sum;
    }

    public static int GetNumberWithMaxDigitSum(int[] arr)
    {
        int maxNumber = arr[0];
        int maxSum = SumDigits(arr[0]);

        for (int i = 1; i < arr.Length; i++)
        {
            int currentSum = SumDigits(arr[i]);

            if (currentSum > maxSum)
            {
                maxSum = currentSum;
                maxNumber = arr[i];
            }
        }

        return maxNumber;
    }
}
