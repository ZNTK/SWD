using LiveCharts;
using SWD.KNearestNeighbours.Models;
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

namespace SWD.Charts
{
    /// <summary>
    /// Interaction logic for KNNChartWindow.xaml
    /// </summary>
    public partial class KNNChartWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public List<string> columnBinding { get; set; }
        List<ValuesWithClass> valuesWithClass;
        public Model.Table mainTable;
        public KNNChartWindow(Model.Table table)
        {
            InitializeComponent();
            mainTable = table;
            columnBinding = DataTableService.GetColumnHeadersAsList(table);
            DataContext = this;
        }

        private void comboBoxClassColumn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            valuesWithClass = DataTableService.GetColumnsFromTableAsValuesWithClassList(mainTable, comboBoxClassColumn.SelectedIndex);
        }

        private void buttonGenerujWykres_Click(object sender, RoutedEventArgs e)
        {
            cartesianChart.Series = ChartsService.GetSeriesCollectionForLineChart(valuesWithClass);
        }
    }
}
