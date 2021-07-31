using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FEM.Output;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using Plotting;
using MathWorks.MATLAB.NET.Utility;

namespace FEM
{
    public class e2DModel : eModel
    {
        /// <summary>
        /// Creates new intance of two dimensional models given name of the model and type of the model.
        /// </summary>
        /// <param name="name">Name of the model.</param>
        /// <param name="ModelType">Type of the model.</param>
        public e2DModel(string name, eAnalysisType AnalysisType = eAnalysisType.SteadyState)
            : base(name, AnalysisType)
        {
        }

        /// <summary>
        /// Adds a LinearTriangular Element for a given coordinates of the connecting nodes.
        /// </summary>
        /// <param name="p1">The the first point.</param>
        /// <param name="p2">The second point.</param>
        /// <param name="p3">The third point.</param>
        /// <returns></returns>
        public eLinearTriangular AddLinearTriangularElement(ePoint p1, ePoint p2, ePoint p3)
        {
            eNode[] ConNds = new eNode[3];

            ConNds[0] = nodes.AddNode(p1, mesh.DOFsPerNode);
            ConNds[1] = nodes.AddNode(p2, mesh.DOFsPerNode);
            ConNds[2] = nodes.AddNode(p3, mesh.DOFsPerNode);

            eLinearTriangular el = new eLinearTriangular(ConNds);
            elements.AddElement(el);
            return el;
        }

        /// <summary>
        /// Adds a BiLinear Rectangular Element for a given coordinates of the connecting nodes.
        /// </summary>
        /// <param name="p1">The the first point.</param>
        /// <param name="p2">The second point.</param>
        /// <param name="p3">The third point.</param>
        /// <param name="p4">The forth point.</param>
        /// <returns></returns>
        public eBiLinearRectangular AddBilinearRectangularElement(ePoint p1, ePoint p2, ePoint p3 ,ePoint p4)
        {
            eNode[] ConNds = new eNode[4];
            ConNds[0] = nodes.AddNode(p1, mesh.DOFsPerNode);
            ConNds[1] = nodes.AddNode(p2, mesh.DOFsPerNode);
            ConNds[2] = nodes.AddNode(p3, mesh.DOFsPerNode);
            ConNds[3] = nodes.AddNode(p4, mesh.DOFsPerNode);

            eBiLinearRectangular el = new eBiLinearRectangular(ConNds);
            elements.AddElement(el);
            return el;
        }

        /// <summary>
        /// Meshs a rectangular model with a given meshing element type and number of elements 
        /// in x and y direction.
        /// </summary>
        /// <param name="MeshingElementType">Type of meshing element to used</param>
        /// <param name="nx">Number of elements in x direction.</param>
        /// <param name="ny">Number of elements in y direction.</param>
        public void MeshRectArea(eMeshElementType MeshingElementType, int nx, int ny)
        {
            mesh.ElementType = MeshingElementType;
            //if (MeshingElementType == eMeshElementType.LinearTirangular)
               // mesh.MeshRectArea2(nx, ny);
           // else if (MeshingElementType == eMeshElementType.BilinearRectagular)
                mesh.MeshByBilinearRectangular(nx, ny);

        }

        /// <summary>
        /// Creates rectangular domain for the problem for the given geometry.
        /// </summary>
        /// <param name="Location">The coordinate of the bottom left correner of the rectangle.</param>
        /// <param name="W">Width of the rectangle.</param>
        /// <param name="H">Hight of the rectangle.</param>
        public void CreateRectDomain(ePoint Location, double W, double H)
        {
            this.domain = new ePoint[4];
            domain[0] = Location;
            domain[1] = new ePoint(Location.X + W, Location.Y);
            domain[2] = new ePoint(Location.X + W, Location.Y + H);
            domain[3] = new ePoint(Location.X, Location.Y + H);
        }

        /// <summary>
        /// Prints the nodal result of two dimensional model.
        /// </summary>
        public override void PrintNodalValues()
        {
            double[] xList = nodes.GetNodalXValues();
            double[] yList = nodes.GetNodalYValues();

            if (Analysis.GetType() == typeof(eSteadyStateAnalysis))
            {
                eOuput form = new eOuput(xList, yList, analysis.Result.ToArray());
                Application.Run(form);
            }
        }

        public double[] GetExactResult()
        {
            double[] reslt = new double[analysis.Result.Count];
            double x,y;
            for (int i = 0; i < reslt.Length; i++)
            {
                x = nodes[i].X;
                y = nodes[i].Y;
                reslt[i] = 100 * Math.Sinh(Math.PI/10 * y) * Math.Sin(Math.PI/10 * x) / Math.Sinh(Math.PI);

            }
            return reslt;
        }

        public override void PlotResult()
        {
            MWArray x, y, u;
            ePlot plt = new ePlot();

            if (analysisType == eAnalysisType.SteadyState)
            {
                x = new MWNumericArray(GetXGrid());
                y = new MWNumericArray(GetYGrid());
                u = new MWNumericArray(GetZGrid());         //The Field variable
                plt.Surface(x, y, u, this.Name, "x", "y", "u", 0);
                //plt.PlotContr(x, y, u, "y");
                plt.ContrFill(x, y, u, 20);
                //plt.ContrFill(x, y, u, 40);
            }
            else if (analysisType == eAnalysisType.Transient)
            {
                x = new MWNumericArray(((eTransientAnalysis)analysis).Time);
                y = new MWNumericArray(((eTransientAnalysis)analysis).History);
                plt.Plot1D(x, y);

                x = new MWNumericArray(GetXGrid());
                y = new MWNumericArray(GetYGrid());
                u = new MWNumericArray(GetZGrid());         //The Field variable
                plt.Surface(x, y, u, this.Name, "x", "y", "u", 0);
                //plt.PlotContr(x, y, u, "y");
                //plt.ContrFill(x, y, u, 20);
                //plt.ContrFill(x, y, u, 40);
            }

        }

        protected double[,] GetXGrid()
        {
            double[,] res = new double[mesh.nx + 1, mesh.ny + 1];
            double X = domain[0].X;
            double b = (domain[2].X - domain[0].X)/mesh.nx;

            for (int i = 0; i <= mesh.nx; i++)
            {
                for (int j = 0;j<= mesh.ny; j++)
                {
                    res[i, j] = X ;
                }
                X += b;
            }

            return res;
        }

        public double[,] GetYGrid()
        {
            double[,] res = new double[mesh.nx + 1, mesh.ny + 1];
            double Y = domain[0].Y;
            double h = (domain[2].Y - domain[0].Y) / mesh.ny;


            for (int j = 0; j <= mesh.ny; j++)
            {
                for (int i = 0; i <= mesh.nx; i++)
                {
                    res[i, j] = Y;
                }
                Y += h;
            }

            return res;
        }

        public double[,] GetZGrid()
        {
            double[,] res = new double[mesh.nx + 1, mesh.ny + 1];
            double Y = domain[0].Y;
            double h = (domain[2].Y - domain[0].Y) / mesh.ny;
            double X = domain[0].X;
            double b = (domain[2].X - domain[0].X) / mesh.nx;


            for (int i = 0; i <= mesh.nx; i++)
            {
                for (int j = 0; j <= mesh.ny; j++)
                {
                    res[i, j] = analysis.Result[nodes.FindNode(X, Y).DOFs[0]];
                    Y += h;
                }
                Y = domain[0].Y;
                X += b;
            }
            return res;
        }

        /// <summary>
        /// Shows the flux on nodes found on the section y.
        /// </summary>
        /// <param name="y">The y coordinate of the section.</param>
        public void ShowFluxOnY(double y)
        {
            List<double> x = new List<double>();
            List<double> vals = new List<double>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (eMath.AreEqual(nodes[i].Y, y))
                {
                    x.Add(nodes[i].X);
                    vals.Add(Math.Round(analysis.Fluxes[i],6));
                }
            }

            Application.Run(new eOuput(x.ToArray(), vals.ToArray()));
        }

        /// <summary>
        /// Shows the flux on nodes found on the section x.
        /// </summary>
        /// <param name="x">The y coordinate of the section.</param>
        public void ShowFluxOnX(double x)
        {
            List<double> y = new List<double>();
            List<double> vals = new List<double>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (eMath.AreEqual(nodes[i].X, x))
                {
                    y.Add(nodes[i].Y);
                    vals.Add(Math.Round(analysis.Fluxes[i]));
                }
            }

            Application.Run(new eOuput(y.ToArray(), vals.ToArray()));
        }


    }
}
