using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataStructure.Model
{
    internal class TimeData
    {
        internal DateTime dateStamp;
        internal string permutation;
        // + parametre pour acceder plus facilement au 

        internal TimeData(string text)
        {
            permutation = text;
            dateStamp = DateTime.Now;
        }
    }
}
