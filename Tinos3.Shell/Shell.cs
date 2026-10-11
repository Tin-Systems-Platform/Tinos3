

namespace Tinos3.Shell
{
    public class Shell
    {
        public void ShellMain()
        {
            Console.WriteLine("[SHELL]: Shell started.");
            Console.WriteLine("[SHELL]: Type 'help' for a list of commands.");
            Console.WriteLine("[SHELL]: Type 'exit' to exit the shell.");
            while (true)
            {
                Console.Write("[SHELL]: ");
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    Console.WriteLine("[SHELL]: Exiting shell...");
                    break;
                }
                else if (input == "help")
                {
                    Console.WriteLine("[SHELL]: Available commands:");
                    Console.WriteLine("[SHELL]: help - Show this help message.");
                    Console.WriteLine("[SHELL]: exit - Exit the shell.");
                }
                else
                {
                    Console.WriteLine($"[SHELL]: Unknown command: {input}");
                }
            }

        }
    }
}
