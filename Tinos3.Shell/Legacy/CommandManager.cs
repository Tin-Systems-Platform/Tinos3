using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Tinos3.Shell.Legacy
{
    public class CommandManager
    {
        private List<Command> commands;


        public CommandManager()
        {
            this.commands = new List<Command>(15);

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
                if (cmd.name == label)
                {
                    return cmd.Execute(args.ToArray());
                }
            }
            return label + ": Not Found";
        }
    }
}
