using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    /// <summary>
    /// Lists all type of model which are solved by FEM method. 
    /// </summary>
    public enum eModelType
    { 
        DescereteSys,
        General,
        Beam,
        Truss2D,
        Truss3D,
        Frame3D,
        Frame2D,
        PlaneStress,
        Plate,
        PlaneStrain,    
    }
}
