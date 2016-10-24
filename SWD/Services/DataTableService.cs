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

        public static List<double>  ConvertStingListToDoubleList(List<string> list, string separator)
        {
            List<double> doubleList = new List<double>();
            foreach (var item in list)
            {
                string temp = item;
                if (item.StartsWith(separator))
                {
                    temp = "0" + item;
                }
                doubleList.Add(Convert.ToDouble(temp.Replace(separator.ToCharArray()[0], '.')));
            }
            return doubleList;
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
