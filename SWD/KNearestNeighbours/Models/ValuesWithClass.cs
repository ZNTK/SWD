using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.KNearestNeighbours.Models
{
    public class ValuesWithClass
    {
        public List<double> valuesList;
        public string klasa;
        public ValuesWithClass(List<double> valuesList, string klasa)
        {
            this.valuesList = valuesList;
            this.klasa = klasa;
        }
    }
}
