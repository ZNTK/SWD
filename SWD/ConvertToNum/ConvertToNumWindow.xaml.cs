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
            List<string> stringColumn = new List<string>();
            foreach (var cell in mainTable.Rows[comboboxSelectedIndex].Cells)
            {
                stringColumn.Add(cell.Value);
            }

            if(rbAlfabetyczna.IsChecked == true)
            {
                //var sortedTable = mainTable.Rows.OrderBy(x => x.Cells[comboboxSelectedIndex].Value);
                stringColumn.Sort();
                mainTable.Headers.Cells.Add(new Model.Cell(comboBoxColumn.SelectedItem + "_NumValues"));
                foreach(var cell in mainTable.Rows)
                {
                    cell.Cells.Add(new Model.Cell("cos"));
                }
            }
            this.Close();
        }
    }
}
