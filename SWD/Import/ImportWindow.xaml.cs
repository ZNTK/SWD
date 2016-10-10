using Microsoft.Win32;
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

namespace SWD.Import
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        public Model.Table mainTable;
        public bool dataWasImported = false;   
        public ImportWindow()
        {
            InitializeComponent();
        }

        private void buttonImportuj_Click(object sender, RoutedEventArgs e)
        {
            char separator = '\0';
            if (rbTabulator.IsChecked == true) separator = '\t';
            if (rbComma.IsChecked == true) separator = ',';
            if (rbSemicolon.IsChecked == true) separator = ';';
            if (rbCustom.IsChecked == true) separator = textBoxCustomSeparator.Text.First();

            if (separator == '\0')
            {
                MessageBox.Show("Niepoprawny separator");
            }else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    MainWindow mainWindow = new MainWindow();
                    bool firstRowIsHeader = checkBoxFirstRowIsHeader.IsChecked == true ? true : false;
                    mainTable = DataTableService.GetTableFromFile(openFileDialog.FileName, firstRowIsHeader, separator);
                    if (mainTable != null) dataWasImported = true;                                  
                }
            }
            this.Close();
        }
    }
}
