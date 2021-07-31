using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESADS.EGraphics;

namespace FEM
{
    public class eSolidElement : eElement
    {

        public eSolidElement( eNode [] connNodes)
            : base( connNodes)
        {
        }

        #region Methods

 
        #endregion


        public override void FormElementMatrix()
        {
            throw new NotImplementedException();
        }

        public override void FormElementVector()
        {
            throw new NotImplementedException();
        }

        public override void Draw(eLayer layer)
        {
            throw new NotImplementedException();
        }

        public override void FormMassMatrix()
        {
            throw new NotImplementedException();
        }
    }
}
