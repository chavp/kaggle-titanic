using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Titanic.Tests
{
    [TestClass]
    public class UnitTest1
    {
        List<Weather> dataSet = new List<Weather>();
        List<Weather> testSet = new List<Weather>();

        [TestInitialize]
        public void Setup()
        {
            string data = @"1 sunny hot high FALSE no
2 sunny hot high TRUE no
3 overcast hot high FALSE yes
4 rainy mild high FALSE yes
5 rainy cool normal FALSE yes
6 rainy cool normal TRUE no
7 overcast cool normal TRUE yes
8 sunny mild high FALSE no
9 sunny mild normal FALSE yes
10 rainy mild normal FALSE yes
11 sunny mild normal TRUE yes
12 overcast mild high TRUE yes
13 overcast hot normal FALSE yes
14 rainy mild high TRUE no";

            var values = data.Split('\n');
            foreach (var val in values)
            {
                var vs = val.Trim().Replace(" ", "|").Split('|');
                var new_we = new Weather
                {
                    No = Convert.ToInt32(vs[0]),
                    Outlook = (Outlook)Enum.Parse(typeof(Outlook), vs[1], true),
                    Temperature = (Temperature)Enum.Parse(typeof(Temperature), vs[2], true),
                    Humanity = (Humanity)Enum.Parse(typeof(Humanity), vs[3], true),
                    Windy = (Windy)Enum.Parse(typeof(Windy), vs[4], true),
                    Play = (vs[5] == "yes") ? true : false,
                };

                dataSet.Add(new_we);
            }
        }

        // http://dataminingtrend.com/2014/decision-tree-model/
        [TestMethod]
        public void find_GI()
        {
            var outlookGI = GI<Outlook>("Outlook", dataSet);
            var humGI = GI<Humanity>("Humanity", dataSet);
            var windGI = GI<Windy>("Windy", dataSet);
            var temGI = GI<Temperature>("Temperature", dataSet);

            Console.WriteLine("Outlook = {0}", outlookGI);
            Console.WriteLine("Humanity = {0}", humGI);
            Console.WriteLine("Windy = {0}", windGI);
            Console.WriteLine("Temperature = {0}", temGI);

            //Assert.AreEqual(0.24674981977443933, outlookGI);
        }

        [TestMethod]
        public void predic_GI()
        {
            int colect = 0;
            int incolect = 0;

            var predictDataSet = new List<bool>();

            var model = BuildDecisionTree();
            foreach (var item in dataSet)
            {
                // Outlook
                predictDataSet.Add(model.Predict(item, new List<string>
                {
                    "Outlook", "Humanity", "Windy"
                }));
            }

            // evaluate
            for (int i = 0; i < predictDataSet.Count; i++)
            {
                if(predictDataSet[i] == dataSet[i].Play)
                {
                    ++colect;
                }
                else
                {
                    ++incolect;
                }
            }

            Console.WriteLine("{0}", (double)colect / dataSet.Count);
        }

        public Node BuildDecisionTree()
        {
            var root = new Node
            {
                FieldName = "Root",
                Count = dataSet.Count,

                CountYes = dataSet.Where(x => x.Play).Count(),
                DataSet = dataSet
            };

            BuildChildNode(new List<string> { "Outlook", "Humanity", "Windy" }, root);
            //BuildChildNode<Outlook>("Outlook", root);
            //foreach (var item in root.Children)
            //{
            //    BuildChildNode<Humanity>("Humanity", item);

            //    foreach (var item2 in item.Children)
            //    {
            //        BuildChildNode<Windy>("Windy", item2);
            //    }
            //}
            return root;
        }

        public void BuildChildNode(List<string> pFieldNames, Node parent)
        {
            if (pFieldNames.Count == 0) return;

            var pFieldName = pFieldNames.First();

            BuildChildNode(pFieldName, parent);
            foreach (var item in parent.Children)
            {
                BuildChildNode(pFieldNames.Skip(1).ToList(), item);
            }
        }
        public void BuildChildNode(string pFieldName, Node parent)
        {
            var listOfDistinData = parent.DataSet.Select(x => GetPropValue(x, pFieldName)).Distinct();
            foreach (var node1 in listOfDistinData)
            {
                var node_1_list = parent.DataSet.Where(x => GetPropValue(x, pFieldName).Equals(node1)).ToList();
                var n1 = new Node
                {
                    FieldName = Convert.ToString(node1),
                    Count = node_1_list.Count,
                    CountYes = node_1_list.Where(x => x.Play).Count(),
                    DataSet = node_1_list,
                };
                parent.Children.Add(n1);
            }
        }

        [Obsolete]
        public void BuildChildNode<E>(string pFieldName, Node parent)
            where E : struct, IComparable, IFormattable, IConvertible
        {
            foreach (var node1 in Enum.GetValues(typeof(E)))
            {
                var e = (E)node1;
                var node_1_list = parent.DataSet.Where(x => GetPropValue(x, pFieldName).Equals(e)).ToList();

                var n1 = new Node
                {
                    FieldName = e.ToString(),
                    Count = node_1_list.Count,
                    CountYes = node_1_list.Where(x => x.Play).Count(),
                    DataSet = node_1_list,
                };
                parent.Children.Add(n1);
                //yield return n1;

                //if (n1.CountYes == 0) break;
                //if (pFieldName == "") break;
                //if (pFieldName == "") break;
                //n1.Children.AddRange(BuildChildNode<Humanity>("Humanity", node_1_list));
            }
        }

        public static double Entropy(double p)
        {
            if (p == 1) return 0;
            return (-p) * Math.Log(p, 2) - (1 - p) * Math.Log(1 - p, 2);
        }

        public double GI<E>(string pFieldName, List<Weather> dataSet) 
            where E: struct, IComparable, IFormattable, IConvertible
        {
            double gi = 0;

            double total = dataSet.Count;
            double cYes = (from x in dataSet where x.Play select x).Count();

            List<double> childsE = new List<double>();
            List<double> childsP = new List<double>();
            foreach (var item in Enum.GetValues(typeof(E)))
            {
                var en = (E)item;

                double t = (from x in dataSet where GetPropValue(x, pFieldName).Equals(en) select x).Count();
                double cCYes = (from x in dataSet where x.Play && GetPropValue(x, pFieldName).Equals(en) select x).Count();

                var e = Entropy(cCYes / t);
                var p = t / total;

                childsE.Add(e);
                childsP.Add(p);
            }

            var eP = Entropy(cYes / total);

            double wEC = 0;
            for (int i = 0; i < childsE.Count; i++)
            {
                wEC += childsE[i] * childsP[i];
            }

            gi = eP - wEC;

            return gi;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }

    public class Node
    {
        public Node()
        {
            Children = new List<Node>();
        }

        public string FieldName { get; set; }
        public int Count { get; set; }

        public int CountYes { get; set; }
        public int CountNo
        {
            get
            {
                return Count - CountYes;
            }
        }

        public List<Node> Children { get; set; }
        public List<Weather> DataSet { get; set; }

        public bool Predict(Weather item, IEnumerable<string> fields)
        {
            if (CountNo == 0) return true;
            if(fields.Count() == 0) return false;
            var ch1 = Children.Where(x => x.FieldName == UnitTest1.GetPropValue(item, fields.First()).ToString()).Single();
            if (ch1.CountNo == 0) return true;
            return ch1.Predict(item, fields.Skip(1));
        }
    }

    public class Weather
    {
        public int No { get; set; }
        public Outlook Outlook { get; set; }
        public Temperature Temperature { get; set; }
        public Humanity Humanity { get; set; }
        public Windy Windy { get; set; }
        public bool Play { get; set; }
    }

    public enum Outlook
    {
        Sunny, Overcast, Rainy
    }

    public enum Temperature
    {
        Cool, Hot, Mild
    }

    public enum Humanity
    {
        Normal, High
    }

    public enum Windy
    {
        True, False
    }
}
