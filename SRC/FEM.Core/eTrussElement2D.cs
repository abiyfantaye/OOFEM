using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM
{
    public class eTrussElement2D : eTrussElement
    {
        
        /// <summary>
        /// Holds the transformation matrix of the element.
        /// </summary>
        private Matrix<double> T;
        /// <summary>
        /// Holds the angle of the element measured counterclockwise from x-axis.
        /// </summary>
        private double angle;
      
        public eTrussElement2D(eNode[] ConnNodes)
            : base(ConnNodes)
        {
           
        }

        /// <summary>
        /// Creates new truss element for the given element properties.
        /// </summary>
        /// <param name="N1">First node.</param>
        /// <param name="N2">Second node.</param>
        /// <param name="Area">Cross sectional area of the truss element.</param>
        /// <param name="Density">Density of the material from which the elemet is made of.</param>
        /// <param name="E">Modulus of elasticity of the matrial from which the element is made of.</param>
        public eTrussElement2D(eNode N1, eNode N2, double Area, double E, double Density)
            : base(new eNode[2] { N1, N2 })
        {
            this.A = Area;
            this.density = Density;
            this.E = E;
            this.massMatrixType = eMassMatrixType.Consistent;
            CalculateAngle(); 
        }

        public override void FormElementMatrix()
        {
            gDisp = new DenseVector(4);

            FormTransformationMatrix();

            double c = T[0, 0];
            double s = T[0, 1];
            
            ke = new DenseMatrix(4, 4, new double[16]
                { c*c,  c*s, -c*c, -c*s,
                  c*s,  s*s, -c*s, -s*s,
                 -c*c, -c*s,  c*c,  c*s,
                 -c*s, -s*s,  c*s,  s*s
                });
            ke = (A * E / length) * ke;
        }

        public override void FormElementVector()
        {
            fe = new DenseVector(4);
        }

        public override void FormMassMatrix()
        {

            double c = T[0, 0];
            double s = T[0, 1];

            if (massMatrixType == eMassMatrixType.Consistent)
            {
                me = new DenseMatrix(4, 4, new double[16]
                { 2*c*c,  2*c*s,  c*c,    c*s,
                  2*c*s,  2*s*s,  c*s,    s*s,
                  c*c,    c*s,    2*c*c,  2*c*s,
                  c*s,    s*s,    2*c*s,  2*s*s
                });
                me = (density * A * length / 6) * me;
            }
            else
            {
                me = new DenseMatrix(4, 4, new double[16]
                { c*c,  c*s,  0,    0,
                  c*s,  s*s,  0,    0,
                  0,    0,    c*c,  c*s,
                  0,    0,    c*s,  s*s
                });
                me = (density * A * length / 2) * me;
            }
        }

        private void FormTransformationMatrix()
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            T = new DenseMatrix(4, 4, new double[16]
                {c,-s, 0 ,0,
                 s, c, 0 ,0,
                 0, 0, c,-s,
                 0, 0, s, c
                });
        }

        /// <summary>
        /// Calculates the inclination angle of the elment.
        /// </summary>
        private void CalculateAngle()
        {
            ePoint p1 = StartCoord;
            ePoint p2 = EndCoord;
            try
            { 
                angle = Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
            }
            catch (DivideByZeroException)
            {
                angle = Math.PI / 2;
            }
        }

      

        /// <summary>
        /// Gets the transformation matrix of the element.
        /// </summary>
        public Matrix<double> TMatrix
        {
            get
            {
                return T; 
            }
        }

        public double GetLocalForces()
        {

            Vector<double> force = ke * gDisp;

            return T[0, 0] * force[2] + T[0, 1] * force[3];
        }

        public Vector<double> GetStress()
        {
            return (T * (ke * gDisp)) / A;
        }

        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }

        public void FillAnalysisResult()
        {
            gDisp.SetSubVector(0, 2, nodes[0].Values);
            gDisp.SetSubVector(2, 2, nodes[1].Values);      
        }
    }
}
