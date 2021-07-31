using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM.Mathematics
{
    public abstract class eTrig : eFunction
    {
        protected double a;
        protected double b;
        protected double c;

        public eTrig(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }
}
