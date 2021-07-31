using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM.Mathematics
{
    public class eCos : eTrig
    {

        public eCos(double a, double b, double c)
            : base(a, b, c)
        {

        }

        public override double GetValueAt(params double[] coord)
        {
            return a * System.Math.Cos(b * coord[0] + c * coord[1]);
        }
    }
}
