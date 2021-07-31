using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESADS.EGraphics;
using MathNet.Numerics.LinearAlgebra;

namespace FEM
{
    public class eElements
    {
        /// <summary>
        /// Holds list of all elements in the list.
        /// </summary>
        private List<eElement> elements;

        /// <summary>
        /// Indexer used to access elements of the collection by their specified index. 
        /// </summary>
        /// <param name="index">Index of the Element.</param>
        /// <returns></returns>
        public eElement this[int index]
        {
            get
            {
                return elements[index];
            }
            set
            {
                elements[index] = value;
            }
        }

        /// <summary>
        /// Creates new instance of Elements collection class. 
        /// </summary>
        public eElements()
        {
            this.elements = new List<eElement>();
        }

        /// <summary>
        /// Adds an Element to the collection, and assigned its name automatically.
        /// </summary>
        /// <param name="Element">The element to be added.</param>
        public eElement AddElement(eElement Element)
        {
            Element.Name = (Count + 1).ToString();
            this.elements.Add(Element);
            return Element;
        }

      

        /// <summary>
        /// Forms element matrix and vector for all elements in the collection.
        /// </summary>
        public void FormElemetStiffnessMatrix()
        {
            foreach (eElement el in elements)
                el.FormElementMatrix();               
        }

        /// <summary>
        /// Form element mass matrix for all elements.
        /// </summary>
        public void FormElementMassMatrix()
        {
            foreach (eElement el in elements)
                el.FormMassMatrix();  
        }

        /// <summary>
        /// Calculates Element vector for all elements in the collection.
        /// </summary>
        public void FormElementVector()
        {
            foreach (eElement el in elements)
                el.FormElementVector();  
        }
  
        /// <summary>
        /// Draw all elements in the collection.
        /// </summary>
        /// <param name="layer">The layer on which the drawing is done.</param>
        public void Draw(eLayer layer)
        {
            foreach (eElement el in elements)
                el.Draw(layer);
        }

        /// <summary>
        /// Gets the total number of elements in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return elements.Count;
            }
        }

        /// <summary>
        /// Removes all Elements in the collection.
        /// </summary>
        public void Reset()
        {
            this.elements = new List<eElement>();         
        }

        /// <summary>
        /// Searches an Element given its name.
        /// </summary>
        /// <param name="Name">Name of the element.</param>
        /// <returns>Returns an element which have the given name.</returns>
        public eElement SearchByName(string Name)
        {
            foreach (eElement el in elements)
            {
                if (el.Name == Name)
                    return el;
            }
            throw new Exception("The required element is not found");
        }

        /// <summary>
        /// Assembles the global element matrix into the given global matrices for all elements.
        /// </summary>
        /// <param name="K">The global stiffness matrix.</param>
        /// <param name="F">The global vector.</param>
        /// <param name="M">The global mass matrix.</param>
        public void AssembleMatrixes(Matrix<double> K, Vector<double> F, Matrix<double> M)
        {
            foreach (eElement el in elements)
                el.Assembele(K, F, M);
        }
    }
}
