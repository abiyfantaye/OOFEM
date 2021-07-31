using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using ESADS.EGraphics;

namespace FEM
{
    public class eLineElement : eElement
    {
        /// <summary>
        /// Holds value for the length of the Element.
        /// </summary>
        protected double length;

        /// <summary>
        /// Gets the Length of the Element.
        /// </summary>
        public double Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// Gets the starting coordinate of the Element.
        /// </summary>
        public ePoint StartCoord
        {
            get
            {
                return nodes.First().Coord;
            }
        }

        /// <summary>
        /// Gets the end coordinate of the Element.
        /// </summary>
        public ePoint EndCoord
        {
            get
            {
               return nodes.Last().Coord;
            }

        }

        /// <summary>
        /// Creates an instance of LineElement given nodes connecting.
        /// The nodes must be in thier proper order as they apear in the element.
        /// </summary>
        /// <param name="connNodes">Nodes connecting tto the element.</param>
        public eLineElement(eNode [] connNodes)
            : base( connNodes)
        {
            this.length = eMath.GetLength(StartCoord, EndCoord);
        }

        /// <summary>
        /// Creates an instance of LineElement given three connecting nodes.
        /// The nodes must be in thier proper order as they apear in the element.
        /// <param name="n1">The first node.</param>
        /// <param name="n2">The second node.</param>
        /// <param name="n3">The third node.</param>
        /// </summary>
        public eLineElement(eNode n1, eNode n2, eNode n3)
            : base(new eNode[3] { n1, n2, n3 })
        {
            this.length = eMath.GetLength(StartCoord, EndCoord);
        }

        /// <summary>
        /// Creates an instance of LineElement given two connecting nodes.
        /// The nodes must be in thier proper order as they apear in the element.
        /// <param name="n1">The first node.</param>
        /// <param name="n2">The second node.</param>
        /// </summary>
        public eLineElement(eNode n1, eNode n2)
            : base(new eNode[2] { n1, n2 })
        {
            this.length = eMath.GetLength(StartCoord, EndCoord);
        }

        public override void FormElementMatrix()
        {
           //Form element matrix.
        }

        public override void FormElementVector()
        {
            //Form element Vector.
        }

        public override void Draw(eLayer layer)
        {
            layer.AddLine(StartCoord.X, -StartCoord.Y, EndCoord.X, -EndCoord.Y);
            //layer.AddText(name, (StartCoord.X + EndCoord.X) / 1.95, (StartCoord.Y + EndCoord.Y) / 2.05);
        }

        public override void FormMassMatrix()
        {
            throw new NotImplementedException();
        }
    }
}
