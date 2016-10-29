using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.KNearestNeighbours.Models
{
    public class ClassPointDistance
    {
        public string klasa { get; set; }
        public double columnX { get; set; }
        public double columnY { get; set; }
        public double distance { get; set; }
        public ClassPointDistance(string klasa, double columnX, double columnY, double distance)
        {
            this.klasa = klasa;
            this.columnX = columnX;
            this.columnY = columnY;
            this.distance = distance;
        }
    }
}
