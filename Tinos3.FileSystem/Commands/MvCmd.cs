using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class MvCmd : Command
    {
        public MvCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            string target = "/mnt" + args[1];
            string dest = "/mnt" + args[2];



            switch (args[0])
            {
                case "-o":
                    try
                    {
                        File.Move(target, dest, true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    break;
                default:
                    try
                    {
                        File.Move(target, dest);
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
