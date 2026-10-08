using System.IO;
using Cosmos.Kernel.System.FileSystem;
using Cosmos.Kernel.System.FileSystem.Fat;
using Cosmos.Kernel.System.Storage;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;
using Tinos3.FileSystem.Tools.Utils;

namespace Tinos3.FileSystem.Commands
{
    public class FormatCmd : Command
    {
        public FormatCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Format format = new Format();

            format.formatPartition();

            return "";
        }
    }
}