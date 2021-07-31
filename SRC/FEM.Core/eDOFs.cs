using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEM
{
    /// <summary>
    /// Collection class which contain DOFs in the node.
    /// </summary>
    public class eDOFs
    {
        /// <summary>
        /// Holds the value indicating whether the system index is assigned or not.
        /// </summary>
        private bool sysmIdxAssigned;
        /// <summary>
        /// Holds the system global index for the degree of freedoms in the node.
        /// </summary>
        private int[] indexes;
        /// <summary>
        /// Indexer for the eDOFs collection class.
        /// </summary>
        /// <param name="index">Index to access the required DOF index.</param>
        /// <returns></returns>
        public int this[int index]
        {
            get
            {
                return indexes[index];
            }
            set
            {
                indexes[index] = value;
            }
        }

        /// <summary>
        /// Creates new instance of collection class which contain all DOFs in a node.
        /// </summary>
        /// <param name="DOFsPerNode">Number of degree of freedom per node.</param>
        public eDOFs(int DOFsPerNode)
        {
            this.indexes = new int[DOFsPerNode];
            this.sysmIdxAssigned = false;
        }

          /// <summary>
        /// Sets the system index used for Dof.
        /// </summary>
        /// <param name="Index">System index to start with.</param>
        public void SetSysIndex(ref int Index)
        {

            for (int i = 0; i < indexes.Length; i++)
            {
                indexes[i]= Index;
                Index++;
            }

            sysmIdxAssigned = true;

        }

        /// <summary>
        /// Gets the index of the degree of freedom. 
        /// </summary>
        public int [] Indexes
        {
            get
            {
                return indexes;
            }
        }

        /// <summary>
        /// Gets the total number of DOF in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return indexes.Length;
            }
        }

        /// <summary>
        /// Gets the value indicating wether the system index is assigned or not.
        /// </summary>
        public bool SystemIndexAssigned
        {
            get { return sysmIdxAssigned;}
        }

        public int GetIndex(int index)
        {
            return indexes[index];
        }
       
    }
}
