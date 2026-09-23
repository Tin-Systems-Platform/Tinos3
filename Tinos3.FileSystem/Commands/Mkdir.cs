using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class Mkdir : Command
    {
        public Mkdir(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            string dirPath = args[0];

            makeDir(dirPath);

            return base.Execute(args);
        }

        private void makeDir(string path)
        {
            if(Directory.Exists(path))
            {
                Console.WriteLine("Directory already exists");
            } else
            {
                try
                {
                    Directory.CreateDirectory("/mnt/" + path);
                    Console.WriteLine("Directory /mnt/" + path + " was created");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            
        }
    }
}
