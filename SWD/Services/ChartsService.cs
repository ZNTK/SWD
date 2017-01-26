using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SWD.ED.Models;
using SWD.KNearestNeighbours.Models;
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
        public static SeriesCollection GetNewSeriesCollectionDependOnColumns(List<ClassPoint> classPointList)
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            List<string> classColumnListDistinct = classPointList.Select(x => x.klasa).Distinct().ToList();

            foreach (var classItem in classColumnListDistinct)
            {
                ChartValues<ScatterPoint> values = new ChartValues<ScatterPoint>();
                var listaDanejKlasy = classPointList.Where(x => x.klasa == classItem).ToList();
                foreach(var item in listaDanejKlasy)
                {
                    values.Add(new ScatterPoint(item.columnX, item.columnY, 0.2));
                }
                seriesCollection.Add(new ScatterSeries
                {
                    Values = values
                });
            }

            

            return seriesCollection;
        }

        public static SeriesCollection GetNewSeriesCollectionFromSeparationResults(List<SeparationResult> separationResults)
        {
            SeriesCollection seriesCollection = new SeriesCollection();

            foreach (SeparationResult separationResult in separationResults)
            {
                int axis;
                if (separationResult.LineOrientation == (int)Enums.LineOrientation.Horizontal)
                    axis = 1;
                else axis = 0;

                seriesCollection.Add(ChartsService.DrawLine(separationResult.Value, axis, -50, 50, 002));


            }

            



            return seriesCollection;
        }


        public static SeriesCollection GetSeriesCollectionForLineChart(List<ValuesWithClass> valuesWithClass)
        {
            SeriesCollection seriesCollection = new SeriesCollection();
            List<List<double>> listOfClassification = KNearestNeighboursNColumsService.GetQualityClassificationForAllMetricAndNeighbors(valuesWithClass);
            List<string> metrics = new List<string>() { "odległość Euklidesowa", "metryka Manhattan", "nieskończoność", "Mahalanobisa" };
            for (int i = 0; i < 3; i++)
            {
                ChartValues<double> chartValues = new ChartValues<double>();
                foreach(var item in listOfClassification[i])
                {
                    chartValues.Add(item);
                }
                seriesCollection.Add(new LineSeries()
                {
                    Title = metrics[i],
                    Values = chartValues
                });
            }
            return seriesCollection;
        }


        public static ScatterSeries DrawLine(double value, int axis, double from, double to, double interval)// 0-X , 1-Y
        {
            ChartValues<ScatterPoint> values = new ChartValues<ScatterPoint>();
            double i = from;
            while (i < to)
            {
                if (axis == 0)
                {
                    values.Add(new ScatterPoint(value, i, 0.1));
                }
                else
                {
                    values.Add(new ScatterPoint(i, value, 0.1));
                }
                i += interval;
            }

            return
                new ScatterSeries
                {
                    Values = values
                };
        }
    }
}
