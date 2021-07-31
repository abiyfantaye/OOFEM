using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathWorks.MATLAB.NET.Arrays;
using FEM.Output;
using Plotting;
namespace FEM
{

    /// <summary>
    /// Defines base class for all one dimensional problems. 
    /// </summary>
    public class e1DModel : eModel
    {

        /// <summary>
        /// Creates an instance of FEM model for one dimensional problems.
        /// </summary>
        /// <param name="name">Name of the model.</param>
        /// <param name="X1">Starting X-coordinate of the domain of the model.</param>
        /// <param name="X2">Ending X-coordinate of the domain of the model.</param>
        public e1DModel(string name, double X1, double X2, eAnalysisType AnalysisType = eAnalysisType.SteadyState)
            : base(name,AnalysisType)
        {
            this.domain = new ePoint[2] { new ePoint(X1), new ePoint(X2) };
        }

        /// <summary>
        /// Gets or sets the mesh one dimensional object for the Model.
        /// </summary>
        public eMesh MeshObj
        {
            get
            {
                return mesh;
            }
            set
            {
                mesh = value;
            }
        }

        /// <summary>
        /// Prints the nodal result.
        /// </summary>
        public virtual void ShowResult()
        {
            
            MWArray xdata = new MWNumericArray(nodes.GetNodalXValues());
            MWArray ydata = new MWNumericArray(analysis.Result.ToArray());
            ePlot p = new ePlot();
            p.Plot1D(xdata, ydata);
        }

        /// <summary>
        /// Adds Linear Line Element
        /// </summary>
        /// <param name="X1">X-Coordinate of the First Node.</param>
        /// <param name="X2">X-Coordinate of the Second Node.</param>
        /// <returns>Returns the added element.</returns>
        public eLinear AddLinearLineElement(double X1, double X2)
        {
            eNode[] conNs = new eNode[2];//Holds array of connecting nodes.

            conNs[0] = nodes.AddNode(new ePoint(X1), mesh.DOFsPerNode);
            conNs[1] = nodes.AddNode(new ePoint(X2), mesh.DOFsPerNode);
            eLinear el = new eLinear(conNs);
            elements.AddElement(el);
            return el;
        }


        /// <summary>
        /// Adds Linear Line Element
        /// </summary>
        /// <param name="X1">X-Coordinate of the First Node.</param>
        /// <param name="X2">X-Coordinate of the Second Node.</param>
        /// <returns>Returns the added element.</returns>
        public eTrussElement1D Add1DTrussElement(double X1, double X2, double A, double E, double Density)
        {
            eNode[] conNs = new eNode[2];//Holds array of connecting nodes.

            conNs[0] = nodes.AddNode(new ePoint(X1), mesh.DOFsPerNode);
            conNs[1] = nodes.AddNode(new ePoint(X2), mesh.DOFsPerNode);
            eTrussElement1D el = new eTrussElement1D(conNs,A,E,Density);
            elements.AddElement(el);
            return el;
        }

        /// <summary>
        /// Adds Linear Line Element contnueing imediately from the last node.
        /// </summary>
        /// <param name="X2">X-Coordinate of the second Node.</param>
        /// <returns>Returns the added element.</returns>
        public eLinear AddLinearLineElement(double X2)
        {
            eNode[] conNs = new eNode[2];//Holds array of connecting nodes.

            conNs[0] = nodes.Last;
            conNs[1] = nodes.AddNode(new ePoint(X2), mesh.DOFsPerNode);
            eLinear el = new eLinear(conNs);
            elements.AddElement(el);
            return el;
        }


        /// <summary>
        /// Adds Quadratic line element to the collection.
        /// </summary>
        /// <param name="X1">X-Coordinate of the first point.</param>
        /// <param name="X2">X-Coordinate of the second point.</param>
        /// <param name="X3">X-Coordinate of the third point.</param>
        /// <returns>Retursn the element added.</returns>
        public eQuadratic AddQuadraticLineElement(double X1, double X2, double X3)
        {
            eNode[] conNs = new eNode[3];//Holds array of connecting nodes.

            conNs[0] = nodes.AddNode(new ePoint(X1), mesh.DOFsPerNode);
            conNs[1] = nodes.AddNode(new ePoint(X2), mesh.DOFsPerNode);
            conNs[2] = nodes.AddNode(new ePoint(X3), mesh.DOFsPerNode);
            eQuadratic el = new eQuadratic(conNs);
            elements.AddElement(el);
            return el;
        }

        public override void PrintNodalValues()
        {
            double[] X = nodes.GetNodalXValues();
            eOuput opt = new eOuput(X, analysis.Result.ToArray());
            System.Windows.Forms.Application.Run(opt);
        }

        public override void PlotResult()
        {
            throw new NotImplementedException();
        }

        public void DrawTimeHistory(double X, int dofIndex, int r)
        {
            eNode node = nodes.FindNode(X);
            int idx = node.DOFs[dofIndex];
            eTransientAnalysis an = (eTransientAnalysis)analysis;
            int n = an.Acc.Count;

            double[] a = new double[n];
            double[] v = new double[n + 1];
            double[] d = new double[n];

            double[] t = new double[n];
            double[] vt = new double[n+1];

            for (int i = 0; i < n; i++)
            {
                a[i] = an.Acc[i][idx];
                v[i] = an.Velocities[i][idx];
                d[i] = an.Disps[i][idx];
                t[i] = i * an.TimeStep;
                vt[i + 1] = i * an.TimeStep + 0.5 * an.TimeStep;
            }

            v[n] = an.Velocities[n][idx];

            ePlot plt = new ePlot();

            if( r == 0)
            plt.Plot1D(new MWNumericArray(t), new MWNumericArray(d));
            else if(r == 1)
            plt.Plot1D(new MWNumericArray(vt), new MWNumericArray(v));
            else
            plt.Plot1D(new MWNumericArray(t), new MWNumericArray(a));
        }
    }
}
