using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    /// <summary>
    /// Represents Quadratic one dimensional Element.
    /// </summary>
    public class eQuadratic : eLineElement
    {
        /// <summary>
        /// Creates new instance of quadratic line element Given connecting three nodes.
        /// </summary>
        /// <param name="n1">The First node.</param>
        /// <param name="n2">The second node.</param>
        /// <param name="n3">The third node.</param>
        public eQuadratic(eNode n1, eNode n2, eNode n3)
            : base(n1, n2, n3)
        {
           // shapeFucns = new eShapeFunctions(eDomainType.T1D, points, new int[3] { 0, 1, 2 });
        }

        /// <summary>
        /// Creates new instance of quadratic line element Given connecting three nodes.
        /// </summary>
        ///<param name="ConNodes">Array of connecting nodes.</param>
        public eQuadratic(eNode[] ConNodes)
            : base(ConNodes)
        {
           // shapeFucns = new eShapeFunctions(eDomainType.T1D, points, new int[3] { 0, 1, 2 });
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
