using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.ED.Models
{
    public class ClassCount
    {
        public int ClassValue;
        public int Count;

        public ClassCount(int classValue, int count)
        {
            ClassValue = classValue;
            Count = count;
        }

        public void UpdateCount(int count)
        {
            Count = count;
        }
    }
}
