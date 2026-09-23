using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class CopyCmd : Command
    {
        public CopyCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            string target = "/mnt" + args[1];
            string dest = "/mnt" + args[2];

            

            switch(args[0])
            {
                case "-o":
                    try
                    {
                        File.Copy(target, dest, true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;
                default:
                    try
                    {
                        File.Copy(target, dest);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;
            }
            
            return base.Execute(args);
        }
    }
}
