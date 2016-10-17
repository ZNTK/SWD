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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SWD.Model;
using SWD.Services;
using Microsoft.Win32;
using SWD.Import;
using SWD.ConvertToNum;

namespace SWD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model.Table mainTable;
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
                    mainDataGrid.ItemsSource = null;
                    mainDataGrid.Items.Refresh();
                    int columnsCount = mainDataGrid.Columns.Count;
                    for (int i = 0; i < columnsCount; i++)
                    {
                        mainDataGrid.Columns.RemoveAt(0);
                    }
                    mainDataGrid = DataTableService.InsertDataToGrid(convertToNumWindow.mainTable, mainDataGrid);
                }
            }            
        }
    }
}
