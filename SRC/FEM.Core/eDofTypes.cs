using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    /// <summary>
    /// Represents different types of degree of freedom.
    /// </summary>
    public enum eDOFTypes
    {
        Disp_X,
        Disp_Y,
        Disp_Z,
        Rot_X,
        Rot_Y,
        Rot_Z,
        Scalar,
        NotAssigned
    }
}
