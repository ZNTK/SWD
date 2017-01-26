using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {        
        public List<string> columnBinding { get; set; }
        public List<string> metrykaBinding { get; set; }
        List<ClassPoint> classPointList;
        public Model.Table mainTable;
        public ChartWindow(Model.Table table)
        {
            InitializeComponent();
            mainTable = table;
            columnBinding = DataTableService.GetColumnHeadersAsList(table);
            metrykaBinding = new List<string>() { "odległość Euklidesowa", "metryka Manhattan", "nieskończoność", "Mahalanobisa" };
            DataContext = this;
        }

        public void buttonGenerujED_Click(object sender, RoutedEventArgs e)
        {
            //if (comboBoxFirstColumn.SelectedIndex == -1
            //    || comboBoxSecondColumn.SelectedIndex == -1
            //    || comboBoxClassColumn.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Nie wybrano wartości dla wszystkich kolumn");
            //}

            //comboBoxFirstColumn.SelectedItem = new KeyValuePair<string, int>("MySelected", 0);
            //comboBoxSecondColumn.SelectedIndex = 1;
            //comboBoxClassColumn.SelectedIndex = 2;

            classPointList = DataTableService.GetColumnFromTableAsClassPointList(mainTable, 0, 1, 2);

            cartesianChart.Series = ChartsService.GetNewSeriesCollectionDependOnColumns(classPointList);
            axisX.Title = "Arg 0";
            axisY.Title = "Arg 1";

            SeriesCollection seriesCollectionSepRes = ChartsService.GetNewSeriesCollectionFromSeparationResults(mainTable.SeparationResults);
            foreach(var i in seriesCollectionSepRes)
                cartesianChart.Series.Add(i);




            //buttonRozwinKlasyfikacje.IsEnabled = true;

        }

        private void buttonGeneruj_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxFirstColumn.SelectedIndex == -1
                || comboBoxSecondColumn.SelectedIndex == -1
                || comboBoxClassColumn.SelectedIndex == -1)
            {
                MessageBox.Show("Nie wybrano wartości dla wszystkich kolumn");
            }
            else
            {
                classPointList = DataTableService.GetColumnFromTableAsClassPointList(mainTable, comboBoxFirstColumn.SelectedIndex, comboBoxSecondColumn.SelectedIndex, comboBoxClassColumn.SelectedIndex);

                cartesianChart.Series = ChartsService.GetNewSeriesCollectionDependOnColumns(classPointList);
                axisX.Title = comboBoxFirstColumn.SelectedItem.ToString();
                axisY.Title = comboBoxSecondColumn.SelectedItem.ToString();

                buttonRozwinKlasyfikacje.IsEnabled = true;
            }
        }


        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonRozwinKlasyfikacje_Click(object sender, RoutedEventArgs e)
        {
            gridKlasyfikacja.Visibility = Visibility.Visible;
            buttonRozwinKlasyfikacje.Visibility = Visibility.Collapsed;
            labelWartoscX.Content = comboBoxFirstColumn.SelectedItem;
            labelWartoscY.Content = comboBoxSecondColumn.SelectedItem;
        }

        private void buttonZwin_Click(object sender, RoutedEventArgs e)
        {
            buttonRozwinKlasyfikacje.Visibility = Visibility.Visible;
            gridKlasyfikacja.Visibility = Visibility.Collapsed;
        }

        private void buttonKlasyfikuj_Click(object sender, RoutedEventArgs e)
        {
            Point point = new Point(Convert.ToDouble(textBoxWartoscX.Text), Convert.ToDouble(textBoxWartoscY.Text));
            int numberOfNeighbours = Convert.ToInt32(textBoxLiczbaSasiadow.Text);

            textBoxKlasa.Text = KNearestNeighboursService.GetNewClassDependsOnPoint(point, classPointList, numberOfNeighbours, comboBoxMetrykaOcenyOdleglosci.SelectedIndex);

            cartesianChart.Series.Add(new ScatterSeries()
            {
                Values = new ChartValues<ScatterPoint>()
                {
                    new ScatterPoint(point.X,point.Y,1)
                },
                PointGeometry = DefaultGeometries.Diamond,
                Fill = Brushes.Black
            });
        }

        private void buttonJakoscKlasyfikacji_Click(object sender, RoutedEventArgs e)
        {
            double doubleResult = KNearestNeighboursService.GetQualityClassification(classPointList, Convert.ToInt32(textBoxLiczbaSasiadow.Text), comboBoxMetrykaOcenyOdleglosci.SelectedIndex);

            MessageBox.Show("Jakość klasyfikacji jest równa: " + doubleResult.ToString());
        }

        private void comboBoxMetrykaOcenyOdleglosci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsAllDataForClassificationFilled();
            IsAllDataForQualityClassificationFilled();
        }

        private void textBoxLiczbaSasiadow_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsAllDataForClassificationFilled();
            IsAllDataForQualityClassificationFilled();
        }

        private void textBoxWartoscX_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsAllDataForClassificationFilled();
        }

        private void textBoxWartoscY_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsAllDataForClassificationFilled();
        }

        private void IsAllDataForClassificationFilled()
        {
            if(textBoxWartoscX.Text != ""
                && textBoxWartoscY.Text != ""
                && textBoxLiczbaSasiadow.Text != ""
                && comboBoxMetrykaOcenyOdleglosci.SelectedIndex > -1)
            {
                buttonKlasyfikuj.IsEnabled = true;
            } else
            {
                buttonKlasyfikuj.IsEnabled = false;
            }
        }

        private void IsAllDataForQualityClassificationFilled()
        {
            if(textBoxLiczbaSasiadow.Text != "" && comboBoxMetrykaOcenyOdleglosci.SelectedIndex > -1)
            {
                buttonJakoscKlasyfikacji.IsEnabled = true;
            }
            else
            {
                buttonJakoscKlasyfikacji.IsEnabled = false;
            }
        }
    }
}
