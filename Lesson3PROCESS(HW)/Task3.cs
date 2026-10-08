using Lesson3PROCESS_HW_;
using System.Diagnostics;
using static System.Console;

namespace T3;

internal static class Task3
{
    public static void Run()
    {
        Clear();

        WriteLine("===Task3===");

        const int size = 50_000_000;

        WriteLine("Generating array...");
        int[] arr = Utils.GetArrNumbers(size, 0, 1000);        

        WriteLine("Array generated...");
        WriteLine();

        Write("Enter number of threads: ");

        if (!int.TryParse(ReadLine(), out int threadCount) || threadCount < 1)
        {
            WriteLine("Invalid thread count.");
            return;
        }

        if (threadCount > size) threadCount = size;

        Result[] results = new Result[threadCount];
        Thread[] threads = new Thread[threadCount];

        int chunk = arr.Length / threadCount;

        Stopwatch stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < threadCount; i++)
        {
            int index = i;

            int start = i * chunk;
            int end = (i == threadCount - 1) ? arr.Length : start + chunk;

            threads[i] = new Thread(() => ProcessPart(arr, results, index, start, end));

            threads[i].Name = $"Worker-{i}";

            threads[i].Start();
        }

        for (int i = 0; i < threadCount; i++) threads[i].Join();

        stopwatch.Stop();

        long totalSum = 0;
        long totalEvenCount = 0;
        int totalMin = results[0].Min;
        int totalMax = results[0].Max;

        for (int i = 0; i < results.Length; i++)
        {
            totalSum += results[i].Sum;
            totalEvenCount += results[i].EvenCount;

            if (results[i].Min < totalMin) totalMin = results[i].Min;

            if (results[i].Max > totalMax) totalMax = results[i].Max;
        }

        double average = (double)totalSum / arr.Length;

        WriteLine();
        WriteLine("===RESULT===");
        WriteLine($"Sum: {totalSum}");
        WriteLine($"Min: {totalMin}");
        WriteLine($"Max: {totalMax}");
        WriteLine($"Even numbers: {totalEvenCount}");
        WriteLine($"Average: {average:F2}");
        WriteLine($"Threads: {threadCount}");
        WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");

        Utils.Stoper();
    }

    private static void ProcessPart(
        int[] arr,
        Result[] results,
        int index,
        int start,
        int end)
    {
        long sum = 0;
        int min = arr[start];
        int max = arr[start];
        long evenCount = 0;

        for (int i = start; i < end; i++)
        {
            int number = arr[i];

            sum += number;

            if (number < min) min = number;

            if (number > max) max = number;

            if (number % 2 == 0) evenCount++;
        }

        results[index] = new Result
        {
            Sum = sum,
            Min = min,
            Max = max,
            EvenCount = evenCount
        };
    }
}

internal class Result
{
    public long Sum { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public long EvenCount { get; set; }
}

