using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using FEM.Mathematics;

namespace FEM
{
    public class eLinearTriangular : eAreaElement
    {
        /// <summary>
        /// Creates Linear Triangular 2D(Area) element given connecting three nodes.
        /// The nodes should be ordered in anticlockwise direction.
        /// </summary>
        /// <param name="n1">The first node.</param>
        /// <param name="n2">The second node.</param>
        /// <param name="n3">The third node.</param>
        public eLinearTriangular(eNode n1, eNode n2, eNode n3)
            : base(new eNode[3] { n1, n2, n3 })
        {
            int[] pows = new int[6] { 0, 0, 1, 0, 0, 1 };
            shapeFuncs = new ePolynomial[3];

            for (int i = 0; i < shapeFuncs.Length; i++)
            {

            }
           
        }

        ///Creates Linear Triangular 2D(Area) element given connecting three nodes.
        ///The nodes should be ordered in anticlockwise direction.
        /// </summary>
        /// <param name="connNodes">Array of nodes connecting to the element.</param>
        public eLinearTriangular(eNode[] connNodes)
            : base(connNodes)
        {
            int[] pows = new int[6] { 0, 0, 1, 0, 0, 1 };
            shapeFuncs = new ePolynomial[3];

            for (int i = 0; i < shapeFuncs.Length; i++)
            {

            }
        }

        public override void FormElementMatrix()
        {
            ke = new DenseMatrix(GetDofPerElement());
            double x1 = nodes[0].X, x2 = nodes[1].X, x3 = nodes[2].X, y1 = nodes[0].Y, y2 = nodes[1].Y, y3 = nodes[2].Y;

           
            ke[0, 0] = (1 / (4 * area)) * (Math.Pow(x3 - x2, 2) + Math.Pow(y2 - y3, 2));
            ke[0, 1] = ke[1, 0] = (1 / (4 * area)) * ((x3 - x2) * (x1 - x3) + (y2 - y3) * (y3 - y1));
            ke[0, 2] = ke[2, 0] = (1 / (4 * area)) * ((x3 - x2) * (x2 - x1) + (y2 - y3) * (y1 - y2));
            ke[1, 1] = (1 / (4 * area)) * (Math.Pow(x1 - x3, 2) + Math.Pow(y3 - y1, 2));
            ke[1, 2] = ke[2, 1] = (1 / (4 * area)) * ((x1 - x3) * (x2 - x1) + (y3 - y1) * (y1 - y2));
            ke[2, 2] = (1 / (4 * area)) * (Math.Pow(x2 - x1, 2) + Math.Pow(y1 - y2, 2));
        }

        public override void FormElementVector()
        {
            fe = new DenseVector(ke.RowCount);
        }

        public override void FormMassMatrix()
        {
            me = new DenseMatrix(ke.RowCount);
            me[0, 0] = me[1, 1] = me[2, 2] = 2;
            me[0, 1] = me[0, 2] = me[1, 0] = me[1, 2] = me[2, 0] = me[2, 1] = 1;
            me = (area / 12) * me;
        }
    }
}
