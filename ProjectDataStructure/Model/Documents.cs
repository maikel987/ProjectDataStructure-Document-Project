using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataStructure.Model
{

    internal class PermutationComparer : IComparer<Permutation>
    {
        public int Compare(Permutation x, Permutation y)
        {

            string xArr = x.permutation;
            string yArr = y.permutation;
            double delta = 0;
            for (int i = 0; i < Math.Min(xArr.Length, yArr.Length); i++)
            {
                if (delta != 0)
                {
                    break;
                }
                else
                {
                    delta += yArr[i].CompareTo(xArr[i]) * Math.Pow(26, Math.Min(xArr.Length, yArr.Length) - i);  
                }
            }
            if (xArr.Length > yArr.Length && delta >= 0)
            {
                delta += " ".CompareTo(yArr[xArr.Length]);
            }
            else if (xArr.Length < yArr.Length && delta >= 0)
            {
                delta += xArr[yArr.Length].CompareTo(" ");
            }

            return (int)delta;
        }
    }

    internal class Document
    {
        internal string mainPermutation;
        internal int countPermutation { get; set; }
        internal BST<Permutation> tree { get; }

        public Document(string text)
        {
            tree = new BST<Permutation>();  // comparer PermComp
            mainPermutation = text;

        }
        
        //return if the Permutation Exist
        internal bool PermExist(string text, out Permutation perm)
        {
            Permutation tmp = new Permutation(text);
            return tree.Search(tmp, out perm);
        }

        //add a new permutation
        public void NewPermutation(string text)
        {
            Permutation perm = new Permutation(text);
            tree.Add(perm);
            countPermutation++;
        }

        //find Closest permutation
        internal Matching FindClosest(string text, out Permutation perm)
        {
            Permutation tmp = new Permutation(text);
            return tree.SearchClosest(tmp, out perm);
        }

    }
}
