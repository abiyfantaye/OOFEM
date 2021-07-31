using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace FEM
{
    /// <summary>
    /// Interface used for transient analysis of models.
    /// </summary>
    public class eTransientAnalysis : eAnalysis
    {
        /// <summary>
        /// Holds the mass matrix of the model for transient problems.
        /// </summary>
        private Matrix<double> M;
        /// <summary>
        /// Holds initial condition for time depedent problems.
        /// </summary>
        private eInitialCondition IC;
        /// <summary>
        /// Holds the time step used in the Transient Anaysis.
        /// </summary>
        private double timeStep;
        /// <summary>
        /// Holds the end time of the analysis.
        /// </summary>
        private double endTime;
        /// <summary>
        /// Holds the time integration technique to be used.
        /// </summary>
        private eTimeIntegrationTechnique technique;
        /// <summary>
        /// The point where time-history curve is needed.
        /// </summary>
        private ePoint point;
        /// <summary>
        /// Holds the time history of the node at the given point. 
        /// </summary>
        private double[] history;
        /// <summary>
        /// Holds the number of steps for complete analysis.
        /// </summary>
        private int noSteps;
        /// <summary>
        /// The node where the time history corve is needed.
        /// </summary>
        private eNode node;
        /// <summary>
        /// Holds series of times(t) where the analysis is carried out.
        /// </summary>
        private double[] time;
        /// <summary>
        /// History of acceleration.
        /// </summary>
        private List<Vector<double>> a;
        /// <summary>
        /// History of velocity
        /// </summary>
        private List<Vector<double>> v;
        /// <summary>
        /// History of displacement.
        /// </summary>
        private List<Vector<double>> d;
        /// <summary>
        /// Holds the type of the problem.
        /// </summary>
        private eProblemType problemType;

        /// <summary>
        /// Performs transient analysis for a given model.
        /// </summary>
        /// <param name="Model">The model on which the analysis is going to be done.</param>
        /// <param name="probType">Type of the problem to be solved(Structural or Thermal).</param>
        public eTransientAnalysis(eModel Model)
            : base(Model)
        {
            this.endTime = 10;
            this.timeStep = 0.1;
            this.technique = eTimeIntegrationTechnique.ForwardDifference;
            this.problemType = eProblemType.Structural;
        }

        /// <summary>
        /// Gets or sets the type of the problem to be solved.
        /// Structural or Heat transfer.
        /// </summary>
        public eProblemType ProblemType
        {
            get
            {
                return problemType;
            }

            set
            {
                problemType = value;
            }
        }

        /// <summary>
        /// Gets or sets the Mass matrix used for transient analysis.
        /// </summary>
        public Matrix<double> MassMatrix
        {
            get { return M; }
            set { M = value; }
        }

        /// <summary>
        /// Gets or sets the time step used used for transient analysis.
        /// </summary>
        public double TimeStep
        {
            get { return timeStep; }
            set { timeStep = value; }
        }

        /// <summary>
        /// Gets or sets the type of time integration technique used.
        /// </summary>
        public eTimeIntegrationTechnique Technique
        {
            get
            {
                return technique;
            }
            set
            {
                technique = value;
            }
        }

        /// <summary>
        /// Gets series of times where the analysis is carried out.
        /// </summary>
        public double[] Time
        {
            get
            {
                return time;
            }
        }

        /// <summary>
        /// Gets the History of specific nodal value at all point in the time series.
        /// </summary>
        public double[] History
        {
            get
            {
                return history;
            }
        }

        /// <summary>
        /// Gets or sets the end time for the analysis.
        /// </summary>
        public double EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                endTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the point where the time history curve is needed.
        /// </summary>
        public ePoint Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
            }
        }

        /// <summary>
        /// Gets or sets the time depedent initial condition for the model.
        /// </summary>
        public eInitialCondition InitialCondition
        {
            get
            {
                return IC;
            }

            set
            {
                IC = value;
            }
        }

        public List<Vector<double>> Acc
        {
            get
            {
                return a;
            }
        }
        public List<Vector<double>> Disps
        {
            get
            {
                return d;
            }
        }
        public List<Vector<double>> Velocities
        {
            get
            {
                return v;
            }
        }

        public override void RunAnalysis()
        {

            Assemble();
            if (problemType == eProblemType.HeatTransfer)
            {
                InitializeIC();

                if (technique == eTimeIntegrationTechnique.ForwardDifference)
                    SolveByForwardDifference();
                else if (technique == eTimeIntegrationTechnique.BackwardDifference)
                    SolveByBackwardDifference();
                else if (technique == eTimeIntegrationTechnique.CrankNicolson)
                    SolveByCrankNicolson();
            }
            else
            {           
                noSteps = (int)((endTime - IC.StartTime) / timeStep);
                model.BCs.AttachNodesToBCs(model.Nodes);
                model.BCs.CalculateBC();
                ApplyF_BC(F);
                SolveByCentralDifference();
            }
            
        }

        protected override void ApplyBC(Matrix<double> K, Vector<double> F)
        {
            eBC bc;
            //Apply BC to each rows of the system matrix.
            for (int i = 0; i < model.BCs.Count; i++)
            {
                bc = model.BCs[i];
                for (int j = 0; j < bc.Nodes.Count; j++)
                {
                    if (bc.BCType == eBCType.Natural)
                        F[bc.GetDOF(j)] += bc.NodalValues[j];

                    else if (bc.BCType == eBCType.Essential)
                    {
                        K.ClearRow(bc.Nodes[j].DOFs[bc.DOFIndex]);
                        K[bc.GetDOF(j), bc.GetDOF(j)] = 1.0;
                        F[bc.GetDOF(j)] = bc.NodalValues[j];
                    }
                }
            }
        }

        protected void ApplyF_BC(Vector<double> F)
        {
            eBC bc;
            //Apply BC to each rows of the system matrix.
            for (int i = 0; i < model.BCs.Count; i++)
            {
                bc = model.BCs[i];
                for (int j = 0; j < bc.Nodes.Count; j++)
                {
                    if (bc.BCType == eBCType.Natural)
                        F[bc.GetDOF(j)] += bc.NodalValues[j];

                    else if (bc.BCType == eBCType.Essential)
                    {
                        F[bc.GetDOF(j)] = bc.NodalValues[j];
                    }
                }
            }
        }

        protected void ApplyACC_BC(Vector<double> ACC)
        {
            eBC bc;
            //Apply BC to each rows of the system matrix.
            for (int i = 0; i < model.BCs.Count; i++)
            {
                bc = model.BCs[i];
                for (int j = 0; j < bc.Nodes.Count; j++)
                {
                    if (bc.BCType == eBCType.Essential)
                    {
                        ACC[bc.GetDOF(j)] = bc.NodalValues[j];
                    }
                }
            }
        }


        protected override void Assemble()
        {
            //Calculates element matrix and vector for all elements.
            model.Elements.FormElemetStiffnessMatrix();
            model.Elements.FormElementMassMatrix();
            model.Elements.FormElementVector();
            model.Nodes.SetSysIndexToDOFs();

            K = new DenseMatrix(model.Nodes.GetTotalDOFs());
            M = new DenseMatrix(K.RowCount);
            F = new DenseVector(K.RowCount);

            model.Elements.AssembleMatrixes(K, F, M);
        }

        private void SolveByForwardDifference()
        {
            Vector<double> FF;

            for (int i = 0; i < noSteps; i++)
            {
                FF = timeStep * F + (M - timeStep * K) * result;
                ApplyBC(M, FF);
                result = M.Solve(FF);
                time[i + 1] = time[i] + timeStep;
                history[i + 1] = result[node.DOFs[IC.DOFIndex]];
            }
        }

        private void SolveByBackwardDifference()
        {
            Vector<double> FF;
            Matrix<double> tMK = M + timeStep * K;

            for (int i = 0; i < noSteps; i++)
            {
                FF = timeStep * F + M * result;
                ApplyBC(tMK, FF);
                result = tMK.Solve(FF);
                time[i + 1] = time[i] + timeStep;
                history[i + 1] = result[node.DOFs[IC.DOFIndex]];
            }
        }

        private void SolveByCrankNicolson()
        {
            Vector<double> FF;
            Matrix<double> tMK = 2 * M + timeStep * K;

            for (int i = 0; i < noSteps; i++)
            {
                FF = timeStep * 2 * F + (2 * M - timeStep * K) * result;
                ApplyBC(tMK, FF);
                result = tMK.Solve(FF);
                time[i + 1] = time[i] + timeStep;
                history[i + 1] = result[node.DOFs[IC.DOFIndex]];
            }
        }

        private void InitializeIC()
        {
            
            noSteps = (int)((endTime - IC.StartTime) / timeStep);
            history = new double[noSteps+1];
            time = new double[noSteps + 1];
            result = DenseVector.Create(K.ColumnCount, IC.Value);
            node = model.Nodes.FindNode(point);

            //Attaches DOF objects to boundary condition on which they are applied. 
            model.BCs.AttachNodesToBCs(model.Nodes);
            model.BCs.CalculateBC();
            time[0] = IC.StartTime;
            history[0] = result[node.DOFs[IC.DOFIndex]];
        }

        public void GetTimSeries()
        {
        }

        /// <summary>
        /// Solves forced undamped transient structural problem using central difference method.
        /// </summary>
        public void SolveByCentralDifference()
        {
            a = new List<Vector<double>>(noSteps);
            v = new List<Vector<double>>(noSteps);
            d = new List<Vector<double>>(noSteps);

            d.Add(new DenseVector(F.Count));
            v.Add(new DenseVector(F.Count));
            a.Add( M.Inverse() * (F - K * d.Last()));
            ApplyACC_BC(a.Last());

            Vector<double> dfic = d.Last() - timeStep * v.Last() - 0.5 * timeStep * timeStep * a.Last();
            Vector<double> vfic = (d.Last() - dfic) / timeStep;

            v.Add(vfic + timeStep * a.Last());

            for (int i = 0; i < noSteps; i++)
            {
                a.Add(M.Inverse() * (F - K * d.Last()));
                ApplyACC_BC(a.Last());
                v.Add(v.Last() + timeStep * a.Last());
                d.Add(d.Last() + timeStep * v.Last());
            }
        }
    }
}
