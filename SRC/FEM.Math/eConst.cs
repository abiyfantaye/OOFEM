using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM.Mathematics
{
    public class eConst : eFunction
    {
        private double value;

        public eConst(double Value)
        {
            this.value = Value;
        }


        public override double GetValueAt(params double [] coord)
        {
            return value;
        }

        /// <summary>
        /// Gets the value of the function.
        /// </summary>
        public double Value
        {
            get
            {
                return value;
            }
        }
    }
}
