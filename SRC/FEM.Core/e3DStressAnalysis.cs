using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    public class e3DStressAnalysis : e3DModel
    {
        public e3DStressAnalysis()
            : base("", eAnalysisType.SteadyState)
        {
        }
    }
}
