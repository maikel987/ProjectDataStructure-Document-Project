using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectDataStructure.Model
{
    internal class Permutation : IComparable<Permutation>
    {
        internal string permutation;
        internal int count { get; private set; }
        internal TimeData timeD;     //Queue<DateTime>.Node
        ProjectDataStructure.Queue<TimeData>.Node node;

        public Permutation(string text)
        {
            permutation = text;
            count = 1;
            timeD = new TimeData(text);

            DocManagement.DateQueue.Enqueue(timeD, out node);
        }


        public int CompareTo(Permutation other)
        {
            return permutation.CompareTo(other.permutation);
        }

        //update the permutation when it apear again
        public void Update()
        {
            count++;
            DocManagement.DateQueue.Delate(node, out node);
            timeD = new TimeData(permutation);
            DocManagement.DateQueue.Enqueue(timeD, out node);
        }
    }
}
