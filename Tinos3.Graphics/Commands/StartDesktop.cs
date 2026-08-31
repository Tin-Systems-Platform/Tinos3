using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;
using Tinos3.Graphics.Desktop;

namespace Tinos3.Graphics.Commands
{
    public class StartDesktop : Command
    {
        public StartDesktop(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            InitDesktop desktop = new InitDesktop();

            desktop.ShowDesktop(true);

            return base.Execute(args);
        }
    }
}
