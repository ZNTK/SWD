using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.KNearestNeighbours.Models
{
    public class ValuesWithClassDistance
    {
        public List<double> valuesList;
        public string klasa;
        public double distance;
        public ValuesWithClassDistance(List<double> valuesList, string klasa, double distance)
        {
            this.valuesList = valuesList;
            this.klasa = klasa;
            this.distance = distance;
        }
    }
}
