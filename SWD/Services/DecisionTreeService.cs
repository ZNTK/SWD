using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.Services;
using SWD.DecisionTree.Models;

namespace SWD.Services
{
    public static class DecisionTreeService
    {
        static int _maxClassIndex;
        static List<int> _classes;

        public static Model.Table Group(Model.Table table)
        {
            _classes = new List<int>();
            int correctClassificationCount = 0;
            Model.Table _tableTmp = CopyTable(table);

            table = ConvertTableValuesToNumbers(table);
           

            for (int i = 0; i < table.Rows.Count; i++)
            {
                Model.Table tableWithoutRow = new Model.Table();
                tableWithoutRow.Rows  = new List<Model.Row>(table.Rows);
                tableWithoutRow.Rows.RemoveAt(i);

                if (ClassifyRow(tableWithoutRow, table.Rows[i], i))
                    correctClassificationCount++;
            }

            table.Headers.Cells.Add(new Model.Cell("ODMIRYS_NumValues"));
            table.Headers.Cells.Add(new Model.Cell("ODMIRYS_DecTree"));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                table.Rows[i].Cells.Add(new Model.Cell(table.Rows[i].Cells[4].Value));
                table.Rows[i].Cells.Add(new Model.Cell(_classes[i].ToString()));

                for(int j = 0; j < 5;j++)
                {
                    table.Rows[i].Cells[j].Value = _tableTmp.Rows[i].Cells[j].Value;
                }

            }
            int s = (int)correctClassificationCount / table.Rows.Count();
            double ss = (int)correctClassificationCount / table.Rows.Count;
            double sss = ss * 100;
            table.ResultInfo = String.Format("Poprawnie sklasyfikowane obiekty: {0}/{1} ({2}%)",
                                            correctClassificationCount.ToString(),
                                            table.Rows.Count.ToString(),
                                            (((double)correctClassificationCount / (double)table.Rows.Count())*100).ToString("00.0"));
            return table;
        }
        

        public static bool ClassifyRow(Model.Table table, Model.Row rowToClassification, int rowIndex)
        {
           
            

            List<Leaf> leafs = GenerateLeafs(table.Rows[0].Cells.Count - 1);
            List<Leaf> _leafsTmp;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                _leafsTmp = leafs;

                for (int j = 0; j < table.Rows[i].Cells.Count - 1; j++)
                {
                    _leafsTmp = _leafsTmp.Where(l => l.ArgValues[j] == int.Parse(table.Rows[i].Cells[j].Value)).ToList();
                }

                int leafIndexToIncreaseClassIndex = _leafsTmp.SingleOrDefault().LeafIndex;
                int classIndex = int.Parse(table.Rows[i].Cells.Last().Value);

                leafs.Where(l => l.LeafIndex == leafIndexToIncreaseClassIndex).SingleOrDefault().ClassIndexCounts[classIndex]++;
            }


            foreach(Leaf leaf in leafs)
            {
                int maxCount = leaf.ClassIndexCounts.Max();
                for(int i = 0; i< leaf.ClassIndexCounts.Count;i++)
                {
                    if (leaf.ClassIndexCounts[i] == maxCount)
                    {
                        leaf.ClassIndex = i;
                        break;
                    }
                }
            }

            List<Leaf>_leafsTmpToClassification = leafs;
            for (int i = 0; i <rowToClassification.Cells.Count - 1; i++)
            {
                _leafsTmpToClassification = _leafsTmpToClassification.Where(l => l.ArgValues[i] == double.Parse(rowToClassification.Cells[i].Value)).ToList();
            }
            int leafClassificationIndex = _leafsTmpToClassification.SingleOrDefault().LeafIndex;
            _classes.Add(leafs[leafClassificationIndex].ClassIndex);
            if (leafs[leafClassificationIndex].ClassIndex == double.Parse(rowToClassification.Cells[rowToClassification.Cells.Count - 1].Value))
                return true;

            return false;
        }



        private static Model.Table ConvertTableValuesToNumbers(Model.Table table)
        {
            for (int i = 0; i < table.Rows[0].Cells.Count - 1; i++)
            {
                table = DiscretizationService.DiscretizeWithReplaceColumn(table, i, 2);
            }
            table = ConvertWithReplaceColumn(table);

            return table;
        }

        private static List<Leaf> GenerateLeafs(int argsCount)
        {
            int leafCount = (int)Math.Pow(2, argsCount);
            List<Leaf> leafs = new List<Leaf>();
            for (int i = 0; i < leafCount; i++)
            {
                leafs.Add(new Leaf(argsCount, i));
            }

            for(int i = 0; i < argsCount; i++)
            {
                int indexToPow = i + 1;
                int partsCount = (int)Math.Pow(2, indexToPow);
                int partSize = leafs.Count / partsCount;
          

                for (int j = 0; j < partsCount; j++)
                {
                    for(int k = j*partSize; k < (j+1)*partSize; k++)
                    {
                        leafs[k].ArgValues[i] = j % 2;
                    }
                }

            }

            foreach(Leaf leaf in leafs)
            {
                for (int i = 0; i <= _maxClassIndex; i++)
                    leaf.ClassIndexCounts.Add(0);
            }


            return leafs;
        }

        private static Model.Table CopyTable(Model.Table table)
        {
            Model.Table newTable = new Model.Table();

            foreach (Model.Cell cell in table.Headers.Cells)
                newTable.Headers.Cells.Add(new Model.Cell(cell.Value));

            foreach (Model.Row row in table.Rows)
            {
                List<string> rowCellsValues = new List<string>();
                foreach (Model.Cell cell in row.Cells)
                    rowCellsValues.Add(cell.Value);

                newTable.Rows.Add(new Model.Row(rowCellsValues.ToArray()));
            }



            return newTable;
        }


        #region szyluk

        private static Model.Table ConvertWithReplaceColumn(Model.Table mainTable)
        {
            var comboboxSelectedIndex = mainTable.Rows[0].Cells.Count - 1;
            List<string> stringColumn = Services.DataTableService.GetColumFromTableAsList(mainTable, comboboxSelectedIndex);

            var klasyZWartosciami = new List<Tuple<string, int>>();

            stringColumn = stringColumn.Distinct().ToList();
            //if (rbAlfabetyczna.IsChecked == true)
            //{
            //    stringColumn.Sort();
            //}
            int i = 0;
            foreach (var row in stringColumn)
            {
                klasyZWartosciami.Add(Tuple.Create(row, i));
                i++;
            }

            //mainTable.Headers.Cells.Add(new Model.Cell(comboBoxColumn.SelectedItem + "_NumValues"));
            //foreach (var row in mainTable.Rows)
            //{
            //    var nr = klasyZWartosciami.Where(x => x.Item1 == row.Cells[comboboxSelectedIndex].Value).Select(x => x.Item2).FirstOrDefault();
            //    row.Cells.Add(new Model.Cell(nr.ToString()));
            //}
            int maxClassIndex = 0;

            for (int j = 0; j < mainTable.Rows.Count; j++)
            {
                var nr = klasyZWartosciami.Where(x => x.Item1 == mainTable.Rows[j].Cells[comboboxSelectedIndex].Value).Select(x => x.Item2).FirstOrDefault();
                mainTable.Rows[j].Cells[comboboxSelectedIndex].Value = nr.ToString();
               // _mainTable.Rows[j].Cells.Add(new Model.Cell(nr.ToString()));
                if (nr > maxClassIndex)
                    maxClassIndex = nr;
            }

            //  result = true;
            _maxClassIndex = maxClassIndex;
            return mainTable;
        }

        #endregion


    }



}
