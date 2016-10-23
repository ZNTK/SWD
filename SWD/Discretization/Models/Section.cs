using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Discretization.Models
{
    public class Section
    {
        public Section(double minValue, double maxValue, int id)
        {
            this.Id = id;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }
        public int Id { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
}
