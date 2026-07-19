using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using Tinos3.Shell.Commands;
using Tinos3.Abstractions.Commands;
using Tinos3.FileSystem.Commands;

namespace Tinos3.Shell.Core
{
    public class CommandManager
    {
        private List<Command> commands;


        public CommandManager()
        {
            this.commands = new List<Command>(15);
            this.commands.Add(new HelpCmd("help"));
            this.commands.Add(new VerCmd("ver"));
            this.commands.Add(new DirCmd("ls"));
            this.commands.Add(new DirCmd("dir"));
            this.commands.Add(new FormatCmd("format"));
            this.commands.Add(new TouchCmd("touch"));
            this.commands.Add(new Nano("nano"));
        }

        public string ProcessInput(string input)
        {
            // "taskkill /F /T IM cmd.exe"

            string[] split = input.Split(' ');

            string label = split[0];

            List<string> args = new List<string>();

            int ctr = 0;

            foreach (string s in split)
            {

                if (ctr != 0)
                    args.Add(s);
                ++ctr;
            }

            foreach (Command cmd in this.commands)
            {
                if (string.IsNullOrEmpty(input))
                    return "";

                if (cmd.name == label)
                {
                    return cmd.Execute(args.ToArray());
                }
            }
            return label + ": Not Found";
        }
    }
}
