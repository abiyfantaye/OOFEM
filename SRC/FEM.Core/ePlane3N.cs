using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using FEM.Mathematics;
namespace FEM
{
    public class ePlane3N : ePlaneStressElement
    {
        protected Matrix<double> B;


         public ePlane3N (eNode[] ConNodes, double E, double v)
            : base(ConNodes,E,v)
        {
        }

         public ePlane3N(eNode[] ConNodes)
            : base(ConNodes)
        {
        }

         public override void FormElementMatrix()
         {
             Vector<double> dhdx, dhdy;

             D = new DenseMatrix(3, 3, new double[9]
                               {1,  v,  0,
                                v,  1,  0,
                                0,  0, (1-v)/2});
             D = (E / (1 - v * v)) * D;

             ke = new DenseMatrix(NoDOFs);
             disp = new DenseVector(ke.RowCount);


             dhdx = (0.5 / area) * (new DenseVector(new double[3] { points[1].Y - points[2].Y, points[2].Y - points[0].Y, points[0].Y - points[1].Y }));
             dhdy = (0.5 / area) * (new DenseVector(new double[3] { points[2].X - points[1].X, points[0].X - points[2].X, points[1].X - points[0].X }));
             B = Bmtx(dhdx, dhdy);

             ke = B.Transpose() * D * B * area;
         }


        
        private Matrix<double> Bmtx(Vector<double> dhdx, Vector<double> dhdy)
        {
            Matrix<double> B = new DenseMatrix(3, NoDOFs);
            int m, n;
            for (int i = 0; i < nodes.Length; i++)
            {
                m = i * 2;
                n =  m + 1;
                B[0, m] = dhdx[i];
                B[1, n] = dhdy[i];
                B[2, m] = dhdy[i];
                B[2, n] = dhdx[i];
            }

            return B;
        }

        public override void FillAnalysisResults()
        {
            disp.SetSubVector(0, 2, nodes[0].Values);
            disp.SetSubVector(2, 2, nodes[1].Values);
            disp.SetSubVector(4, 2, nodes[2].Values);
            stress = new List<Vector<double>>();
            stress.Add(D * B * disp);
        }

        public override Vector<double> StressAt(int n)
        {
            return stress[0];
        }
    }
}
