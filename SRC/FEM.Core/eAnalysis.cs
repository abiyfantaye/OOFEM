using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM
{
    /// <summary>
    /// Represents abtract class for FEM analysis.
    /// </summary>
    public abstract class eAnalysis
    {
        protected bool analysisCompleted;
        protected Matrix<double> K;
        protected Vector<double> F;
        protected Vector<double> result;
        protected eModel model;
        protected double excutionTime;

        public eAnalysis(eModel Model)
        {
            this.model = Model;
            this.analysisCompleted = false;
        }

        public abstract void RunAnalysis();

        protected abstract void ApplyBC(Matrix<double> K, Vector<double> F);

        protected abstract void Assemble();

        public void FillNodalFlux()
        {
           // Assemble();
            F = new DenseVector(K.RowCount);
            F = K * result;
        }

        public Vector<double> Result
        {
            get
            {
                return result;
            }
        }
        public double ExcutionTime
        {
            get
            {
                return excutionTime;
            }
        }
        public Vector<double> Vector
        {
            get
            {
                return F;
            }
        }

        public Matrix<double> Matrix
        {
            get { return K; }
        }

        /// <summary>
        /// Gets or sets the value indicating the completion of the analysis.
        /// </summary>
        public bool AnalysisCompleted
        {
            get
            {
                return analysisCompleted;
            }
        }

        /// <summary>
        /// Gets the nodal fluxes. 
        /// </summary>
        public Vector<double> Fluxes
        {
            get
            {
                if (analysisCompleted)
                    return F;
                throw new Exception("The analysis is not complteted");
            }
        }
    }
}
