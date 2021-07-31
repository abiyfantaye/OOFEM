using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using FEM.Mathematics;

namespace FEM
{
    /// <summary>
    /// Collection class which holds all the boundary conditions in the model. 
    /// </summary>
    public class eBCs 
    {

        /// <summary>
        /// Holds the value for all boundary conditions. 
        /// </summary>
        private List<eBC> bcs;

        /// <summary>
        /// Creates instance of new collection class which store boundary conditions.
        /// </summary>
        public eBCs()
        {
            this.bcs = new List<eBC>();
        }

        /// <summary>
        /// Adds new boundary condition to the collection.
        /// </summary>
        /// <param name="bc">Boundary Condition to be added.</param>
        /// <returns>Returns the boundary condition added.</returns>
        public eBC AddBC(eBC bc)
        {
            bc.Name = (Count + 1).ToString();
            if (bc.BCType == eBCType.Essential)
                this.bcs.Add(bc);
            else
                this.bcs.Insert(GetLastNaturalBC() + 1, bc);
            return bc;
        }

        /// <summary>
        /// Adds Boundary Condition at one particular point.
        /// </summary>
        /// <param name="Point">Coordinate of the point where the BC is sepcified.</param>
        /// <param name="DOFIndex">Degree of freedom on which the BC is specified.</param>
        /// <param name="Value">The value of the boundary condition.</param>
        /// <returns>Returns the added BC.</returns>
        public ePointBC AddPointBC(ePoint Point, int DOFIndex, double Value)
        {
            ePointBC bc = new ePointBC(Point, DOFIndex, Value);
            AddBC(bc);
            return bc;
        }

        /// <summary>
        /// Adds Boundary Condition at one particular point.
        /// </summary>
        /// <param name="Point">Coordinate of the point where the BC is sepcified.</param>
        /// <param name="DOFIndex">Degree of freedom on which the BC is specified.</param>
        /// <param name="Value">The value of the boundary condition.</param>
        /// <returns>Returns the added BC.</returns>
        public ePointBC AddPointBC(ePoint Point, int DOFIndex, double Value, eBCType Type)
        {
            ePointBC bc = new ePointBC(Point, DOFIndex, Value,Type);
            AddBC(bc);
            return bc;
        }

        /// <summary>
        /// Adds Line boundary condition to the collection.
        /// </summary>
        /// <param name="Points">Coordinate of the point where the BC is sepcified.</param>
        /// <param name="DOFIndex">Degree of freedom on which the BC is specified.</param>
        /// <param name="Func">The function which represetn the variation of BC.</param>
        /// <param name="BCType">The type of bounary condition(Essential or Natural).</param>
        /// <returns>Returns the added BC.</returns>
        public eLineBC AddLineBC(ePoint[] Points, int DOFIndex, eFunction Func, eBCType BCType = eBCType.Essential)
        {
            eLineBC bc = new eLineBC(Points, DOFIndex, BCType, Func);
            AddBC(bc);
            return bc;
        }

        /// <summary>
        /// Adds Line boundary condition to the collection.
        /// </summary>
        /// <param name="Points">Coordinate of the point where the BC is sepcified.</param>
        /// <param name="DOFIndex">Degree of freedom on which the BC is specified.</param>
        /// <param name="Value">The value of the BC.</param>
        /// <param name="BCType">The type of bounary condition(Essential or Natural).</param>
        /// <returns>Returns the added BC.</returns>
        public eLineBC AddLineBC(ePoint[] Points, int DOFIndex, double Value, eBCType BCType = eBCType.Essential)
        {
            eLineBC bc = new eLineBC(Points, DOFIndex, BCType, new eConst(Value));
            AddBC(bc);
            return bc;
        }

        /// <summary>
        /// Adds Line boundary on the line connectin the given two points.
        /// </summary>
        /// <param name="p1">Coordinate of the point where the BC is sepcified.</param>
        /// <param name="DOFIndex">Degree of freedom on which the BC is specified.</param>
        /// <param name="Func">The function which represetn the variation of BC.</param>
        /// <param name="BCType">The type of bounary condition(Essential or Natural).</param>
        /// <returns>Returns the added BC.</returns>
        public eLineBC AddLineBC(ePoint p1, ePoint p2, int DOFIndex, eFunction Func, eBCType BCType = eBCType.Essential)
        {
            eLineBC bc = new eLineBC(new ePoint[2] { p1, p2 }, DOFIndex, BCType, Func);
            AddBC(bc);
            return bc;
        }

        /// <summary>
        /// Adds Line boundary on the line connectin the given two points.
        /// </summary>
        /// <param name="p1">Coordinate of the point where the BC is sepcified.</param>
        /// <param name="DOFIndex">Degree of freedom on which the BC is specified.</param>
        /// <param name="Value">The value of the BC.</param>
        /// <param name="BCType">The type of bounary condition(Essential or Natural).</param>
        /// <returns>Returns the added BC.</returns>
        public eLineBC AddLineBC(ePoint p1, ePoint p2, int DOFIndex, double Value, eBCType BCType = eBCType.Essential)
        {
            eLineBC bc = new eLineBC(new ePoint[2] { p1, p2 }, DOFIndex, BCType, new eConst(Value));
            AddBC(bc);
            return bc;
        }

        /// <summary>
        /// Attaches Nodes to each boundary condition in the collection.
        /// Nodes specifiey the where the boundary condition should be applied.
        /// </summary>
        /// <param name="nodes">List of nodes in the model.</param>
        public void AttachNodesToBCs(eNodes nodes)
        {
            foreach (eBC bc in bcs)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (bc.Contain(nodes[i].Coord))
                    {
                        bc.Nodes.Add(nodes[i]);
                        if (bc.GetType() == typeof(ePointBC))
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the boundary conditions for all BC in the Collection.
        /// </summary>
        public void CalculateBC()
        {
            foreach (eBC bc in bcs)
                bc.CalculateNodalValues();
        }

        /// <summary>
        /// Returns total number of boundary condition in the collection. 
        /// </summary>
        public int Count
        {
            get
            {
                return bcs.Count;
            }
        }

        /// <summary>
        /// Retursn the boundary condition at the sepcified index.
        /// </summary>
        /// <param name="index">Index where the boundary condition is found.</param>
        /// <returns></returns>
        public eBC this[int index]
        {
            get
            {
                return bcs[index];
            }
            set
            {
                bcs[index] = value;
            }
        }

        /// <summary>
        /// Returns the index of the last natural boundary condition in the collection.
        /// </summary>
        /// <returns>Returns the index if found and -1 if not found.</returns>
        private int GetLastNaturalBC()
        {
            int index = -1;
            for (int i = 0; i < Count; i++)
            {
                if (bcs[i].BCType == eBCType.Natural)
                    index = i;
            }
            return index;
        }
       
    }
}
