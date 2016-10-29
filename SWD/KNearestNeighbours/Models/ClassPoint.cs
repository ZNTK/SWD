using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.KNearestNeighbours.Models
{
    public class ClassPoint
    {
        public string klasa { get; set; }
        public double columnX { get; set; }
        public double columnY { get; set; }
        public ClassPoint(string klasa, double columnX, double columnY)
        {
            this.klasa = klasa;
            this.columnX = columnX;
            this.columnY = columnY;
        }
    }
}
