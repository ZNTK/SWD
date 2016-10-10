﻿using SWD.Model;
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

    }
}
