using Lesson3PROCESS_HW_;
using System.Diagnostics;
using static System.Console;

namespace T1;
internal static class Task1
{
    public static void Run()
    {
        while (true)
        {
            Clear();

            WriteLine("===Task1===");
            WriteLine("1. Show all processes");
            WriteLine("2. Show process details");
            WriteLine("3. Start process");
            WriteLine("4. Kill process");
            WriteLine("5. Search process");
            WriteLine("6. Start process and wait");
            WriteLine("0. Exit");
            Write("Choose: ");

            string choice = ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": ShowProcesses(); break;
                    case "2": ShowProcessDetails(); break;
                    case "3": StartProcess(); break;
                    case "4": KillProcess(); break;
                    case "5": SearchProcesses(); break;
                    case "6": StartAndWait(); break;
                    case "0": return;
                    default: WriteLine("Invalid choice."); break;
                }
            }
            catch (Exception ex)
            {
                WriteLine($"Error: {ex.Message}");
            }

            WriteLine();
            Utils.Stoper();
        }
    }

    static void ShowProcesses()
    {
        Process[] processes = Process.GetProcesses();

        var sortedProcesses = processes.OrderByDescending(process =>
        {
            try { return process.WorkingSet64; }
            catch { return 0; }
        });

        WriteLine();
        WriteLine($"{"ID",-8} {"Name",-30} {"Threads",-10} {"Memory (MB)",-15}");
        WriteLine(new string('-', 70));

        foreach (Process process in sortedProcesses)
        {
            try
            {
                double memory = process.WorkingSet64 / 1024.0 / 1024.0;

                WriteLine(
                    $"{process.Id,-8} " +
                    $"{process.ProcessName,-30} " +
                    $"{process.Threads.Count,-10} " +
                    $"{memory:F2,-15}");
            }
            catch (Exception ex)
            {
                WriteLine($"Error accessing process {process.ProcessName}: {ex.Message}");                    
            }
        }
    }

    static void ShowProcessDetails()
    {
        Write("Enter process ID: ");

        if (!int.TryParse(ReadLine(), out int id))
        {
            WriteLine("Invalid ID.");
            return;
        }

        try
        {
            Process process = Process.GetProcessById(id);

            WriteLine();
            WriteLine($"ID: {process.Id}");
            WriteLine($"Name: {process.ProcessName}");
            WriteLine($"Threads: {process.Threads.Count}");

            try
            {
                WriteLine($"Start time: {process.StartTime}");
            }
            catch
            {
                WriteLine("Path: Access denied.");
            }
        }
        catch (ArgumentException)
        {
            WriteLine("Process with this ID does not exist.");
        }
        catch (Exception ex)
        {
            WriteLine($"Cannot access process: {ex.Message}");
        }
    }

    static void StartProcess()
    {
        Write("Enter program name: ");
        string name = ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            WriteLine("Program name cannot be empty.");
            return;
        }

        try
        {
            Process process = Process.Start(name);

            if (process != null) WriteLine($"Process started. ID: {process.Id}");
        }
        catch (Exception ex)
        {
            WriteLine($"Cannot start process: {ex.Message}");
        }
    }

    static void KillProcess()
    {
        Write("Enter process ID: ");

        if (!int.TryParse(ReadLine(), out int id))
        {
            WriteLine("Invalid ID.");
            return;
        }

        try
        {
            Process process = Process.GetProcessById(id);

            WriteLine($"Process: {process.ProcessName}");
            Write("Are you sure? (y/n): ");

            if (ReadLine()?.ToLower() != "y")
            {
                WriteLine("Operation cancelled.");
                return;
            }

            process.Kill();

            WriteLine("Process terminated.");
        }
        catch (ArgumentException)
        {
            WriteLine("Process with this ID does not exist.");
        }
        catch (Exception ex)
        {
            WriteLine($"Cannot terminate process: {ex.Message}");
        }
    }

    static void SearchProcesses()
    {
        Write("Enter part of process name: ");
        string search = ReadLine();

        if (string.IsNullOrWhiteSpace(search))
        {
            WriteLine("Search text cannot be empty.");
            return;
        }

        Process[] processes = Process.GetProcesses();

        foreach (Process process in processes)
        {
            if (process.ProcessName.Contains(search, StringComparison.OrdinalIgnoreCase))
            {
                WriteLine($"ID: {process.Id}, Name: {process.ProcessName}");
            }
        }
    }

    static void StartAndWait()
    {
        Write("Enter program name: ");
        string name = ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            WriteLine("Program name cannot be empty.");
            return;
        }

        try
        {
            Process process = Process.Start(name);

            if (process == null)
            {
                WriteLine("Process was not started.");
                return;
            }

            WriteLine($"Process started. ID: {process.Id}");
            WriteLine("Waiting for process to finish...");

            process.WaitForExit();

            WriteLine($"Process finished. Exit code: {process.ExitCode}");
        }
        catch (Exception ex)
        {
            WriteLine($"Error: {ex.Message}");
        }
    }
}