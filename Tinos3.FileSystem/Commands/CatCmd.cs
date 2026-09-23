using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class CatCmd : Command
    {
        public CatCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            string fileName = args[0];

            readFile(fileName);
            return base.Execute(args);
        }

        private void readFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    Console.WriteLine(File.ReadAllText("/mnt/" + path));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            } else
            {
                Console.WriteLine("File " + path + "doesn't exist");
            }
        }
    }
}
