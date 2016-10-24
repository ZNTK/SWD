using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SWD.Services
{
    class ChartsService
    {
        public static SeriesCollection GetNewSeriesCollectionDependOnColumns(List<double> firstColumn, List<double> secondColumn, List<string> classColumn)
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            List<string> classColumnListDistinct = classColumn.Distinct().ToList();
            var tupleZWartosciami = new List<Tuple<double, double, string>>();

            for (int i = 0; i < firstColumn.Count; i++)
            {
                tupleZWartosciami.Add(Tuple.Create(firstColumn[i], secondColumn[i], classColumn[i]));
            }

            foreach (var classItem in classColumnListDistinct)
            {
                ChartValues<ScatterPoint> values = new ChartValues<ScatterPoint>();
                var listaDanejKlasy = tupleZWartosciami.Where(x => x.Item3 == classItem).ToList();
                foreach(var item in listaDanejKlasy)
                {
                    values.Add(new ScatterPoint(item.Item1, item.Item2, 1));
                }
                seriesCollection.Add(new ScatterSeries
                {
                    Values = values
                });
            }
            return seriesCollection;
        }        
    }
}
