using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM.Mathematics
{
    /// <summary>
    /// Mathematica class which handle mathematical Sin function.
    /// This class represent a fuction f(x,y) = a*sin(b*x+c*y)
    /// </summary>
    public class eSin : eTrig
    {

        public eSin(double a, double b, double c)
            : base(a, b, c)
        {
        }

        public override double GetValueAt(params double [] p)
        {
            return a * System.Math.Sin(b * p[0] + c * p[1]);
        }
    }
}
