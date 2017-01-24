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
        public Tuple<double, int> EDMethodWhereLine(string pickedClass, List<ValuesWithClass> listValuesWithClass)
        {
            

            int maxValue = -listValuesWithClass.Count;
            Tuple<double, int> rowAndPosition = new Tuple<double, int>(0, 0);

            var valuesList = listValuesWithClass.First().valuesList;
            for (int i = 0; i < valuesList.Count; i++) 
            {
                List<int> tempListWantedItemsCount = new List<int>();
                List<int> tempListUnWantedItemsCount = new List<int>();
                var listValuesWithOneClass = listValuesWithClass.Where(x => x.klasa == pickedClass).OrderBy(x => x.valuesList[i]).ToList();

                foreach (var rowValue in listValuesWithOneClass)
                {
                    var tempPickedClass = listValuesWithClass.Where(x => x.valuesList[i] > rowValue.valuesList[i] && x.klasa == pickedClass).ToList();
                    var tempAnotherClass = listValuesWithClass.Where(x => x.valuesList[i] > rowValue.valuesList[i] && x.klasa != pickedClass).ToList();
                    if (maxValue <= tempPickedClass.Count - tempAnotherClass.Count)
                    {
                        maxValue = tempPickedClass.Count - tempAnotherClass.Count;
                        rowAndPosition = new Tuple<double, int>(rowValue.valuesList[i], i);
                    }
                }
            }
            return rowAndPosition;
        }

        public List<Tuple<double, int>> GetAllLines(int linesCount, string pickedClass, List<ValuesWithClass> listValuesWithClass)
        {
            
            List<Tuple<double, int>> linesPositions = new List<Tuple<double, int>>();
            for(int i = 0; i < linesCount; i++)
            {
                if (listValuesWithClass.Count == 0) break;
                var temp = EDMethodWhereLine(pickedClass, listValuesWithClass);
                if (linesPositions.Contains(temp)) break;
                linesPositions.Add(temp);
                listValuesWithClass = listValuesWithClass.Where(x => x.valuesList[temp.Item2] >= temp.Item1).ToList();
            }
            return linesPositions;
        }

        public List<double> CreateVector(List<Tuple<double, int>> linesPositions, List<ValuesWithClass> listValuesWithClass)
        {
            List<double> listDouble = new List<double>();
            foreach (var item in linesPositions)
            {

            }
            

            return listDouble;
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
