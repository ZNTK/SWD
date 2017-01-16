using SWD.ED.Models;
using SWD.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Services
{
    public static class EDService
    {
        public static void Separate(Model.Table table)
        {
            
        }

        private static void SeparateByColumn(Model.Table table, int columnIndex)
        {
            List<ClassCount> classCounts = GetClasses(table);

            double maxValue = GetMaxValue(table, columnIndex);
            double minValue = GetMinValue(table, columnIndex);

            for (double i = maxValue; i >= maxValue; i = i - 0.000000001)
            {
                foreach (Model.Row row in table.Rows)
                {
                    if (double.Parse(row.Cells[columnIndex].Value) > i)
                        classCounts.Where(c => c.ClassValue == int.Parse(row.Cells.Last().Value)).SingleOrDefault().Count++;
                }

               // if()
            }
        }

        //private static List<ClassCount> ClassCount(double lineLevel, int columnIndex)
        //{
           
        //}

        private static double GetMaxValue(Model.Table table, int columnIndex)
        {
            double maxValue = double.MinValue;
            foreach(Model.Row row in table.Rows)
            {
                if (double.Parse(row.Cells[columnIndex].Value) > maxValue)
                    maxValue = double.Parse(row.Cells[columnIndex].Value);
            }

            return maxValue;
        }

        private static double GetMinValue(Model.Table table, int columnIndex)
        {
            double minValue = double.MaxValue;
            foreach (Model.Row row in table.Rows)
            {
                if (double.Parse(row.Cells[columnIndex].Value) < minValue)
                    minValue = double.Parse(row.Cells[columnIndex].Value);
            }

            return minValue;
        }

        private static List<ClassCount> GetClasses(Model.Table table)
        {
            List<ClassCount> classCounts = new List<ClassCount>();

            foreach(Model.Row row in table.Rows)
            {
                if (!classCounts.Where(c => c.ClassValue == int.Parse(row.Cells[2].Value)).Any())
                    classCounts.Add(new ClassCount(int.Parse(row.Cells.Last().Value), 0));
            }

            return classCounts;
        }
    }
}
