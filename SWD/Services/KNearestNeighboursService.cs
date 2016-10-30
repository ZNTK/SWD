using SWD.KNearestNeighbours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SWD.Services
{
    class KNearestNeighboursService
    {
        public KNearestNeighboursService()
        {

        }

        public static string GetNewClassDependsOnPoint(Point point, List<ClassPoint> classPointList, int numberOfNeighbors, int method)
        {
            List<ClassPointDistance> classPointDistances = new List<ClassPointDistance>();

            switch (method)
            {
                case 0://euklidesowa
                    foreach (var classPoint in classPointList)
                    {
                        classPointDistances.Add(new ClassPointDistance(classPoint.klasa, classPoint.columnX, classPoint.columnY, GetEuclideanDistance(point, classPoint)));
                    }
                    break;
                case 1://manhattan
                    foreach (var classPoint in classPointList)
                    {
                        classPointDistances.Add(new ClassPointDistance(classPoint.klasa, classPoint.columnX, classPoint.columnY, GetManhattanDistance(point, classPoint)));
                    }
                    break;
                case 2://nieskonczonosc//czebyszewa
                    foreach (var classPoint in classPointList)
                    {
                        classPointDistances.Add(new ClassPointDistance(classPoint.klasa, classPoint.columnX, classPoint.columnY, GetInfinityDistance(point, classPoint)));
                    }
                    break;
                case 3://Mahalanobisa

                    break;
                default:
                    break;
            }

            var nearestNeighboursDistances = classPointDistances.OrderBy(x => x.distance).Take(numberOfNeighbors).ToList();

            return GetClassWithLowestDistanceSum(nearestNeighboursDistances);
        }

        public static string GetClassWithLowestDistanceSum(List<ClassPointDistance> nearestNeighboursDistances)
        {
            List<ClassDistance> classDistances = new List<ClassDistance>();
            var classes = nearestNeighboursDistances.Select(x => x.klasa).Distinct().ToList();

            foreach(var klasa in classes)
            {
                var distance = nearestNeighboursDistances.Where(x => x.klasa == klasa).Select(x => x.distance).ToList().Sum();
                classDistances.Add(new ClassDistance(klasa, distance));
            }
            
            return classDistances.OrderBy(x => x.distance).First().klasa; // jak taka sama odleglość to pierwszego bierze czyl itego co najblizej
        }

        public static double GetEuclideanDistance(Point pointOne, ClassPoint pointTwo)
        {
            return Math.Sqrt((Math.Pow(pointOne.X - pointTwo.columnX, 2) + (Math.Pow(pointOne.Y - pointTwo.columnY, 2))));
        }

        public static double GetManhattanDistance(Point pointOne, ClassPoint pointTwo)
        {
            return (Math.Abs(pointOne.X - pointTwo.columnX)) + (Math.Abs(pointOne.Y - pointTwo.columnY));
        }

        public static double GetInfinityDistance(Point pointOne, ClassPoint pointTwo) // chebyshev distance // odległość czebyszewa
        {
            return Math.Max(Math.Abs(pointOne.X - pointTwo.columnX), Math.Abs(pointOne.Y - pointTwo.columnY));
        }

        public static double GetMahalanobisDistance(Point pointOne,ClassPoint pointTwo) // te cale jast napisane na 2 wymiarowe i KNearestNeighboursNColumsService trzeba sie bawic bo tam dla roznych
        {
            return 1;
        }

        //ocena jakosci klasyfikacji

        public static double GetQualityClassification(List<ClassPoint> classPointList, int numberOfNeighbors, int method)
        {
            int classEqualsCount = 0;
            foreach(var classPoint in classPointList)
            {
                var classPointListWithoutClassPoint = classPointList.Where(x => !x.Equals(classPoint)).ToList();
                string newClass = GetNewClassDependsOnPoint(new Point(classPoint.columnX, classPoint.columnY), classPointListWithoutClassPoint, numberOfNeighbors, method);
                if (classPoint.klasa == newClass)
                {
                    classEqualsCount++;
                }
            }
            return (double)classEqualsCount / (double)classPointList.Count;
        }
    }
}
