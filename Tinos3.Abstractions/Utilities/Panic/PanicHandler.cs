using Cosmos.Executable.Lua;
using Cosmos.Kernel.System;
using System;
using System.Runtime.CompilerServices;

namespace Tinos3.Abstractions.Utilities.Panic
{
    public static class PanicHandler
    {
        private static bool _handlingException;

        public static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                OnUnhandledException;
        }

        private static void OnUnhandledException(
            object sender,
            UnhandledExceptionEventArgs args)
        {
            string message = args.ExceptionObject is Exception ex
                ? ex.ToString()
                : args.ExceptionObject?.ToString()
                    ?? "Unknown exception";

            ShowPanic(message);
        }

        public static void Panic(string message)
        {
            ShowPanic(message);
        }

        public static void TestPanic(string message)
        {
            throw new Exception(
                $"[TEST PANIC] Intentional test exception: {message}");
        }

        private static void ShowPanic(string message)
        {
            // Prevent recursive panic rendering.
            if (_handlingException)
                Power.Halt();

            _handlingException = true;

            try
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                Console.WriteLine("========================================");
                Console.WriteLine("              TINOS3 PANIC");
                Console.WriteLine("========================================");
                Console.WriteLine();
                Console.WriteLine("An unrecoverable error occurred.");
                Console.WriteLine();
                Console.WriteLine("Error details:");
                Console.WriteLine(message);
                Console.WriteLine();
                Console.WriteLine("The system has halted.");
                WriteLogFile(message);
            }
            catch
            {
                // If rendering fails, try to halt regardless.
            }

            while (true)
                Power.Halt();
        }


        private static bool WriteLogFile(string exceptionMessage)
        {
            try
            {
                DateTime crashTime = DateTime.Now;
                string timestamp = crashTime.ToString("yyyy-MM-dd-HH-mm-ss");
                string logFilePath = "/mnt/crashes/panic-" + timestamp + ".txt";

                string logMainContent =
                    "========================================\n" +
                    "              TINOS3 PANIC\n" +
                    "========================================\n\n" +
                    "Tinos3 version: " +
                    Tinos3.Abstractions.Utilities.Version.GetTinosVersion() + "\n" +
                    "Crash time: " +
                    crashTime.ToString("yyyy-MM-dd HH:mm:ss") + "\n\n" +
                    "Error details:\n" +
                    exceptionMessage + "\n";

                System.IO.Directory.CreateDirectory("/mnt/crashes");
                System.IO.File.WriteAllText(logFilePath, logMainContent);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}