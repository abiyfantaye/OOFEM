using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using FEM.Mathematics;

namespace FEM
{
    public abstract class ePlaneStressElement : eAreaElement
    {
        /// <summary>
        /// Holds the modulus of elasticity of the element.
        /// </summary>
        protected double E;
        /// <summary>
        /// Holds the Poisson's ratio of the material.
        /// </summary>
        protected double v;

        protected Vector<double> disp;

        protected List<Vector<double>> stress;

        protected Matrix<double> D;

        public ePlaneStressElement (eNode[] ConNodes, double E, double v)
            : base(ConNodes)
        {
            this.E = E;
            this.v = v;
        }

        public ePlaneStressElement(eNode[] ConNodes)
            : base(ConNodes)
        {
        }

        public override void FormMassMatrix()
        {
        }

        public override void FormElementVector()
        {
            fe = new DenseVector(NoDOFs);
        }

        public double ModulusOfElasticity
        {
            get
            {
                return E;
            }
            set
            {
                E = value;
            }
        }

        public double PoissonRatio
        {
            get
            {
                return v;
            }
            set
            {
                v = value;
            }
        }

        public Vector<double> Disp
        {
            get
            {
                return disp;
            }

            set
            {
                disp = value;
            }
        }
        public List<Vector<double>> Stress
        {
            get
            {
                return stress;
            }
        }
        protected Matrix<double> Bmtx(Vector<double> dhdx, Vector<double> dhdy)
        {
            Matrix<double> B = new DenseMatrix(3, NoDOFs);
            int m, n;
            for (int i = 0; i < nodes.Length; i++)
            {
                m = i * 2;
                n = m + 1;
                B[0, m] = dhdx[i];
                B[1, n] = dhdy[i];
                B[2, m] = dhdy[i];
                B[2, n] = dhdx[i];
            }

            return B;
        }

        public abstract void FillAnalysisResults();

        /// <summary>
        /// Returns the stress at the given node index.
        /// </summary>
        /// <param name="n">Index of the node in the element node array.</param>
        public abstract Vector<double> StressAt(int n);

    }
}
