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
        private static int percentageThreshold = 5;

        public static Model.Table Separate(Model.Table table)
        {
            Model.Table mainTable = DecisionTreeService.CopyTable(table);
            List<SeparationResult> separationResults = new List<SeparationResult>();

            while(!CheckIfAlgorithmIsFinished(table))
            {
                SeparationResult separationResultVertical = SeparateByColumn(table, 0);
                SeparationResult separationResultHorizontal = SeparateByColumn(table, 1);

                SeparationResult separationResult = new SeparationResult();
                if (separationResultVertical.Count > separationResultHorizontal.Count)
                {
                    separationResult.Count = separationResultVertical.Count;
                    separationResult.Value = separationResultVertical.Value;
                    separationResult.LineOrientation = (int)LineOrientation.Vertical;
                }
                else
                {
                    separationResult.Count = separationResultHorizontal.Count;
                    separationResult.Value = separationResultHorizontal.Value;
                    separationResult.LineOrientation = (int)LineOrientation.Horizontal;
                }

                mainTable = ExtendVector(mainTable, separationResult);
                table = RemoveObjectsByLine(table, separationResult);
                separationResults.Add(separationResult);
            }

            mainTable = AddColumnWithVector(mainTable);
            mainTable.SeparationResults = separationResults;
            
            return mainTable;
        }

        private static SeparationResult SeparateByColumn(Model.Table table, int columnIndex)
        {
            SeparationResult separationResult = new SeparationResult();

            List<ClassCount> classCounts = GetClasses(table);

            double maxValue = GetMaxValue(table, columnIndex);
            double minValue = GetMinValue(table, columnIndex);

            List<double> distinctValues = GetDistinctValues(table, columnIndex);

            // for (double i = maxValue; i >= minValue; i = i - 0.000000001)
            for (double i = maxValue; i >= minValue; i = i - 0.01)
            {
                foreach (ClassCount classCount in classCounts)
                    classCount.Count = 0;

                foreach (Model.Row row in table.Rows)
                {
                    if (double.Parse(row.Cells[columnIndex].Value) > i)
                        classCounts.Where(c => c.ClassValue == int.Parse(row.Cells.Last().Value)).SingleOrDefault().Count++;
                }

                if (!CheckClassCounts(classCounts, percentageThreshold))
                {
                    int maxCount = 0;


                    foreach (ClassCount classCount in classCounts)
                    {
                        if (classCount.Count > maxCount)
                            maxCount = classCount.Count;
                    }

                    separationResult.Count = maxCount;
                    separationResult.Value = i + 0.01;
                    break;
                }
                   
            }

            return separationResult;
        }

        private static bool CheckClassCounts(List<ClassCount> classCounts, int percentageThreshold)
        {
            int maxCount = 0;
            int maxCountClassValue = 0;

            foreach (ClassCount classCount in classCounts)
            {
                if(classCount.Count > maxCount)
                {
                    maxCount = classCount.Count;
                    maxCountClassValue = classCount.ClassValue;
                }
            }

            int otherClassCount = 0;
            List<int> otherCounts = classCounts.Where(c => c.ClassValue != maxCountClassValue).Select(c => c.Count).ToList();
            foreach (int count in otherCounts)
                otherClassCount += count;


            double percentageResult = ((double)otherClassCount / (double)(maxCount + otherClassCount)) * 100;


            if (percentageResult > percentageThreshold)
                return false;
            else return true;
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

        private static Model.Table RemoveObjectsByLine(Model.Table table, SeparationResult separationResult)
        {
            // Model.Table tableCopy = DecisionTreeService.CopyTable(table);
            List<int> rowIndexesToRemove = new List<int>();

            if(separationResult.LineOrientation == (int)LineOrientation.Horizontal)
            {
                //oreach(Model.Row row in table.Rows)
                for(int i = 0; i < table.Rows.Count; i++)
                {
                    if (double.Parse(table.Rows[i].Cells[1].Value) > separationResult.Value)
                        rowIndexesToRemove.Add(i);
                }
            }
            else if (separationResult.LineOrientation == (int)LineOrientation.Vertical)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (double.Parse(table.Rows[i].Cells[0].Value) > separationResult.Value)
                        rowIndexesToRemove.Add(i);
                }
            }

            rowIndexesToRemove = rowIndexesToRemove.OrderByDescending(r => r).ToList();

            foreach (int i in rowIndexesToRemove)
                table.Rows.RemoveAt(i);

            return table;
        }

        private static bool CheckIfAlgorithmIsFinished(Model.Table table)
        {
            if (!table.Rows.Any())
                return true;

            int firstClass = int.Parse(table.Rows[0].Cells.Last().Value);
            foreach (Model.Row row in table.Rows)
                if (int.Parse(row.Cells.Last().Value) != firstClass)
                    return false;


            return true;
        }

        private static Model.Table ExtendVector(Model.Table table, SeparationResult separationResult)
        {
            if (separationResult.LineOrientation == (int)LineOrientation.Horizontal)
            {
                foreach(Model.Row row in table.Rows)
                {
                    if (double.Parse(row.Cells[1].Value) > separationResult.Value)
                        row.Vector.Add(1);
                    else row.Vector.Add(0);
                }
            }
            else if (separationResult.LineOrientation == (int)LineOrientation.Vertical)
            {
                foreach (Model.Row row in table.Rows)
                {
                    if (double.Parse(row.Cells[0].Value) > separationResult.Value)
                        row.Vector.Add(1);
                    else row.Vector.Add(0);
                }
            }

            return table;
        }

        private static Model.Table AddColumnWithVector(Model.Table table)
        {
            table.Headers.Cells.Add(new Model.Cell("Vector"));
            table.Headers.Cells.Add(new Model.Cell("Vector Length"));

            foreach (Model.Row row in table.Rows)
            {
                row.Cells.Add(new Model.Cell(GenerateVectorString(row)));
                row.Cells.Add(new Model.Cell(row.Vector.Count().ToString()));
            }


            return table;
        }

        private static string GenerateVectorString(Model.Row row)
        {
            string result = "[";

            foreach (byte vectorValue in row.Vector)
                result += vectorValue.ToString() + ",";

            result = result.Substring(0, result.Length - 1);
            result += "]";

            return result;
        }

        private static List<double> GetDistinctValues(Model.Table table, int columnIndex)
        {
            List<double> distinctValues = new List<double>();

            foreach (Model.Row row in table.Rows)
                if (!distinctValues.Contains(double.Parse(row.Cells[columnIndex].Value)))
                    distinctValues.Add(double.Parse(row.Cells[columnIndex].Value));

            return distinctValues;

        }






    }
}
