using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Model
{
    public class Cell
    {
        public Cell() { }
        public Cell(string value)
        {
            Value = value;
        }
        public string Value { get; set; }
    }
}
