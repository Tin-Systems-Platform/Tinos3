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

            switch (args[0])
            {
                case "help":
                    {
                        Console.WriteLine("touch /mnt/path/to/file.txt");
                        break;
                    }
                default:
                    {

                        string fileName = args[0];
                        makeFile(fileName);
                        break;
                    }
            }

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
