using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.DecisionTree.Models
{
    public class Leaf
    {
        public Leaf()
        {
            ;
        }

        public Leaf(int argsCount, int leafIndex)
        {
            this.ArgValues = new List<int>();
            this.ClassIndexCounts = new List<int>();
            for (int i = 0; i < argsCount; i++)
                this.ArgValues.Add(new int());
            this.LeafIndex = leafIndex;
        }


        public List<int> ArgValues { get; set; }
        public List<int> ClassIndexCounts { get; set; } 
        public int ClassIndex { get; set; }
        public int LeafIndex { get; set; }
    }
}
