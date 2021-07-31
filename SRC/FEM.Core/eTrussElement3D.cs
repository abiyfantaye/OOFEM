using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    public class eTrussElement3D : eTrussElement
    {
         public eTrussElement3D(eNode[] ConnNodes)
            : base(ConnNodes)
        {
           
        }
    }
}
