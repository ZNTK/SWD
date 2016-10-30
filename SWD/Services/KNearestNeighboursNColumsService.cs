using SWD.KNearestNeighbours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SWD.Services
{
    public class KNearestNeighboursNColumsService
    {
        public static string GetNewClassDependsOnValues(List<double> givenValuesList, List<ValuesWithClass> valuesWithClass, int numberOfNeighbors, int method)
        {
            List<ValuesWithClassDistance> valuesWithClassDistance = new List<ValuesWithClassDistance>();
            switch (method)
            {
                case 0://euklidesowa
                    foreach(var values in valuesWithClass)
                    {
                        valuesWithClassDistance.Add(new ValuesWithClassDistance(values.valuesList, values.klasa, GetEuclideanDistance(givenValuesList, values.valuesList)));
                    }
                    break;
                case 1://manhattan
                    foreach (var values in valuesWithClass)
                    {
                        valuesWithClassDistance.Add(new ValuesWithClassDistance(values.valuesList, values.klasa, GetManhattanDistance(givenValuesList, values.valuesList)));
                    }
                    break;
                case 2://nieskonczonosc//czebyszewa
                    foreach (var values in valuesWithClass)
                    {
                        valuesWithClassDistance.Add(new ValuesWithClassDistance(values.valuesList, values.klasa, GetInfinityDistance(givenValuesList, values.valuesList)));
                    }
                    break;
                case 3://Mahalanobisa

                    break;
                default:
                    break;
            }

            var nearestNeighboursDistances = valuesWithClassDistance.OrderBy(x => x.distance).Take(numberOfNeighbors).ToList();

            return GetClassWithLowestDistanceSum(nearestNeighboursDistances);
        }
        public static string GetClassWithLowestDistanceSum(List<ValuesWithClassDistance> nearestNeighboursDistances)
        {
            List<ClassDistance> classDistances = new List<ClassDistance>();
            var classes = nearestNeighboursDistances.Select(x => x.klasa).Distinct().ToList();

            foreach (var klasa in classes)
            {
                var distance = nearestNeighboursDistances.Where(x => x.klasa == klasa).Select(x => x.distance).ToList().Sum();
                classDistances.Add(new ClassDistance(klasa, distance));
            }

            return classDistances.OrderBy(x => x.distance).First().klasa; // jak taka sama odleglość to pierwszego bierze czyl itego co najblizej
        }

        public static double GetEuclideanDistance(List<double> givenValuesList, List<double> valuesList)
        {
            double sum = 0;
            for(int i = 0; i < valuesList.Count; i++)
            {
                sum += Math.Pow(givenValuesList[i] - valuesList[i], 2);
            }
            return Math.Sqrt(sum);
        }

        public static double GetManhattanDistance(List<double> givenValuesList, List<double> valuesList)
        {
            double sum = 0;
            for(int i = 0; i < valuesList.Count; i++)
            {
                sum += Math.Abs(givenValuesList[i] - valuesList[i]);
            }
            return sum;
        }

        public static double GetInfinityDistance(List<double> givenValuesList, List<double> valuesList) // chebyshev distance // odległość czebyszewa
        {
            List<double> chebyshevValues = new List<double>();
            for(int i = 0; i < valuesList.Count; i++)
            {
                chebyshevValues.Add(Math.Abs(givenValuesList[i] - valuesList[i]));
            }
            return chebyshevValues.Max();
        }

        public static double GetMahalanobisDistance(Point pointOne, ClassPoint pointTwo)
        {
            return 1;
        }

        //ocena jakosci klasyfikacji

        public static double GetQualityClassification(List<ValuesWithClass> valuesWithClass, int numberOfNeighbors, int method)
        {
            int classEqualsCount = 0;
            foreach (var values in valuesWithClass)
            {
                var listWithoutCheckedItem = valuesWithClass.Where(x => !x.Equals(values)).ToList();
                string newClass = GetNewClassDependsOnValues(values.valuesList, listWithoutCheckedItem, numberOfNeighbors, method);
                if (values.klasa == newClass)
                {
                    classEqualsCount++;
                }
            }
            return (double)classEqualsCount / (double)valuesWithClass.Count;
        }
    }
}
