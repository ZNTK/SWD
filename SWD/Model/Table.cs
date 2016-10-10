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
            Headers = new Row();
        }
        public Table(List<Row> allRowsWithHeaders)
        {
            Rows = new List<Row>();
            Headers = new Row();

            this.Headers = (Row)allRowsWithHeaders[0];
            for (int i = 1; i < allRowsWithHeaders.Count(); i++)
                this.Rows.Add(allRowsWithHeaders[i]);
        }
        public List<Row> Rows { get; set; }
        public Row Headers { get; set; }
    }
}
