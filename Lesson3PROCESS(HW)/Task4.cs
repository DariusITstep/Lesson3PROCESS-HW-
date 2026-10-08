using Lesson3PROCESS_HW_;
using System.Diagnostics;
using static System.Console;

namespace T4;

internal static class Task4
{
    public static void Run()
    {
        Clear();

        WriteLine("===Task4===");
        Write("Enter number of participants (3-8): ");

        if (!int.TryParse(ReadLine(), out int participantCount) || participantCount < 3 || participantCount > 8)
        {
            WriteLine("Invalid number of participants.");
            return;
        }

        Write("Enter distance: ");

        if (!int.TryParse(ReadLine(), out int distance) || distance < 1)
        {
            WriteLine("Invalid distance.");
            return;
        }

        Thread[] threads = new Thread[participantCount];
        long[] finishTimes = new long[participantCount];

        Stopwatch stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < participantCount; i++)
        {
            int index = i;

            threads[i] = new Thread(() => RunParticipant(index, distance, finishTimes, stopwatch));

            threads[i].Name = $"Horse {i + 1}";
            threads[i].Start();
        }

        while (threads.Any(thread => thread.IsAlive))
        {
            WriteLine();
            WriteLine("---THREAD STATES---");

            foreach (Thread thread in threads)
            {
                WriteLine(
                    $"{thread.Name,-10} " +
                    $"State: {thread.ThreadState,-15} " +
                    $"Alive: {thread.IsAlive}");
            }

            Thread.Sleep(500);
        }

        for (int i = 0; i < threads.Length; i++) threads[i].Join();

        stopwatch.Stop();

        WriteLine();
        WriteLine("===RESULTS===");

        int[] places = Enumerable.Range(0, participantCount)
            .OrderBy(i => finishTimes[i])
            .ToArray();

        for (int place = 0; place < places.Length; place++)
        {
            int index = places[place];

            WriteLine(
                $"{place + 1}. {threads[index].Name,-10} " +
                $"{finishTimes[index]} ms");
        }

        Utils.Stoper();
    }

    private static void RunParticipant(
        int index,
        int distance,
        long[] finishTimes,
        Stopwatch stopwatch)
    {
        Random random = new Random(Environment.TickCount + index);

        for (int step = 1; step <= distance; step++)
        {
            WriteLine($"{Thread.CurrentThread.Name}: " + $"{step}/{distance}");

            Thread.Sleep(random.Next(50, 301));
        }

        finishTimes[index] = stopwatch.ElapsedMilliseconds;
    }
}