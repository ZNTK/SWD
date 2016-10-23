using SWD.Discretization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Services
{
    public static class DiscretizationService
    {
        public static Model.Table Discretize(Model.Table table, int selectedColumnIndex, int sectionCount)
        {
            List<Section> sections = GetSectionList(table, selectedColumnIndex, sectionCount);

            table.Headers.Cells.Add(
                new Model.Cell(
                    table.Headers.Cells[selectedColumnIndex].Value + "_Disc"
                    ));

            foreach(Model.Row row in table.Rows)
            {
                row.Cells.Add(
                    new Model.Cell(
                        GetNumberOfSection(
                            Convert.ToDouble(row.Cells[selectedColumnIndex].Value)
                            , sections).ToString()
                            )
                    );
                
            }


            return table;
        }


        private static List<Section> GetSectionList(Model.Table table, int selectedColumnIndex, int sectionCount)
        {
            double minValue = GetMinValueOfColumn(table, selectedColumnIndex);
            double maxValue = GetMaxValueOfColumn(table, selectedColumnIndex);

            double sectionRange = (maxValue - minValue) / sectionCount;

            List<Section> sections = new List<Section>();

            for(int i = 0 ; i < sectionCount ; i++)
            {
                double minV_tmp = minValue + (i * sectionRange);
                double maxV_tmp = minV_tmp + sectionRange;
                sections.Add(new Section(minV_tmp, maxV_tmp, i));
            }
            sections[sections.Count - 1].MaxValue = maxValue;


            return sections;
        }

        private static int GetNumberOfSection(double value, List<Section> sections)
        {
            foreach(Section section in sections)
            {
                if (value >= section.MinValue && value <= section.MaxValue)
                    return section.Id;
            }

            return -1;
        }

        private static double GetMinValueOfColumn(Model.Table table, int selectedColumnIndex)
        {
            double minValue = 99999999;
            foreach (Model.Row row in table.Rows)
            {
                if (Convert.ToDouble(row.Cells[selectedColumnIndex].Value) < minValue)
                    minValue = Convert.ToDouble(row.Cells[selectedColumnIndex].Value);
            }

            return minValue;
        }

        private static double GetMaxValueOfColumn(Model.Table table, int selectedColumnIndex)
        {
            double maxValue = -9999999;
            foreach (Model.Row row in table.Rows)
            {
                if (Convert.ToDouble(row.Cells[selectedColumnIndex].Value) > maxValue)
                    maxValue = Convert.ToDouble(row.Cells[selectedColumnIndex].Value);
            }

            return maxValue;
        }

    }
}
