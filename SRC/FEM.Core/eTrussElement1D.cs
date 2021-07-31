using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM
{
    public class eTrussElement1D : eTrussElement
    {
         public eTrussElement1D(eNode[] ConnNodes)
            : base(ConnNodes)
        {
           
        }

         public eTrussElement1D(eNode[] ConnNodes, double A, double E, double Density)
             : base(ConnNodes)
         {

             this.A = A;
             this.E = E;
             this.density = Density;
         }
         public override void FormElementMatrix()
         {
             gDisp = new DenseVector(2); 

             ke = new DenseMatrix(2, 2, new double[4]{ 1, -1, 
                                                      -1,  1});
             ke = (A * E / length) * ke;
         }

         public override void FormMassMatrix()
         {
             if (massMatrixType == eMassMatrixType.Consistent)
             {
                 me = new DenseMatrix(2, 2, new double[4]{ 2,  1, 
                                                           1,  2});
                 me = (density * A * length / 6) * me;
             }
             else
             {
                 me = new DenseMatrix(2, 2, new double[4]{ 1,  0, 
                                                           0,  1});
                 me = (density * A * length / 2) * me;
             }
         }
         public override void FormElementVector()
         {
             fe = new DenseVector(2);
         }
    }
}
