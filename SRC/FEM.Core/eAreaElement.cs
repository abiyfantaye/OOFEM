using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Integration;
using ESADS.EGraphics;
using System.Drawing;


namespace FEM
{
    public class eAreaElement : eElement
    {

        public eAreaElement(eNode [] connNodes)
            : base(connNodes)
        {
            this.FillArea();
        }

        /// <summary>
        /// Holds the area of the Element.
        /// </summary>
        protected double area;
        /// <summary>
        /// Gets the are of the Element.
        /// </summary>
        public double Area
        {
            get
            {
                return area;
            }
        }

        public override void FormElementMatrix()
        {
            throw new NotImplementedException();
        }

        public override void FormElementVector()
        {
           
        }

        public override void FormMassMatrix()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fills the area of the shape. 
        /// </summary>
        private void FillArea()
        {
            List<ePoint> pts = new List<ePoint>(points);
            pts.Add(pts[0]);

            double sum = 0;

            for (int i = 0; i < pts.Count - 1; i++)
            {
                sum += pts[i].X * pts[i + 1].Y;
                sum -= pts[i + 1].X * pts[i].Y;
            }
            area = Math.Abs(sum / 2);
            
        }

        /// <summary>
        /// Draws this area alement given the layer on which the drawing is done.
        /// </summary>
        /// <param name="layer">The layer on which the drawing is done.</param>
        public override void Draw(eLayer layer)
        {
            PointF[] res = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                res[i] = points[i];
            }

            layer.AddPolyGon(res);
        }
    }
}
