using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
namespace FEM
{
    public class eModalAnalysis : eAnalysis
    {

        public eModalAnalysis(eModel Model)
            : base(Model)
        {
        }

        public override void RunAnalysis()
        {
            throw new NotImplementedException();
        }

        protected override void ApplyBC(Matrix<double> K, Vector<double> F)
        {
            throw new NotImplementedException();
        }

        protected override void Assemble()
        {
        }
    }
}
