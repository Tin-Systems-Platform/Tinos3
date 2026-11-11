using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Graphical.Apps.System.Calc
{
    internal class CalcEngine
    {
        public float Add(float a, float b)
        {
            return a + b;
        }

        public float Subtract(float a, float b)
        {
            return a - b;
        }

        public float Multiply(float a, float b)
        {
            return a * b;
        }

        public float Divide(float a, float b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            return a / b;
        }


    }
}
