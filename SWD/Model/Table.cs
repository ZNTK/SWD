using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Model
{
    public class Table
    {
        public Table()
        {
            Rows = new List<Row>();
        }
        public List<Row> Rows { get; set; }
    }
}
