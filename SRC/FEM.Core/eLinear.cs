using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM
{
    /// <summary>
    /// Represents Linear one dimensional Element.
    /// </summary>
    public class eLinear : eLineElement
    {
        public eLinear(eNode n1, eNode n2)
            : base(new eNode[2] { n1, n2 })
        {
           // shapeFucns = new eShapeFunctions(eDomainType.T1D, points, new int[2] { 0, 1 });
        }

        /// <summary>
        /// Creates new instance of Linear line element given nodes connecting to it.
        /// </summary>
        /// <param name="ConNodes">Nodes connecting to the element.</param>
        public eLinear(eNode[] ConNodes)
            : base(ConNodes)
        {
           // shapeFucns = new eShapeFunctions(eDomainType.T1D, points, new int[2] { 0, 1 });
        }

        public override void FormElementMatrix()
        {
            ke = new DenseMatrix(NoDOFs);

            ke[0, 0] = 2 * length / 6;
            ke[0, 1] = length / 6;
            ke[1, 0] = length / 6;
            ke[1, 1] = 2 * length / 6;


            //ke[0, 0] = 1 / length + length / 3;
            //ke[0, 1] = -1 / length + length / 6;
            //ke[1, 0] = ke[0, 1];
            //ke[1, 1] = ke[0, 0]; 
            //double a1 = -(1 / length), b1 = -3.0 / 2.0, c1 = 2 * length / 6;
            //ke[0, 0] = a1 - b1 + c1 * 2;
            //ke[0, 1] = -a1 + b1 + c1;
            //ke[1, 0] = -a1 - b1 + c1;
            //ke[1, 1] = a1 + b1 + c1 * 2;
        }

        public override void FormElementVector()
        {
            fe = new DenseVector(ke.RowCount);
            //fe[0] = length / 2;
            //fe[1] = fe[0];

            //fe[0] = length * (EndCoord.X + 2 * StartCoord.X) / 6;
            //fe[1] = length * (2 * EndCoord.X + StartCoord.X) / 6;
        }
    }
}
