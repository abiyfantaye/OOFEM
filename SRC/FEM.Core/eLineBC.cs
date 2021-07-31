using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FEM.Mathematics;

namespace FEM
{
    /// <summary>
    /// Represents Line boundary condition.
    /// </summary>
    public class eLineBC : eBC
    {
        /// <summary>
        /// Creates new instance of line boundary condition.
        /// </summary>
        /// <param name="Points">The pints in the line.</param>
        /// <param name="DOFIndex">The index of DOF in the node on which the BC is going to be applied.</param>
        /// <param name="BCType">The type of BC.</param>
        /// <param name="Func">The function which represetn the variation of BC.</param>
        public eLineBC(ePoint[] Points, int DOFIndex, eBCType BCType, eFunction Func)
            : base(Points, DOFIndex, BCType,Func)
        {
           
        }

        /// <summary>
        /// Checks whether the boundary point contains the point or not.
        /// </summary>
        /// <param name="p">The point to be checked.</param>
        /// <returns></returns>
        public override bool Contain(ePoint p)
        {
            //for (int i = 0; i < points.Length - 1; i++)
            //{
            //    if (Cont(p, points[i], points[i + 1]))
            //        return true;
            //}

            return IsContainedInRect(p);
        }

        /// <summary>
        /// Checks if the point p is found on the line connecting point p1 and p2.
        /// </summary>
        /// <param name="p">The point to be checked.</param>
        /// <param name="p1">The first point of the line.</param>
        /// <param name="p2">The second point of the line.</param>
        /// <returns>Retursn true if the point is found of the line and false if not.</returns>
        private bool PointOnLine(ePoint p, ePoint p1, ePoint p2)
        {
            double X = p1.X <= p2.X ? p1.X : p2.X;
            double Y = p1.Y <= p2.Y ? p1.Y : p2.Y;
            double H = Math.Abs(p1.Y - p2.Y);
            double W = Math.Abs(p1.X - p2.X);



            if (p == p1 || p == p2)
                return true;

            if (H == 0)
            {
                if (p.X < X || p.X > X + W)
                    return false;
                return eMath.AreEqual(Y, p.Y);
            }

            else if (W == 0)
            {
                if (p.Y < Y || p.Y > Y + H)
                    return false;
                return eMath.AreEqual(X, p.X);
            }

            if (!(p.X < X || p.Y < Y || p.X > X + W || p.Y > Y + H))
                return eMath.AreEqual(GetSlope(p1, p2), GetSlope(p1, p));
            else
                return false;
        }

        /// <summary>
        /// Gets the slope of the line connecting the given two points. 
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns></returns>
        private double GetSlope(ePoint p1, ePoint p2)
        {
            return (p2.Y - p1.Y) / (p2.X - p1.X);
        }

        /// <summary>
        /// Calculates the nodal boundary condition to be applied at each nodes in the boundary line.
        /// </summary>
        public override void CalculateNodalValues()
        {
            nodalVals = new double[nodes.Count];

            if (bcType == eBCType.Essential)
            {
                for (int i = 0; i < nodalVals.Length; i++)
                {
                    nodalVals[i] = func.GetValueAt(nodes[i].Coord);                   
                }
            }
            else if (bcType == eBCType.Natural)
            {
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    double[] segVector = GetSegmentVector(eMath.GetLength(nodes[i].Coord, nodes[i + 1].Coord));
                    nodalVals[i] += segVector[0];
                    nodalVals[i + 1] += segVector[1];
                }
            }
        }

        /// <summary>
        /// Returns the boundary value vector for a given line segment.
        /// </summary>
        /// <param name="hi">The length of the segment on which the flux is applied.</param>
        /// <returns></returns>
        private double[] GetSegmentVector(double hi)
        {
            return new double[2] { func.GetValueAt(new ePoint()) * hi / 2, func.GetValueAt(new ePoint()) * hi / 2, };
        }

        private bool IsContainedInRect(ePoint p)
        {
            double tol = 0.0002;

            if (Math.Abs(points[0].X - points[1].X) < tol)
            {
                if (points[1].Y + tol < p.Y || points[0].Y - tol > p.Y || points[1].X + tol < p.X || points[1].X - tol > p.X)
                    return false;
                else return true;
            }
            else
            {
                if (points[1].Y + tol < p.Y || points[1].Y - tol > p.Y || points[1].X + tol < p.X || points[0].X - tol > p.X)
                    return false;
                else return true;
            }
             
        }

        //private bool ContainedInRect(ePoint p)
        //{

        //    ePoint[] pts = GetContRect();

        //    if (p.X < pts[3].X || p.X > pts[1].X || p.Y > pts[2].Y || p.Y < pts[0].Y)
        //        return false;
        //    double a1, a2, b1, b2;
            
        //     a1 = (pts[1].Y - pts[0].Y) / (pts[1].X - pts[0].X);            
        //     b1 = pts[0].Y - a1 * pts[0].X;

        //     a2 = (pts[3].Y - pts[2].Y) / (pts[3].X - pts[2].X);
        //     b2 = pts[3].Y - a2 * pts[3].X;

        //     return a1 * p.X + b1 < p.Y && a2 * p.X + b2 > p.Y;
        //}



        private double Angle()
        {
            ePoint p1 = points[0];
            ePoint p2 = points[1];
            try
            {
                return Math.Atan((p2.Y - p1.Y) / (p2.X - p1.X));
            }
            catch (DivideByZeroException)
            {
                return Math.PI / 2;
            }
        }

    }
}
