using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;
using Tinos3.Applications.Utilities.Editor;

namespace Tinos3.FileSystem.Commands
{
    public class Nano : Command
    {
        public Nano(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {

            string categoryName = "Utilities";

            string description = "Utilities App Category";

            string fileName = args[0];
            Editor ed = new Editor(categoryName, description);

            ed.Edit(fileName);
            return "";
        }
    }
}
