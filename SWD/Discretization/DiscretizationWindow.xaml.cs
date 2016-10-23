using SWD.Services;
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

namespace SWD.Discretization
{
    /// <summary>
    /// Interaction logic for DiscretizationWindow.xaml
    /// </summary>
    public partial class DiscretizationWindow : Window
    {
        public Model.Table mainTable;
        public DiscretizationWindow(Model.Table table)
        {
            InitializeComponent();
            mainTable = table;
            comboBoxColumn.ItemsSource = table.Headers.Cells.Select(c => c.Value).ToList();
        }


        private void buttonDiscretize_Click(object sender, RoutedEventArgs e)
        {
            int sectionCount;
            if (!Int32.TryParse(textBoxSectionCount.Text, out sectionCount))
                MessageBox.Show("error");
            else
            {
                mainTable = DiscretizationService.Discretize(mainTable, comboBoxColumn.SelectedIndex, sectionCount);
                this.Close();
            }
                
        }
    }
}
