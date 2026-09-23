using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class DirCmd : Command
    {
        public DirCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            listDirectories();
            return "";
        }

        private void listDirectories() {
            string[] files = Directory.GetFiles("/mnt");
            string[] directories = Directory.GetDirectories("/mnt");

            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
            foreach (string directory in directories)
            {
                Console.WriteLine(directory);
            }
        }
    }
}
