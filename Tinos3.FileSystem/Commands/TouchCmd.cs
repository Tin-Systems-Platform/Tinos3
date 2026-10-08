using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class TouchCmd : Command
    {
        public TouchCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: touch <filename>");
                return "";
            }

            makeFile(args[0]);

            return "";
        }

        private void makeFile(string name)
        {
            try
            {
                using FileStream stream = File.Create("/mnt/" + name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
