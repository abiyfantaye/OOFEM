using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    public class eFrameElement : eLineElement
    {
        public eFrameElement(eNode[] conNodes)
            : base(conNodes)
        {
        }
    }
}
