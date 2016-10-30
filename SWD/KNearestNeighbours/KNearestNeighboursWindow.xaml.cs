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

namespace SWD.KNearestNeighbours
{
    /// <summary>
    /// Interaction logic for KNearestNeighboursWindow.xaml
    /// </summary>
    public partial class KNearestNeighboursWindow : Window
    {
        public List<string> columnBinding { get; set; }
        public List<string> metrykaBinding { get; set; }
        List<ValuesWithClass> valuesWithClass;
        Model.Table table;
        public KNearestNeighboursWindow(Model.Table table)
        {
            InitializeComponent();
            this.table = table;
            columnBinding = DataTableService.GetColumnHeadersAsList(table);
            
            metrykaBinding = new List<string>() { "odległość Euklidesowa", "metryka Manhattan", "nieskończoność", "Mahalanobisa" };
            DataContext = this;
        }

        private void buttonJakoscKlasyfikacji_Click(object sender, RoutedEventArgs e)
        {
            double doubleResult = KNearestNeighboursNColumsService.GetQualityClassification(valuesWithClass, Convert.ToInt32(textBoxLiczbaSasiadow.Text), comboBoxMetrykaOcenyOdleglosci.SelectedIndex);
            MessageBox.Show("Jakość klasyfikacji jest równa: " + doubleResult.ToString());
        }

        private void buttonKlasyfikuj_Click(object sender, RoutedEventArgs e)
        {
            List<double> givenValues = new List<double>();
            foreach (var item in gridValues.Children.OfType<TextBox>())
            {
                givenValues.Add(Convert.ToDouble(item.Text));
            }

            textBoxKlasa.Text = KNearestNeighboursNColumsService.GetNewClassDependsOnValues(givenValues, valuesWithClass, Convert.ToInt32(textBoxLiczbaSasiadow.Text), comboBoxMetrykaOcenyOdleglosci.SelectedIndex);
        }

        private void comboBoxClassColumn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            valuesWithClass = DataTableService.GetColumnsFromTableAsValuesWithClassList(table, comboBoxClassColumn.SelectedIndex);
            int j = 0;
            for (int i = 1; i < columnBinding.Count; i++)
            {
                if (i != comboBoxClassColumn.SelectedIndex)
                {
                    gridValues.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });

                    Label tempLabel = new Label();
                    tempLabel.Name = "labelValue" + i;
                    tempLabel.Content = columnBinding[i];
                    tempLabel.HorizontalContentAlignment = HorizontalAlignment.Center;

                    TextBox tempTextBox = new TextBox();
                    tempTextBox.Name = "textBoxValue" + i;
                    tempTextBox.Height = 30;
                    tempTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;

                    gridValues.Children.Add(tempLabel);
                    gridValues.Children.Add(tempTextBox);

                    Grid.SetRow(tempLabel, j);
                    Grid.SetColumn(tempLabel, i - (j/2 * 20));

                    Grid.SetRow(tempTextBox, j + 1);
                    Grid.SetColumn(tempTextBox, i - (j/2 * 20));
                }
                if (i % 20 == 0 && i != 0)
                {
                    j += 2;
                    gridValues.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = GridLength.Auto
                    });
                    gridValues.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = GridLength.Auto
                    });
                }
            }
            DataContext = this;
        }
    }
}
