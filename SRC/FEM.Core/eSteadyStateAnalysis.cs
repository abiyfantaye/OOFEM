using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using System.Windows.Forms;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra.Solvers;

namespace FEM
{
    public class eSteadyStateAnalysis : eAnalysis
    {

        public eSteadyStateAnalysis(eModel Model)
            : base(Model)
        {

        }
        public override void RunAnalysis()
        {

            Stopwatch t = new Stopwatch();
            
            Assemble();
            ApplyBC(K, F);
            t.Start();
            result = K.Solve(F);
            t.Stop();
            FillNodalFlux();
            analysisCompleted = true;
            excutionTime = (double)t.ElapsedMilliseconds;
        }

        protected override void ApplyBC(Matrix<double>K, Vector<double> F)
        {
            //Attaches DOF objects to boundary condition on which they are applied. 
            model.BCs.AttachNodesToBCs(model.Nodes);
            model.BCs.CalculateBC();
            eBC bc;
            //Apply BC to each rows of the system matrix.
            for (int i = 0; i < model.BCs.Count; i++)
            {
                 bc = model.BCs[i];
                for (int j = 0; j < bc.Nodes.Count; j++)
                {
                    if (bc.BCType == eBCType.Natural)
                        F[bc.Nodes[j].DOFs[bc.DOFIndex]] += bc.NodalValues[j];

                    else if (bc.BCType == eBCType.Essential)
                    {
                        K.ClearRow(bc.Nodes[j].DOFs[bc.DOFIndex]);
                        K[bc.Nodes[j].DOFs[bc.DOFIndex], bc.Nodes[j].DOFs[bc.DOFIndex]] = 1.0;
                        F[bc.Nodes[j].DOFs[bc.DOFIndex]] = bc.NodalValues[j];
                    }
                }
            }
        }

        protected override void Assemble()
        {
            //Calculates element matrix and vector for all elements.
            model.Elements.FormElemetStiffnessMatrix();
            model.Elements.FormElementVector();
            model.Nodes.SetSysIndexToDOFs();

            K = new DenseMatrix(model.Nodes.GetTotalDOFs());
            F = new DenseVector(K.RowCount);

            model.Elements.AssembleMatrixes(K, F, null);           
        }


    }
}
