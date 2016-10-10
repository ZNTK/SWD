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
        public static Model.Table GetTableFromFile(string filepath)
        {
            var lines = File.ReadAllLines(filepath);

            var data = (from l in lines.Skip(0)
                       let split = l.Split(';')
                       select new Row(split)).ToList();

            Model.Table table = new Model.Table();
            table.Rows = (List<Row>)data;

            return table;
        }

        public static DataGrid InsertDataToGrid(Model.Table table, DataGrid dataGrid)
        {
            int i = 0;
            foreach (Cell headerCell in table.Rows[0].Cells)
            {
                Binding binding = new Binding(String.Format("Cells[{0}].Value", i));
                DataGridTextColumn column = new DataGridTextColumn();
                binding.Mode = BindingMode.TwoWay;
                binding.ValidatesOnDataErrors = true;
                column.Binding = binding;
                column.CanUserSort = false;
                // column.Header = headerCell.Value;
                dataGrid.Columns.Add(column);

                i++;
            }

            dataGrid.ItemsSource = table.Rows;
            return dataGrid;
        }
    }
}
