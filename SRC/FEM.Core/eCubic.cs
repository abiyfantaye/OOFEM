using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    /// <summary>
    /// Represents one dimensional Quibic Element.
    /// </summary>
    public class eCubic : eLineElement
    {
         /// <summary>
        /// Creates new instance of eQubic line element Given connecting four nodes.
        /// </summary>
        /// <param name="n1">The First node.</param>
        /// <param name="n2">The second node.</param>
        /// <param name="n3">The third node.</param>
        /// <param name="n4">The fourth node.</param>
        public eCubic(eNode n1, eNode n2, eNode n3, eNode n4)
            : base(new eNode[4] { n1, n2, n3, n4 })
        {
          //  shapeFucns = new eShapeFunctions(eDomainType.T1D, points, new int[4] { 0, 1, 2, 3 });
        }

        public override void FormElementMatrix()
        {
            base.FormElementMatrix();
        }

        public override void FormElementVector()
        {
            base.FormElementVector();
        }

    }
}
