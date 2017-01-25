using SWD.KNearestNeighbours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Services
{
    public class EDService
    {
        public Tuple<double, int> EDMethodWhereLine( List<ValuesWithClass> listValuesWithClass, double percent)
        {

            int itemsCount = 0;
            Tuple<double, int> rowAndPosition = new Tuple<double, int>(0, 0);

            var valuesList = listValuesWithClass.First().valuesList;
            for (int i = 0; i < valuesList.Count; i++) 
            {
                int itemCountInside = 0;
                int anotherItemCountInside = 0;
                double pointPositionValue = 0;
                var listValuesWithAllClasses = listValuesWithClass.OrderBy(x => x.valuesList[i]).ToList();
                string prvClass = listValuesWithAllClasses.First().klasa;
                var lastElement = listValuesWithAllClasses.Last();
                foreach (var rowValue in listValuesWithAllClasses)
                {
                    if(prvClass != rowValue.klasa)
                    {
                        anotherItemCountInside++;
                        if (percent * itemCountInside < anotherItemCountInside)
                        {
                            pointPositionValue = rowValue.valuesList[i];
                            break;
                        }
                    }
                    else
                    {
                        itemCountInside++;
                        prvClass = rowValue.klasa;
                    }                   
                    if (rowValue.Equals(lastElement))
                    {
                        pointPositionValue = rowValue.valuesList[i];
                        break;
                    }                    
                }
                if(itemsCount < itemCountInside)
                {
                    itemsCount = itemCountInside;
                    rowAndPosition = new Tuple<double, int>(pointPositionValue, i);
                }
            }
            return rowAndPosition;
        }

        public List<Tuple<double, int>> GetAllLines(int linesCount, List<ValuesWithClass> listValuesWithClass,double percent)
        {
            
            List<Tuple<double, int>> linesPositions = new List<Tuple<double, int>>();
            while(listValuesWithClass.Count > 0)
            {
                if (listValuesWithClass.Count == 0) break;
                var temp = EDMethodWhereLine(listValuesWithClass, percent);
                if (linesPositions.Contains(temp)) break;
                linesPositions.Add(temp);
                listValuesWithClass = listValuesWithClass.Where(x => x.valuesList[temp.Item2] >= temp.Item1).ToList();
                if (listValuesWithClass.Count == listValuesWithClass.Where(x => x.klasa == listValuesWithClass.First().klasa).Count()) break;
            }
            return linesPositions;
        }

        public Model.Table AddEDColumn(Model.Table table, List<Tuple<double, int>> linesPositions)
        {
            table.Headers.Cells.Add(
                new Model.Cell(
                    "ED"
                    ));

            foreach (Model.Row row in table.Rows)
            {
                string temp = "[";
                foreach (var item in linesPositions)
                {
                    if(Convert.ToDouble(row.Cells[item.Item2].Value) >= item.Item1)
                    {
                        temp += "1,";
                    }
                    else
                    {
                        temp += "0,";
                    }
                }
                temp = temp.Remove(temp.Length - 1);
                temp += "]";
                row.Cells.Add(
                    new Model.Cell(
                        temp)
                    );

            }
            
            return table;
        }
    }
}
