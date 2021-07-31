using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using FEM.Mathematics;

namespace FEM
{
    /// <summary>
    /// Represents 4-node plane stress or strain element.
    /// </summary>
    public class ePlane4N : ePlaneStressElement
    {

        public ePlane4N(eNode[] ConNodes, double E, double v)
            : base(ConNodes)
        {
            this.E = E;
            this.v = v;
        }

        public ePlane4N(eNode[] ConNodes)
            : base(ConNodes)
        {
        }

        public override void FormElementMatrix()
        {
            double[] X, W;
            Vector<double> dhdr, dhds, dhdx, dhdy;
            int nIntP = 2;
            Matrix<double> J, invJ, B;
            double detJ;
            D = new DenseMatrix(3, 3, new double[9]
                               {1,  v,  0,
                                v,  1,  0,
                                0,  0, (1-v)/2});
            D = (E / (1 - v * v)) * D;
            ke = new DenseMatrix(NoDOFs);
            disp = new DenseVector(ke.RowCount);

            X = eGaussQuadrature.GetIntPoits(nIntP);
            W = eGaussQuadrature.GetIntWeights(nIntP);
            for (int i = 0; i < nIntP; i++)
            {
                for (int j = 0; j < nIntP; j++)
                {
                    dhdr = dHdr(X[i], X[j]);
                    dhds = dHds(X[i], X[j]);
                    J = Jacobian(dhdr, dhds);

                    detJ = J.Determinant();
                    invJ = J.Inverse();
                    dhdx = dHdx(dhdr, dhds, invJ);
                    dhdy = dHdy(dhdr, dhds, invJ);
                    B = Bmtx(dhdx, dhdy);

                    ke += B.Transpose() * D * B * W[i] * W[j] * detJ;
                }
            }
        }


        public override void FormMassMatrix()
        {         
        }

        public override void FormElementVector()
        {
            fe = new DenseVector(NoDOFs);
        }

        private Vector<double> H(double r, double s)
        {
            return new DenseVector(new double[4]
                {0.25*(1 - r) * (1 - s),
                 0.25*(1 + r) * (1 - s),
                 0.25*(1 + r) * (1 + s),
                 0.25*(1 - r) * (1 + s)});
        }

        private Vector<double> dHdr(double r, double s)
        {
            return new DenseVector(new double[4]
                {-0.25*(1 - s),
                  0.25*(1 - s),
                  0.25*(1 + s),
                 -0.25*(1 + s)});
        }

        private Vector<double> dHds(double r, double s)
        {
            return new DenseVector(new double[4]
                {-0.25*(1 - r),
                 -0.25*(1 + r),
                  0.25*(1 + r),
                  0.25*(1 - r)});
        }

        private Matrix<double> Jacobian(Vector<double> dhdr, Vector<double> dhds)
        {
            Matrix<double> J = new DenseMatrix(2);

            for (int i = 0; i < nodes.Length; i++)
            {
                J[0, 0] += dhdr[i] * nodes[i].X;
                J[0, 1] += dhdr[i] * nodes[i].Y;
                J[1, 0] += dhds[i] * nodes[i].X;
                J[1, 1] += dhds[i] * nodes[i].Y;
            }
            return J;
        }

        private Vector<double> dHdx(Vector<double> dhdr, Vector<double> dhds, Matrix<double> R)
        {
            Vector<double> dhdx = new DenseVector(nodes.Length);

            for (int i = 0; i < dhdx.Count; i++)
               dhdx[i] = R[0, 0] * dhdr[i] + R[0, 1] * dhds[i];
            return dhdx;
        }

        private Vector<double> dHdy(Vector<double> dhdr, Vector<double> dhds, Matrix<double> R)
        {
            Vector<double> dhdy = new DenseVector(nodes.Length);

            for (int i = 0; i < dhdy.Count; i++)
                dhdy[i] = R[1, 0] * dhdr[i] + R[1, 1] * dhds[i];
            return dhdy;
        }

        public override void FillAnalysisResults()
        {
            disp.SetSubVector(0, 2, nodes[0].Values);
            disp.SetSubVector(2, 2, nodes[1].Values);
            disp.SetSubVector(4, 2, nodes[2].Values);
            disp.SetSubVector(6, 2, nodes[3].Values);
            FindStressAndStrain();
        }

        private void FindStressAndStrain()
        {
            double[] X;
            Vector<double> dhdr, dhds, dhdx, dhdy;
            int nIntP = 2;
            Matrix<double> J, invJ, B;
            stress = new List<Vector<double>>();

            X = eGaussQuadrature.GetIntPoits(nIntP);
            for (int i = 0; i < nIntP; i++)
            {
                for (int j = 0; j < nIntP; j++)
                {
                    dhdr = dHdr(X[i], X[j]);
                    dhds = dHds(X[i], X[j]);
                    J = Jacobian(dhdr, dhds);

                    invJ = J.Inverse();
                    dhdx = dHdx(dhdr, dhds, invJ);
                    dhdy = dHdy(dhdr, dhds, invJ);
                    B = Bmtx(dhdx, dhdy);
                    stress.Add(D * B * disp);
                }
            }
        }

        public override Vector<double>StressAt(int n)
        {
            Vector<double> dhdr, dhds, dhdx, dhdy;
            Matrix<double> J, invJ, B;
            double r = 0, s = 0;

            if (n == 0)
            {
                r = -1;
                s = -1;
            }
            else if (n == 1)
            {
                r = 1;
                s = -1;
            }
            else if (n == 2)
            {
                r = 1;
                s = 1;
            }
            else if (n == 3)
            {
                r = -1;
                s = 1;
            }

            dhdr = dHdr(r, s);
            dhds = dHds(r, s);
            J = Jacobian(dhdr, dhds);

            invJ = J.Inverse();
            dhdx = dHdx(dhdr, dhds, invJ);
            dhdy = dHdy(dhdr, dhds, invJ);
            B = Bmtx(dhdx, dhdy);
            return D * B * disp;
        }     
    }
}
