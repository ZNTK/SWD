using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SWD.ConvertToNum
{
    /// <summary>
    /// Interaction logic for ConvertToNumWindow.xaml
    /// </summary>
    public partial class ConvertToNumWindow : Window
    {
        public Model.Table mainTable;
        public bool result = false;
        public ConvertToNumWindow(Model.Table table)
        {
            InitializeComponent();
            List<string> stringTable = new List<string>();
            mainTable = table;
            foreach(var cell in table.Headers.Cells)
            {
                stringTable.Add(cell.Value);
            }

            comboBoxColumn.ItemsSource = stringTable;
        }

        private void buttonZamien_Click(object sender, RoutedEventArgs e)
        {
            var comboboxSelectedIndex = comboBoxColumn.SelectedIndex;
            List<string> stringColumn = Services.DataTableService.GetColumFromTableAsList(mainTable, comboboxSelectedIndex);

            var klasyZWartosciami = new List<Tuple<string, int>>();
                                       
            stringColumn = stringColumn.Distinct().ToList();
            if (rbAlfabetyczna.IsChecked == true)
            {
                stringColumn.Sort();
            }
            int i = 0;
            foreach (var row in stringColumn)
            {
                klasyZWartosciami.Add(Tuple.Create(row, i));
                i++;
            }
                           
            mainTable.Headers.Cells.Add(new Model.Cell(comboBoxColumn.SelectedItem + "_NumValues"));
            foreach (var row in mainTable.Rows)
            {
                var nr = klasyZWartosciami.Where(x => x.Item1 == row.Cells[comboboxSelectedIndex].Value).Select(x => x.Item2).FirstOrDefault();
                row.Cells.Add(new Model.Cell(nr.ToString()));
            }
            result = true;
            this.Close();
        }
    }
}
