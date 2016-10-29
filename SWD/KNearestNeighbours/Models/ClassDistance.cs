using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.KNearestNeighbours.Models
{
    public class ClassDistance
    {
        public string klasa { get; set; }
        public double distance { get; set; }
        public ClassDistance(string klasa, double distance)
        {
            this.klasa = klasa;
            this.distance = distance;
        }
    }
}
