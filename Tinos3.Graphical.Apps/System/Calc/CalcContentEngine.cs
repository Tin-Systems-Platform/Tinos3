using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Graphical.Apps.System.Calc
{
    internal class CalcContentEngine
    {
        public string AddNumToString(string current, string numToAdd)
        {
            return current + numToAdd;
        }

        public string RemoveLastCharFromString(string current)
        {
            if (string.IsNullOrEmpty(current))
            {
                return current;
            }
            return current.Substring(0, current.Length - 1);
        }

        public string ClearString()
        {
            return string.Empty;
        }

        public string AppendDecimal(string current)
        {
            if (string.IsNullOrEmpty(current))
            {
                return "0.";
            }
            if (!current.Contains("."))
            {
                return current + ".";
            }
            return current;
        }

    }
}
