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
using SWD.Model;
using SWD.Services;

namespace SWD.KMeans
{
    /// <summary>
    /// Interaction logic for KMeansWindow.xaml
    /// </summary>
    public partial class KMeansWindow : Window
    {
        public SWD.Model.Table mainTable;
        public KMeansWindow(SWD.Model.Table table)
        {
            InitializeComponent();
            mainTable = table;
            comboBoxColumn.ItemsSource = table.Headers.Cells.Select(c => c.Value).ToList();
        }


        private void buttonGroup_Click(object sender, RoutedEventArgs e)
        {
            int sectionCount;
            if (!Int32.TryParse(textBoxSectionCount.Text, out sectionCount))
                MessageBox.Show("error");
            else
            {
               
                mainTable = KMeansService.Group(mainTable, comboBoxColumn.SelectedIndex, sectionCount);
                this.Close();
            }

        }
    }
}
