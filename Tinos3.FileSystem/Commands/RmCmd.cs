using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class RmCmd : Command
    {
        public RmCmd(string name) : base(name)
        {
        }
        public override string Execute(string[] args)
        {
            string path = "/mnt" + args[1];

            

            switch (args[0])
            {

                case "-r":
                    
                    try
                    {
                        Directory.Delete(path, recursive: true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;

                default:
                    try
                    {
                        Directory.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    break;

            }

            return base.Execute(args);
        }
    }
}
