using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    public class eBeamElement : eLineElement
    {
        public eBeamElement(eNode [] connNodes):base(connNodes)
        {
        }
    }
}
