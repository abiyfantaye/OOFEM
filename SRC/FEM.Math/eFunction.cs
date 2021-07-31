using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM.Mathematics
{
    public abstract class eFunction
    {
        /// <summary>
        /// The type of space space on which the shape function is defined.
        /// </summary>
        protected eSpaceType dim;

        /// <summary>
        /// Gets the value of the function at spesified point.
        /// </summary>
        /// <param name="coord">Coordinates.</param>
        /// <returns></returns>
        public abstract double GetValueAt(params double [] coord);
        /// <summary>
        /// Gets the derivative of the function at the specific point.
        /// </summary>
        /// <param name="coord">Coordinate of the point.</param>
        /// <returns></returns>
        public virtual double GetDerivative(params double[] coord)
        {
            throw new NotImplementedException();
        }

        public eSpaceType DimType
        {
            get
            {
                return dim;
            }
        }
    }
}
