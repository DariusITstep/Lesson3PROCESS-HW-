using Lesson3PROCESS_HW_;
using System.Diagnostics;
using static System.Console;

namespace T2;

internal static class Task2
{
    public static void Run()
    {
        Clear();

        WriteLine("===Task2===");
        Write("Enter array size: ");

        if (!int.TryParse(ReadLine(), out int size) || size < 1)
        {
            WriteLine("Invalid size.");
            return;
        }

        Write("Enter minimum value: ");

        if (!int.TryParse(ReadLine(), out int min))
        {
            WriteLine("Invalid minimum value.");
            return;
        }

        Write("Enter maximum value: ");

        if (!int.TryParse(ReadLine(), out int max) || max < min)
        {
            WriteLine("Invalid maximum value.");
            return;
        }

        int[] arr = Utils.GetArrNumbers(size, min, max);

        WriteLine();
        WriteLine("Generated array:");
        WriteLine(string.Join(" ", arr));

        Thread primesThread = new Thread(() => FindPrimes(arr))
        {
            Name = "Primes",
            IsBackground = true
        };

        Thread fibonacciThread = new Thread(() => FindFibonacci(arr, max))
        {
            Name = "Fibonacci",
            IsBackground = true
        };

        Thread digitSumThread = new Thread(() => FindMaxDigitSum(arr))
        {
            Name = "DigitSum",
            IsBackground = true
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        primesThread.Start();
        fibonacciThread.Start();
        digitSumThread.Start();

        primesThread.Join();
        fibonacciThread.Join();
        digitSumThread.Join();

        stopwatch.Stop();

        WriteLine();
        WriteLine($"Total execution time: {stopwatch.ElapsedMilliseconds} ms");

        Utils.Stoper();
    }

    private static void FindPrimes(int[] arr)
    {
        WriteLine($"[{Thread.CurrentThread.Name}] started.");

        int[] primes = Utils.GetArrPrimes(arr);

        WriteLine($"[{Thread.CurrentThread.Name}] primes:");
        WriteLine(string.Join(" ", primes));

        WriteLine($"[{Thread.CurrentThread.Name}] finished.");
    }

    private static void FindFibonacci(int[] arr, int max)
    {
        WriteLine($"[{Thread.CurrentThread.Name}] started.");

        int[] fibonacciNumbers = Utils.GetArrFibonacci(arr, max);

        WriteLine($"[{Thread.CurrentThread.Name}] Fibonacci numbers:");
        WriteLine(string.Join(" ", fibonacciNumbers));

        WriteLine($"[{Thread.CurrentThread.Name}] finished.");
    }

    private static void FindMaxDigitSum(int[] arr)
    {
        WriteLine($"[{Thread.CurrentThread.Name}] started.");

        int number = Utils.GetNumberWithMaxDigitSum(arr);
        int sum = Utils.SumDigits(number);

        WriteLine($"[{Thread.CurrentThread.Name}] " + $"number: {number}, digit sum: {sum}");
        WriteLine($"[{Thread.CurrentThread.Name}] finished.");
    }
}