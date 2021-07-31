using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using System.Windows.Forms;

namespace FEM
{
    public class ePlaneStress : e2DModel
    {
        /// <summary>
        /// Model class to analyse plane stress problems 
        /// </summary>
        /// <param name="Name"></param>
        public ePlaneStress(string Name)
            : base(Name)
        {
        }

        /// <summary>
        /// Adds 4-node quadrilateral to the model.
        /// The point must be in counterclockwise direction.
        /// </summary>
        /// <param name="p1">Firs point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="p3">Third point.</param>
        /// <param name="p4">Fourth point.</param>
        /// <param name="E">Modulus of elasticity.</param>
        /// <param name="v">Poisson's ratio.</param>
        /// <returns></returns>
        public ePlane4N Addd4NodeQuad(ePoint p1, ePoint p2, ePoint p3, ePoint p4, double E, double v)
        {
            eNode[] nds = new eNode[4];
            nds[0] = nodes.AddNode(p1, 2);
            nds[1] = nodes.AddNode(p2, 2);
            nds[2] = nodes.AddNode(p3, 2);
            nds[3] = nodes.AddNode(p4, 2);

            ePlane4N el = new ePlane4N(nds, E, v);
            elements.AddElement(el);
            return el;
        }

        /// <summary>
        /// Adds 4-node quadrilateral to the model.
        /// The point must be in counterclockwise direction.
        /// </summary>
        /// <param name="p1">Firs point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="p3">Third point.</param>
        /// <param name="p4">Fourth point.</param>
        /// <returns></returns>
        public ePlane4N Addd4NodeQuad(ePoint p1, ePoint p2, ePoint p3, ePoint p4)
        {
            eNode[] nds = new eNode[4];
            nds[0] = nodes.AddNode(p1, 2);
            nds[1] = nodes.AddNode(p2, 2);
            nds[2] = nodes.AddNode(p3, 2);
            nds[3] = nodes.AddNode(p4, 2);

            ePlane4N el = new ePlane4N(nds);
            elements.AddElement(el);
            return el;
        }

        public ePlane3N Add3NodeTriagular(ePoint p1, ePoint p2, ePoint p3)
        {
            eNode[] nds = new eNode[3];
            nds[0] = nodes.AddNode(p1, 2);
            nds[1] = nodes.AddNode(p2, 2);
            nds[2] = nodes.AddNode(p3, 2);

            ePlane3N el = new ePlane3N(nds);
            elements.AddElement(el);
            return el;
        }


        /// <summary>
        /// Sets the material properties of all the elements with the given values.
        /// </summary>
        /// <param name="E">Modulus of elasticity.</param>
        /// <param name="v">Poisson's ratio.</param>
        public void SetMaterialProperties(double E, double v)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                ((ePlaneStressElement)elements[i]).ModulusOfElasticity = E;
                ((ePlaneStressElement)elements[i]).PoissonRatio = v;
            }
        }

        public void FillAnalysisResult()
        {
            nodes.FillNodalValues(analysis.Result);

            for (int i = 0; i < elements.Count; i++)
            {
                ((ePlaneStressElement)elements[i]).FillAnalysisResults();
            }
        }

        public Vector<double> StressAt(double x, double y)
        {
            ePlaneStressElement el = null;
            bool found = false;
            int n = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                for (int j = 0; j < elements[i].Nodes.Length; j++)
                {
                    if (elements[i].Nodes[j].Coord == new ePoint (x,y))
                    {
                        n = j;
                        found = true;
                        el = (ePlaneStressElement)elements[i];
                        i = elements.Count;
                        break;
                    }
                }
            }

            if (!found)
                throw new Exception("the node is not found");

           return el.StressAt(n);
        }

        public override void PrintNodalValues()
        {
            double [] Xdisp = GetXDisp();
            double [] Ydisp = GetYDisp();
            double [] X = nodes.GetNodalXValues();
            double [] Y = nodes.GetNodalYValues();

            Output.eOuput form = new Output.eOuput(X, Y, Xdisp, Ydisp, 1);
            Application.Run(form);
        }

        private double[] GetXDisp()
        {
            double [] Xdisps = new double [nodes.Count];
            for( int i = 0 ;i<nodes.Count;i++)
            {
                Xdisps[i] = nodes[i].Values[0];
            }
            return Xdisps;
        }
        private double[] GetYDisp()
        {
            double[] Ydisps = new double[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                Ydisps[i] = nodes[i].Values[1];
            }

            return Ydisps;
        }

        public double GetAbsXstress( out int elIndex)
        {
            double temp = 0;
            elIndex = 0;
            ePlaneStressElement p;

            for (int i = 0; i < elements.Count; i++)
            {
                p = (ePlaneStressElement)elements[i];
                for( int j = 0 ; j<p.Stress.Count;j++)
                {
                    if ((Math.Abs(p.Stress[j][0]) > temp))
                    {
                        temp = Math.Abs(p.Stress[j][0]);
                        elIndex = i;
                    }
                }
            }

            return temp;
        }    
    }
}
