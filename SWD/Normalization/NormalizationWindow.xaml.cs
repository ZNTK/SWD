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

namespace SWD.Normalization
{
    /// <summary>
    /// Interaction logic for NormalizationWindow.xaml
    /// </summary>
    public partial class NormalizationWindow : Window
    {
        public Model.Table mainTable;
        public NormalizationWindow(Model.Table table)
        {
            InitializeComponent();
            mainTable = table;
            comboBoxColumn.ItemsSource = table.Headers.Cells.Select(c => c.Value).ToList();
        }

        private void buttonNormalize_Click(object sender, RoutedEventArgs e)
        {
            mainTable = NormalizationService.Normalize(mainTable, comboBoxColumn.SelectedIndex);
            this.Close();
        }
    }
}
