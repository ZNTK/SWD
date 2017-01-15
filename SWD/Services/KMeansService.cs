using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.KMeans.Model;
using System.Windows;

namespace SWD.Services
{
    public static class KMeansService
    {
        public static Model.Table Group(Model.Table table, int decisionalColumnIndex, int numberOfGroups)
        {
            if((table.Rows.Count() - 1) != decisionalColumnIndex)
            {
                //zamienic kolejnosc kolumn, potem sie zrobi xd
            }

            Random random = new Random();
            List<Model.Row> means = new List<Model.Row>();
            for (int i = 0; i < numberOfGroups; i++)
            {
                Model.Row newRandomValue = GetRandomValue(table, random);

                while (CheckIfMeanExists(newRandomValue, means))
                    newRandomValue = GetRandomValue(table, random);

                means.Add(newRandomValue);
              //  System.Threading.Thread.Sleep(217);
            }
            //Model.Row row1 = new Model.Row();
            //    row1.Cells.Add(new Model.Cell("5"));
            //    row1.Cells.Add(new Model.Cell("6"));
            //Model.Row row2 = new Model.Row();
            //    row2.Cells.Add(new Model.Cell("2"));
            //    row2.Cells.Add(new Model.Cell("7"));
            //Model.Row row3 = new Model.Row();
            //    row3.Cells.Add(new Model.Cell("1"));
            //    row3.Cells.Add(new Model.Cell("3"));
            //Model.Row row4 = new Model.Row();
            //    row4.Cells.Add(new Model.Cell("4"));
            //    row4.Cells.Add(new Model.Cell("3"));

            //means.Add(row1); means.Add(row2); means.Add(row3); means.Add(row4);

            List<RowClass> rowClasses = new List<RowClass>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                rowClasses.Add(GetRowClass(table.Rows[i], i, means));
            }


            List<RowClass> rowClasses_prev = new List<RowClass>();

            int safetyCounter = 0;
             while (safetyCounter < 1000 || (CheckIfMeansChanged(rowClasses_prev, rowClasses)))
            //while (safetyCounter < 1000)
                {
                rowClasses_prev = rowClasses;
                

                for (int i = 0; i < means.Count; i++)
                {
                    means[i] = CalculateNewMean(i, rowClasses, table);
                }

                rowClasses.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    rowClasses.Add(GetRowClass(table.Rows[i], i, means));
                }


                safetyCounter++;
            }




            table.Headers.Cells.Add(
               new Model.Cell(
                   "Grouped"
                   ));

            for(int i = 0;i<table.Rows.Count;i++)
            //foreach (Model.Row row in table.Rows)
            {
                table.Rows[i].Cells.Add(
                    new Model.Cell(
                        rowClasses[i].ClassIndex.ToString()
                            )
                    );

            }


            return table;



        }

        private static Model.Row GetRandomValue(Model.Table table, Random random)
        {
            Model.Row randomRow = new Model.Row();

           // Random random = new Random();
            for(int i = 0; i< table.Headers.Cells.Count - 1; i++)
            {
                int randomRowIndex = random.Next(table.Rows.Count());
                randomRow.Cells.Add(new Model.Cell(table.Rows[randomRowIndex].Cells[i].Value));
 
            }


            return randomRow;
        }

        private static RowClass GetRowClass(Model.Row row, int rowIndex, List<Model.Row> means)
        {
            RowClass rowClass = new RowClass();
            rowClass.RowIndex = rowIndex;
            rowClass.ClassIndex = 0;

            double currentDistance = GetInfinityDistance(row, means[0]);
            for (int i = 1; i < means.Count; i++)
            {
                double distance = GetInfinityDistance(row, means[i]);
                if (distance < currentDistance)
                {
                    rowClass.ClassIndex = i;
                    currentDistance = distance;
                }
                    
            }


            return rowClass;
        }

        private static Model.Row CalculateNewMean(int classIndex, List<RowClass> rowClasses, Model.Table table)
        {
            Model.Row newMean = new Model.Row();

            for (int i = 0; i < table.Rows[0].Cells.Count - 1; i++)
            {
                double sum = 0;
                double counter = 0;

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    if(rowClasses[j].ClassIndex == classIndex)
                    {
                        sum += double.Parse(table.Rows[j].Cells[i].Value);
                        counter++;
                    }
                }

                double cellMean = sum / counter;
                newMean.Cells.Add(new Model.Cell(cellMean.ToString()));

            }

             foreach (RowClass rowClass in rowClasses)
            {

            }


            return newMean;
        }

        private static bool CheckIfMeansChanged(List<RowClass> rowClasses_prev, List<RowClass> rowClasses)
        {
            if (rowClasses_prev.Count == 0)
                return true;

            for (int i = 0; i < rowClasses.Count; i++)
            {
                if (rowClasses[i].ClassIndex != rowClasses_prev[i].ClassIndex)
                    return true;
            }

            return false;
        }

        private static bool CheckIfMeanExists(Model.Row newRandomValue, List<Model.Row> means)
        {
            if (means.Count == 0)
                return false;

            foreach(Model.Row mean in means)
            {
                bool allCellsAreTheSame = true;
                for(int i = 0; i<newRandomValue.Cells.Count;i++)
                {
                    if (newRandomValue.Cells[i].Value != mean.Cells[i].Value)
                    {
                        allCellsAreTheSame = false;
                        break;
                    }
                }

                if (allCellsAreTheSame)
                    return true;
            }



            return false;
        }



        //public static double GetEuclideanDistance(double pointOneX, double pointOneY, double pointTwoX, double pointTwoY)
        //{

        //    return Math.Sqrt((Math.Pow(pointOneX - pointTwoX, 2) + (Math.Pow(pointOneY - pointTwoY, 2))));
        //}

        private static double GetEuclidesDistance(Model.Row rowA, Model.Row rowB)
        {
            double sum = 0;
            for (int i = 0; i < rowA.Cells.Count - 1; i++)
                sum += Math.Pow(double.Parse(rowA.Cells[i].Value) - double.Parse(rowB.Cells[i].Value), 2);

            return Math.Sqrt(sum);
        }

        public static double GetManhattanDistance(Model.Row rowA, Model.Row rowB)
        {
            double sum = 0;
            for (int i = 0; i < rowA.Cells.Count - 1; i++)
                sum += Math.Abs(double.Parse(rowA.Cells[i].Value) - double.Parse(rowB.Cells[i].Value));

            return sum;
        }

        public static double GetInfinityDistance(Model.Row rowA, Model.Row rowB) // chebyshev distance // odległość czebyszewa
        {
            double result = Math.Abs(double.Parse(rowA.Cells[0].Value) - double.Parse(rowB.Cells[0].Value));

            for(int i = 1; i < rowA.Cells.Count - 1 ; i++)
            {
                double distance_i = Math.Abs(double.Parse(rowA.Cells[i].Value) - double.Parse(rowB.Cells[i].Value));
                if (distance_i > result)
                    result = distance_i;
            }

            return result;
        }

    }
}
