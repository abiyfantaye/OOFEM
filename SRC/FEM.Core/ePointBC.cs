using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FEM.Mathematics;

namespace FEM
{
    public class ePointBC : eBC
    {
        public ePointBC(ePoint Point, int DOFIndex, double Value)
            : base(new ePoint[] { Point }, DOFIndex, eBCType.Essential, new eConst(Value))
        {

        }

        public ePointBC(ePoint Point, int DOFIndex, double Value, eBCType BCType)
            : base(new ePoint[] { Point }, DOFIndex, BCType, new eConst(Value))
        {

        }


        public override bool Contain(ePoint p)
        {
            return p == points[0];
        }


        /// <summary>
        /// Calulates the nodal boundary value to be applied at the apoint on the DOF
        /// </summary>
        public override void CalculateNodalValues()
        {
            nodalVals = new double[1] { Value };
        }

        /// <summary>
        /// Gets the value of the boundary condition.
        /// </summary>
        public double Value
        {
            get
            {
                return func.GetValueAt(new ePoint());
            }
        }
    }
}
