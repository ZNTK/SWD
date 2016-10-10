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

namespace SWD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow importWindow = new ImportWindow();
            if (importWindow.ShowDialog() == false)
            {
				mainDataGrid = DataTableService.InsertDataToGrid(importWindow.mainTable, mainDataGrid);
                textBoxImport.Text = openFileDialog.FileName;

				bool firstRowIsHeader = true;
                Model.Table mainTable = DataTableService.GetTableFromFile(openFileDialog.FileName, firstRowIsHeader);
                if (ValidationService.TableIsValid(mainTable))
                    mainDataGrid = DataTableService.InsertDataToGrid(mainTable, mainDataGrid);
                else MessageBox.Show("W pliku występują braki danych!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }
    }
}
