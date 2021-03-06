﻿using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
                    values.Add(new ScatterPoint(item.columnX, item.columnY, 1));
                }
                seriesCollection.Add(new ScatterSeries
                {
                    Values = values
                });
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
    }
}
