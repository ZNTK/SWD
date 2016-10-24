using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Services
{
    public static class NormalizationService
    {
        public static Model.Table Normalize(Model.Table table, int selectedColumnIndex)
        {
            List<double> columnValues = GetListOfColumnValues(table, selectedColumnIndex);
            double columnAvarage = columnValues.Average();
            double standardDeviation = CalculateStdDev(columnValues);




            table.Headers.Cells.Add(
                new Model.Cell(
                    table.Headers.Cells[selectedColumnIndex].Value + "_Norm"
                    ));

            foreach (Model.Row row in table.Rows)
            {
                row.Cells.Add(
                    new Model.Cell(
                        GetNormalizedValue(Convert.ToDouble(row.Cells[selectedColumnIndex].Value), columnAvarage, standardDeviation).ToString())
                    );

            }


            //double columnAvarage = GetAvarageOfColumn(table, selectedColumnIndex);


            return table;
        }

        private static List<double> GetListOfColumnValues(Model.Table table, int selectedColumnIndex)
        {
            List<double> column = new List<double>();
            foreach (Model.Row row in table.Rows)
            {
                column.Add(Convert.ToDouble(row.Cells[selectedColumnIndex].Value));
            }

            return column;
        }

        private static double GetNormalizedValue(double value, double columnAvarage, double standardDeviation)
        {
            return ((value - columnAvarage) / standardDeviation);
        }


        private static double GetAvarageOfColumn(Model.Table table, int selectedColumnIndex)
        {
            List<double> column = new List<double>();
            foreach(Model.Row row in table.Rows)
            {
                column.Add(Convert.ToDouble(row.Cells[selectedColumnIndex].Value));
            }

            return Enumerable.Average(column);
        }

        private static double CalculateStdDev(IEnumerable<double> values)
        {
            double ret = 0;
  
            double avg = values.Average();
            //Perform the Sum of (value-avg)_2_2      
            double sum = values.Sum(d => Math.Pow(d - avg, 2));
            //Put it all together      
            ret = Math.Sqrt((sum) / (values.Count() - 1));
            return ret;
        }
    }
}
