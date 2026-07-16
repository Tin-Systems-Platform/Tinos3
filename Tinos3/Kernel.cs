using System;
using Tinos3.Shell.Legacy;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Shell = Tinos3.Shell.Core.CommandLineShell;
using Sys = Cosmos.Kernel.System;

namespace Tinos3
{
    /// <summary>
    /// Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
    /// </summary>
    public class Kernel : Sys.Kernel
    {

        //private Shell.Core.CommandLineShell shell;
        private CommandManager commandManager;

        protected override void BeforeRun()
        {
            //shell = new Shell.Core.CommandLineShell();
            commandManager = new CommandManager();
            Console.WriteLine("Welcome to Tinos3!");
            Console.WriteLine("Type help to get started");
        }

        protected override void Run()
        {
            //shell.MainCommandLine();

            string response;
            Console.Write(">");

            response = this.commandManager.ProcessInput(Console.ReadLine());



            Console.WriteLine(response);
        }
    }
}
