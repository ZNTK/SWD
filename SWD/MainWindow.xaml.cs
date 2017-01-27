using System.Windows;
using SWD.Services;
using SWD.Import;
using SWD.ConvertToNum;
using SWD.Discretization;
using SWD.Charts;
using SWD.Normalization;
using SWD.KNearestNeighbours;
using SWD.KMeans;
using System;
using System.Collections.Generic;

namespace SWD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model.Table mainTable;
        List<Tuple<double, int>> linesPositions;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow importWindow = new ImportWindow();
            if (importWindow.ShowDialog() == false)
            {
                if (importWindow.dataWasImported)
                {
                    if (ValidationService.TableIsValid(importWindow.mainTable))
                    {
                        mainDataGrid = DataTableService.InsertDataToGrid(importWindow.mainTable, mainDataGrid);
                        mainTable = importWindow.mainTable;
                    }
                    else MessageBox.Show("W pliku występują braki danych!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }                
            }
        }

        private void buttonConvertToNum_Click(object sender, RoutedEventArgs e)
        {
            ConvertToNumWindow convertToNumWindow = new ConvertToNumWindow(mainTable);
            if (convertToNumWindow.ShowDialog() == false)
            {
                if (convertToNumWindow.result)
                {
                    ClearMainDataGrid();
                    mainDataGrid = DataTableService.InsertDataToGrid(convertToNumWindow.mainTable, mainDataGrid);
                }
            }            
        }

        private void buttonDiscretize_Click(object sender, RoutedEventArgs e)
        {
            DiscretizationWindow discretizationWindow = new DiscretizationWindow(mainTable);
            discretizationWindow.ShowDialog();

            ClearMainDataGrid();
            mainDataGrid = DataTableService.InsertDataToGrid(discretizationWindow.mainTable, mainDataGrid);
        }

        private void buttonNormalize_Click(object sender, RoutedEventArgs e)
        {
            NormalizationWindow normalizationWindow = new NormalizationWindow(mainTable);
            normalizationWindow.ShowDialog();

            ClearMainDataGrid();
            mainDataGrid = DataTableService.InsertDataToGrid(normalizationWindow.mainTable, mainDataGrid);
        }

        private void ClearMainDataGrid()
        {
            mainDataGrid.ItemsSource = null;
            mainDataGrid.Items.Refresh();
            int columnsCount = mainDataGrid.Columns.Count;
            for (int i = 0; i < columnsCount; i++)
            {
                mainDataGrid.Columns.RemoveAt(0);
            }
        }

        private void buttonCharts_Click(object sender, RoutedEventArgs e)
        {
            ChartWindow chartsWindow = new ChartWindow(mainTable);
            chartsWindow.ShowDialog();
        }

        private void buttonKNN_Click(object sender, RoutedEventArgs e)
        {
            KNearestNeighboursWindow kNearestNeighbours = new KNearestNeighboursWindow(mainTable);
            kNearestNeighbours.ShowDialog();
        }

        private void buttonKNNChart_Click(object sender, RoutedEventArgs e)
        {
            KNNChartWindow kNNChartWindow = new KNNChartWindow(mainTable);
            kNNChartWindow.ShowDialog();
        }

        private void buttonExportToCsv_Click(object sender, RoutedEventArgs e)
        {
            DataTableService.ExportDataTableToCsv(mainTable);
        }

        private void buttonGroup_Click(object sender, RoutedEventArgs e)
        {
            KMeansWindow kMeansWindow = new KMeansWindow(mainTable);
            kMeansWindow.ShowDialog();

            ClearMainDataGrid();
            mainDataGrid = DataTableService.InsertDataToGrid(kMeansWindow.mainTable, mainDataGrid);
        }

        private void buttonTree_Click(object sender, RoutedEventArgs e)
        {
            mainTable = DecisionTreeService.Group(mainTable);

            ClearMainDataGrid();
            mainDataGrid = DataTableService.InsertDataToGrid(mainTable, mainDataGrid);
            MessageBox.Show(mainTable.ResultInfo);
        }

        private void buttonED_Click(object sender, RoutedEventArgs e)
        {
            EDService edService = new EDService();

            linesPositions = edService.GetAllLines(10, DataTableService.GetColumnsFromTableAsValuesWithClassList(mainTable, mainTable.Headers.Cells.Count - 1), Convert.ToDouble(textBoxPercent.Text));
            mainTable = edService.AddEDColumn(mainTable, linesPositions);
            ClearMainDataGrid();
            mainDataGrid = DataTableService.InsertDataToGrid(mainTable, mainDataGrid);
            textBoxDlugosc.Text = linesPositions.Count.ToString();
        }

        private void buttonEDChart_Click(object sender, RoutedEventArgs e)
        {
            EDChartWindow chartsWindow = new EDChartWindow(mainTable, linesPositions);
            chartsWindow.ShowDialog();
        }

        private void buttonCzysc_Click(object sender, RoutedEventArgs e)
        {
            mainTable = null;
            ClearMainDataGrid();
        }
    }
}
