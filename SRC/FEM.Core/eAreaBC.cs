using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FEM.Mathematics;

namespace FEM
{
    /// <summary>
    /// Represents boundary conditions which are applied on area.
    /// </summary>
    public class eAreaBC : eBC
    {
        /// <summary>
        /// Creats new intance of Areal boundary condition.
        /// </summary>
        /// <param name="Points">Points which define the area.</param>
        /// <param name="DOFIndex">Index of DOF on which the boundary condition is applied.</param>
        /// <param name="BCType">Type of the boundary condition.</param>
        /// <param name="Value">Value or flux of the BC.</param>
        public eAreaBC(ePoint[] Points, int DOFIndex, eBCType BCType, eFunction Func)
            : base(Points, DOFIndex, BCType,Func)
        {
        }


        public override bool Contain(ePoint p)
        {
            throw new NotImplementedException();
        }

        public override void CalculateNodalValues()
        {
            throw new NotImplementedException();
        }
    }
}
