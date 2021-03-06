﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Model
{
    public class Row
    {
        public Row(string[] cells)
        {
            Cells = new List<Cell>();
            foreach(string cell in cells)
            {
                Cells.Add(new Cell()
                {
                    Value = cell
                });
            }
        }
        public Row()
        {
            Cells = new List<Cell>();
        }
        public List<Cell> Cells { get; set; }
    }
}
