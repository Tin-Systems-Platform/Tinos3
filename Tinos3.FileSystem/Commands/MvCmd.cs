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
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: mv [-o] <source> <destination>");
                return "";
            }

            bool overwrite = false;
            int sourceIndex = 0;

            if (args[0] == "-o")
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("Usage: mv [-o] <source> <destination>");
                    return "";
                }

                overwrite = true;
                sourceIndex = 1;
            }

            string target = "/mnt/" + args[sourceIndex];
            string dest = "/mnt/" + args[sourceIndex + 1];

            try
            {
                File.Move(target, dest, overwrite);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return "";
        }
    }
    }
