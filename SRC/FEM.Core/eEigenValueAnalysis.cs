using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace FEM
{
    public class eEigenValueAnalysis : eAnalysis
    {

        public eEigenValueAnalysis(eModel Model)
            : base(Model)
        {
        }

        public override void RunAnalysis()
        {
            throw new NotImplementedException();
        }

        protected override void ApplyBC( Matrix<double> K, Vector<double> F)
        {
           
        }

        protected override void Assemble()
        {
            throw new NotImplementedException();
        }
    }
}
