using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathWorks.MATLAB.NET.Arrays;
using Plotting;

namespace FEM
{
    public class eTruss : e2DModel
    {
        
        /// <summary>
        /// Creates new 2D truss structure 
        /// </summary>
        /// <param name="Name">Name of the structure.</param>
        public eTruss(string Name, eAnalysisType AnalysisType = eAnalysisType.SteadyState)
            : base(Name,AnalysisType)
        {

        }

        public eTrussElement2D AddElement(ePoint p1, ePoint p2, double Area, double E, double Density)
        {
            eNode N1 = nodes.AddNode(p1, 2);
            eNode N2 = nodes.AddNode(p2, 2);

            eTrussElement2D el = new eTrussElement2D(N1, N2, Area, E, Density);
            elements.AddElement(el);

            return el;
        }


        public eTrussElement2D AddElement(ePoint p1, ePoint p2)
        {
            eNode N1 = nodes.AddNode(p1, 2);
            eNode N2 = nodes.AddNode(p2, 2);

            eTrussElement2D el = new eTrussElement2D(N1, N2, 0, 0, 0);
            elements.AddElement(el);

            return el;
        }

      

        public void SetElementProperties(double A, double E, double Density)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                ((eTrussElement)elements[i]).Area = A;
                ((eTrussElement)elements[i]).ModulusOfElasticity = E;
                ((eTrussElement)elements[i]).Density = Density;
            }
        }
        public void SetElementProperties(double A, double E)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                ((eTrussElement)elements[i]).Area = A;
                ((eTrussElement)elements[i]).ModulusOfElasticity = E;
            }
        }

        public void FillAnalysisResult()
        {
            nodes.FillNodalValues(analysis.Result);

            for (int i = 0; i < elements.Count; i++)
            {
                ((eTrussElement2D)elements[i]).FillAnalysisResult();
            }
        }

        public void DrawTimeHistory(double X, double Y, int dofIndex, int r)
        {
            eNode node = nodes.FindNode(X,Y);
            int idx = node.DOFs[dofIndex];
            eTransientAnalysis an = (eTransientAnalysis)analysis;
            int n = an.Acc.Count;

            double[] a = new double[n];
            double[] v = new double[n + 1];
            double[] d = new double[n];

            double[] t = new double[n];
            double[] vt = new double[n + 1];

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

            if (r == 0)
                plt.Plot1D(new MWNumericArray(t), new MWNumericArray(d),"Time(s)","Displacement(m)");
            else if (r == 1)
                plt.Plot1D(new MWNumericArray(vt), new MWNumericArray(v), "Time(s)", "Velocity(m/s)");
            else
                plt.Plot1D(new MWNumericArray(t), new MWNumericArray(a), "Time(s)", "Acceleration(m/s^2)");
        }
    }
}
