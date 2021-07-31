using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    public class e3DModel : eModel
    {
        public e3DModel(string name, eAnalysisType AnalysisType)
            : base(name,AnalysisType)
        {
        }

        public override void PrintNodalValues()
        {
            throw new NotImplementedException();
        }

        public override void PlotResult()
        {
            throw new NotImplementedException();
        }
    }
}
