using Cosmos.Kernel.System.Graphics;
using System;
using Tinos3.FileSystem;
using Tinos3.Graphics;
using Tinos3.Shell.Core;
using Sys = Cosmos.Kernel.System;

namespace Tinos3
{
    /// <summary>
    /// Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
    /// </summary>
    public class Kernel : Sys.Kernel
    {
        private Canvas canvas;
        //private Shell.Core.CommandLineShell shell;
        private CommandManager commandManager;

        protected override void BeforeRun()
        {

            Console.WriteLine("GRAPHICS: Initializing resolution setting");

            DisplayManager displayManager = new DisplayManager();

            displayManager.InitializeStaticResolution();

            //shell = new Shell.Core.CommandLineShell();
            commandManager = new CommandManager();

            Console.WriteLine("KERNEL: Loading system modules");

            FSMain.initFS();

            GraphicsInit graphicsInit = new GraphicsInit();

            graphicsInit.initGraphics();

            Console.WriteLine("KERNEL: Loaded system modules");


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
