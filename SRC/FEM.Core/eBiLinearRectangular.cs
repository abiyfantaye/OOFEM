using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using FEM.Mathematics;

namespace FEM
{
    public class eBiLinearRectangular : eAreaElement
    {
        /// <summary>
        /// Creates Bilinear Rectangular area element given connecting four nodes.
        /// The nodes should be arranged in the anticlockwise order.
        /// </summary>
        /// <param name="ConnNodes">Nodes connecting to the Element.</param>
        public eBiLinearRectangular(eNode[] ConnNodes)
            : base(ConnNodes)
        {
           // shapeFucns = new eShapeFunctions(eDomainType.T2D, points, new int[8] { 0, 0, 1, 0, 0, 1, 1, 1 });
        }

        public override void FormElementMatrix()
        {
            /*
            double b = Math.Abs(nodes[0].X - nodes[2].X) / 2;
            double c = Math.Abs(nodes[0].Y - nodes[2].Y) / 2;
            ke = new DenseMatrix(4);

            ke[0, 0] = ke[1, 1] = ke[2, 2] = ke[3, 3] = (b * b + c * c) / (3 * b * c);
            ke[0, 1] = ke[1, 0] = ke[2, 3] = ke[3, 2] = (b * b - 2 * c * c) / (6 * b * c);
            ke[0, 2] = ke[2, 0] = ke[1, 3] = ke[3, 1] = -(b * b + c * c) / (6 * b * c);
            ke[0, 3] = ke[3, 0] = ke[1, 2] = ke[2, 1] = (c * c - 2 * b * b) / (6 * b * c);
             */
            ke = new DenseMatrix(4);

            double[] X, Y, Wx, Wy;
            double[,] SF, derv;
            int nInt = 2;
            Matrix<double> J;
            double detJ;

            X = Y = eGaussQuadrature.GetIntPoits(nInt);
            Wx = Wy = eGaussQuadrature.GetIntWeights(nInt);

            for (int i = 0; i < nInt; i++)
            {
                for (int j = 0; j < nInt; j++)
                {
                    SF = ShapeFunc(X[i], Y[j]);
                    J = Jacobian(SF);
                    detJ = J.Determinant();
                    derv = Derivative(J.Inverse(), SF);

                    for (int m = 0; m < 4; m++)
                    {
                        for (int n = 0; n < 4; n++)
                        {
                            ke[m, n] += (derv[m, 0] * derv[n, 0] + derv[m, 1] * derv[n, 1]) * Wx[i] * Wy[j] * detJ;
                        }
                    }
                }
            }
        }

        public override void FormElementVector()
        {
            fe = new DenseVector(4);
        }

        private double[,] ShapeFunc(double r, double s)
        {
            double[,] res = new double[4, 3];

            res[0, 0] = 0.25 * (1 - r) * (1 - s);
            res[1, 0] = 0.25 * (1 + r) * (1 - s);
            res[2, 0] = 0.25 * (1 + r) * (1 + s);
            res[3, 0] = 0.25 * (1 - r) * (1 + s);

            res[0, 1] = -0.25 * (1 - s);
            res[1, 1] = 0.25 * (1 - s);
            res[2, 1] = 0.25 * (1 + s);
            res[3, 1] = -0.25 * (1 + s);

            res[0, 2] = -0.25 * (1 - r);
            res[1, 2] = -0.25 * (1 + r);
            res[2, 2] = 0.25 * (1 + r);
            res[3, 2] = 0.25 * (1 - r);

            return res;
        }

        private Matrix<double> Jacobian(double[,] SF)
        {
            Matrix<double> J = new DenseMatrix(2);
            for (int i = 0; i < 4; i++)
            {
                J[0, 0] += SF[i, 1] * points[i].X;
                J[0, 1] += SF[i, 1] * points[i].Y;

                J[1, 0] += SF[i, 2] * points[i].X;
                J[1, 1] += SF[i, 2] * points[i].Y;
            }
            return J;
        }

        private double[,] Derivative(Matrix<double> R, double[,] SF)
        {
            double[,] res = new double[4, 2];
            for (int i = 0; i < 4; i++)
            {
                res[i, 0] = R[0, 0] * SF[i, 1] + R[0, 1] * SF[i, 2];
                res[i, 1] = R[1, 0] * SF[i, 1] + R[01, 1] * SF[i, 2];
            }

            return res;
        }

        

        public override void FormMassMatrix()
        {
            me = new DenseMatrix(4, 4,
                 new double[16] { 4, 2, 1, 2, 
                                  2, 4, 2, 1,  
                                  1, 2, 4, 2,  
                                  2, 1, 2, 4 });
            me = (area / 36) * me;
        }
    }
}
