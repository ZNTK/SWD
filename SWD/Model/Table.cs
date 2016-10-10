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

        public Table(List<Row> allRows, bool firstRowIsHeader)
        {
            Rows = new List<Row>();
            Headers = new Row();

            if(firstRowIsHeader)
            {
                this.Headers = (Row)allRows[0];
                for (int i = 1; i < allRows.Count(); i++)
                    this.Rows.Add(allRows[i]);
            }
            else
            {
                int columnCount = allRows[0].Cells.Count();
                for (int i = 0; i < columnCount; i++)
                {
                    this.Headers.Cells.Add(new Cell("Var " + i.ToString()));
                }

                for (int i = 0; i < allRows.Count(); i++)
                    this.Rows.Add(allRows[i]);
            }

            
        }

        public List<Row> Rows { get; set; }
        public Row Headers { get; set; }
    }
}
