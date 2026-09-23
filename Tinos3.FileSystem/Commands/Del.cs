using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class Del : Command
    {
        public Del(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            string path = "/mnt" + args[0];

            try
            {
                File.Delete(path);
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return base.Execute(args);
        }
    }
}
