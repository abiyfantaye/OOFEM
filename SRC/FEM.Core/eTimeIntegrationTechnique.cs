using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    public enum eTimeIntegrationTechnique
    {
        ForwardDifference,
        BackwardDifference,
        CrankNicolson,
    }
}
