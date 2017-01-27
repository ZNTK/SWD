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
    /// Interaction logic for EDChartWindow.xaml
    /// </summary>
    public partial class EDChartWindow : Window
    {
        
        public Model.Table mainTable;
        List<Tuple<double, int>> linesPositions;
        public EDChartWindow(Model.Table table, List<Tuple<double, int>> positionsLines)
        {
            InitializeComponent();
            mainTable = table;
            DataContext = this;
            linesPositions = positionsLines;
            labelDlugosc.Content += linesPositions.Count.ToString();

            List<ClassPoint> classPointList = DataTableService.GetColumnFromTableAsClassPointList(mainTable, 0, 1, 2);

            cartesianChart.Series = ChartsService.GetNewSeriesCollectionDependOnColumns(classPointList);

            var fromX = classPointList.Min(x => x.columnX);
            var fromY = classPointList.Min(x => x.columnY);
            var toX = classPointList.Max(x => x.columnX);
            var toY = classPointList.Max(x => x.columnY);
            double from = fromX;
            double to = toX;
            foreach (var linePosition in linesPositions)
            {
                if (linePosition.Item2 == 1)
                {
                    from = fromX;
                    to = toX;
                }
                else
                {
                    from = fromY;
                    to = toY;
                }
                cartesianChart.Series.Add(ChartsService.DrawLine(linePosition.Item1, linePosition.Item2, from, to, 0.1));
            }
            axisX.Title = "X";
            axisY.Title = "Y";
        }
        
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
