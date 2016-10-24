using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {        
        public List<string> columnBinding { get; set; }
        public Model.Table mainTable;
        public ChartWindow(Model.Table table)
        {
            InitializeComponent();
            mainTable = table;
            columnBinding = DataTableService.GetColumnHeadersAsList(table);
            DataContext = this;
        }

        private void buttonGeneruj_Click(object sender, RoutedEventArgs e)
        {
            List<string> firstColumn = DataTableService.GetColumFromTableAsList(mainTable, comboBoxFirstColumn.SelectedIndex);
            List<string> secondColumn = DataTableService.GetColumFromTableAsList(mainTable, comboBoxSecondColumn.SelectedIndex);
            List<string> classColumn = DataTableService.GetColumFromTableAsList(mainTable, comboBoxClassColumn.SelectedIndex);

            List<double> firstColumnDouble = DataTableService.ConvertStingListToDoubleList(firstColumn, ".");
            List<double> secondColumnDouble = DataTableService.ConvertStingListToDoubleList(secondColumn, ".");

            cartesianChart.Series = ChartsService.GetNewSeriesCollectionDependOnColumns(firstColumnDouble, secondColumnDouble, classColumn);
            axisX.Title = comboBoxFirstColumn.SelectedItem.ToString();
            axisY.Title = comboBoxSecondColumn.SelectedItem.ToString();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
