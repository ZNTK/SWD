using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.KMeans.Model
{
    public class RowClass
    {
        public int RowIndex;
        public int ClassIndex;

        public RowClass(int RowIndex, int ClassIndex)
        {
            this.RowIndex = RowIndex;
            this.ClassIndex = ClassIndex;
        }

        public RowClass()
        { }
    }
}
