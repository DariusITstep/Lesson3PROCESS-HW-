using Lesson3PROCESS_HW_;
using System.Diagnostics;
using static System.Console;

namespace T5;

internal class Task5
{
    public static void Run()
    {
        Clear();

        WriteLine("===Task5===");

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

        Write("Enter number of processes: ");
        if (!int.TryParse(ReadLine(), out int processCount) ||
            processCount < 1)
        {
            WriteLine("Invalid process count.");
            return;
        }

        int rangeSize = max - min + 1;

        if (processCount > rangeSize) processCount = rangeSize;

        Process[] processes = new Process[processCount];

        int chunk = rangeSize / processCount;

        Stopwatch stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < processCount; i++)
        {
            int a = min + i * chunk;

            int b = (i == processCount - 1) ? max : a + chunk - 1;

            ProcessStartInfo psi = new ProcessStartInfo("LinkWorker.exe", $"{a} {b}")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process();
            process.StartInfo = psi;

            int index = i;

            process.EnableRaisingEvents = true;

            process.Exited += (sender, e) =>
            {
                WriteLine($"Worker {index + 1} finished. " + $"Exit code: {process.ExitCode}");
            };

            processes[i] = process;

            WriteLine($"Starting Worker {i + 1}: [{a}; {b}]");
            process.Start();
        }

        long total = 0;

        for (int i = 0; i < processes.Length; i++)
        {
            Process process = processes[i];

            if (!process.WaitForExit(10_000))
            {
                WriteLine($"Worker {i + 1} exceeded 10 seconds. " + $"Terminating.");

                process.Kill();
                process.WaitForExit();

                continue;
            }

            string output = process.StandardOutput.ReadToEnd();

            if (process.ExitCode != 0)
            {
                WriteLine($"Worker {i + 1} finished with error code " + $"{process.ExitCode}.");
                continue;
            }

            if (int.TryParse(output.Trim(), out int result))
            {
                total += result;
            }
        }

        stopwatch.Stop();

        WriteLine();
        WriteLine("===RESULT===");
        WriteLine($"Total prime numbers: {total}");
        WriteLine($"Processes: {processCount}");
        WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");

        WriteLine();
        Utils.Stoper();
    }
}