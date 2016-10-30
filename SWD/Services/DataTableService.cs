using SWD.KNearestNeighbours.Models;
using SWD.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace SWD.Services
{
    public static class DataTableService
    {
        public static Model.Table GetTableFromFile(string filepath, bool firstRowIsHeader, char separator)
        {
            var lines = File.ReadAllLines(filepath);

            var linesList = lines.Where(x => x.StartsWith("#") == false).ToList();            

            var data = (from l in linesList.Skip(0)
                        let split = l.Split(separator)
                        select new Row(split)).ToList();

            Model.Table table = new Table(data, firstRowIsHeader);

            return table;
        }

        public static DataGrid InsertDataToGrid(Model.Table table, DataGrid dataGrid)
        {
            int i = 0;
            foreach (Cell headerCell in table.Headers.Cells)
            {
                Binding binding = new Binding(String.Format("Cells[{0}].Value", i));
                DataGridTextColumn column = new DataGridTextColumn();
                column.Binding = binding;
                column.Header = headerCell.Value;
                dataGrid.Columns.Add(column);

                i++;
            }

            dataGrid.ItemsSource = table.Rows;
            dataGrid.CanUserAddRows = false;
            return dataGrid;
        }

        public static List<string> GetColumFromTableAsList(Model.Table table,int columnNumber)
        {
            List<string> columnList = new List<string>();
            foreach (var row in table.Rows)
            {
                columnList.Add(row.Cells[columnNumber].Value);
            }
            return columnList;
        }

        public static List<ClassPoint> GetColumnFromTableAsClassPointList(Model.Table table, int firstColumnNumber,int secondColumnNumber,int classColumnNumber)
        {
            List<ClassPoint> classPointList = new List<ClassPoint>();
            foreach(var row in table.Rows)
            {
                classPointList.Add(new ClassPoint(row.Cells[classColumnNumber].Value, Convert.ToDouble(row.Cells[firstColumnNumber].Value), Convert.ToDouble(row.Cells[secondColumnNumber].Value)));
            }
            return classPointList;
        }

        public static List<ValuesWithClass> GetColumnsFromTableAsValuesWithClassList(Model.Table table, int classColumnNumber)
        {
            List<ValuesWithClass> valuesWithClass = new List<ValuesWithClass>();
            foreach(var row in table.Rows)
            {
                List<double> tempList = new List<double>();
                int i = 0;
                foreach(var cell in row.Cells)
                {
                    if(i != classColumnNumber)
                    {
                        tempList.Add(Convert.ToDouble(cell.Value));
                    }
                    i++;
                }
                valuesWithClass.Add(new ValuesWithClass(tempList, row.Cells[classColumnNumber].Value));
            }
            return valuesWithClass;
        }

        public static List<string> GetColumnHeadersAsList(Model.Table table)
        {
            List<string> stringTable = new List<string>();
            foreach (var cell in table.Headers.Cells)
            {
                stringTable.Add(cell.Value);
            }
            return stringTable;
        }
    }
}
