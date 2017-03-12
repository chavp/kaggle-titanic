using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CsvHelper;

namespace Titanic.Tests
{
    [TestClass]
    public class TitanicTest
    {
        // https://www.kaggle.com/c/titanic
        // https://www.kaggle.com/mrisdal/titanic/exploring-survival-on-the-titanic
        // https://www.kaggle.com/helgejo/titanic/an-interactive-data-science-tutorial
        const string data_root = @"E:\projects\Kaggle\kaggle-titanic\data";
        const string submission_root = @"E:\projects\Kaggle\kaggle-titanic\out";

        List<Passenger> trainData = new List<Passenger>();
        List<Passenger> testData = new List<Passenger>();
        List<Passenger> fullData = new List<Passenger>();
        [TestInitialize]
        public void Setup()
        {
            // load data use Install-Package CsvHelper
            using (var reader = File.OpenText(Path.Combine(data_root, "train.csv")))
            using (var csv = new CsvReader(reader))
            {
                while (csv.Read())
                {
                    trainData.Add(csv.ToPassenger());
                }
            }

            trainData.FillNullValues();

            using (var reader = File.OpenText(Path.Combine(data_root, "test.csv")))
            using (var csv = new CsvReader(reader))
            {
                while (csv.Read())
                {
                    testData.Add(csv.ToPassenger());
                }
            }

            testData.FillNullValues();

            //fullData = trainData.Union(testData).ToList();

            //fullData.FillNullValues();
        }

        [TestMethod]
        public void display_survival_prob()
        {
            // PassengerId	Survived	Pclass	Name	Sex	Age	SibSp	Parch	Ticket	Fare	Cabin	Embarked
            // 1   0   3   Braund, Mr.Owen Harris male    22  1   0   A / 5 21171   7.25        S

            var c_emb_null = (from x in trainData where x.Embarked == null select x).Count();
            var c_age_null = (from x in trainData where x.Age == null select x).Count();
            var c_fare_null = (from x in trainData where x.Fare == null select x).Count();

            var surTotal = trainData.Where(x => x.Survived == 1).Count();

            Console.WriteLine("{0}", (double)surTotal / trainData.Count);

            var tC = trainData.Where(x => x.Embarked == Embarked.C).Count();
            var tQ = trainData.Where(x => x.Embarked == Embarked.Q).Count();
            var tS = trainData.Where(x => x.Embarked == Embarked.S).Count();

            var pC = (double)trainData.Where(x => x.Survived == 1 && x.Embarked == Embarked.C).Count() / tC;
            var pQ = (double)trainData.Where(x => x.Survived == 1 && x.Embarked == Embarked.Q).Count() / tQ;
            var pS = (double)trainData.Where(x => x.Survived == 1 && x.Embarked == Embarked.S).Count() / tS;

            Console.WriteLine("Survival rate by Embarked");
            Console.WriteLine("p(C) = {0}", pC);
            Console.WriteLine("p(Q) = {0}", pQ);
            Console.WriteLine("p(S) = {0}", pS);
            Console.WriteLine("------------------------------------------------------");

            var tM = trainData.Where(x => x.Sex == Sex.Male).Count();
            var tF = trainData.Where(x => x.Sex == Sex.Female).Count();
            Console.WriteLine("Survival rate by Sex");
            var pM = (double)trainData.Where(x => x.Survived == 1 && x.Sex == Sex.Male).Count() / tM;
            var pF = (double)trainData.Where(x => x.Survived == 1 && x.Sex == Sex.Female).Count() / tF;
            Console.WriteLine("p(M) = {0}", pM);
            Console.WriteLine("p(F) = {0}", pF);
            Console.WriteLine("------------------------------------------------------");

            var tC1 = trainData.Where(x => x.Pclass == 1).Count();
            var tC2 = trainData.Where(x => x.Pclass == 2).Count();
            var tC3 = trainData.Where(x => x.Pclass == 3).Count();
            Assert.AreEqual(tC1 + tC2 + tC3, trainData.Count);
            Console.WriteLine("Survival rate by Pclass");
            var pC1 = (double)trainData.Where(x => x.Survived == 1 && x.Pclass == 1).Count() / tC1;
            var pC2 = (double)trainData.Where(x => x.Survived == 1 && x.Pclass == 2).Count() / tC2;
            var pC3 = (double)trainData.Where(x => x.Survived == 1 && x.Pclass == 3).Count() / tC3;
            Console.WriteLine("p(C1) = {0}", pC1);
            Console.WriteLine("p(C2) = {0}", pC2);
            Console.WriteLine("p(C3) = {0}", pC3);
            Console.WriteLine("------------------------------------------------------");

            var tSing = trainData.Where(x => x.FamilySize == FamilySize.Single).Count();
            var tSmal = trainData.Where(x => x.FamilySize == FamilySize.Small).Count();
            var tLarg = trainData.Where(x => x.FamilySize == FamilySize.Large).Count();
            Console.WriteLine("Survival rate by FamilySize");
            var pSing = (double)trainData.Where(x => x.Survived == 1 && x.FamilySize == FamilySize.Single).Count() / tSing;
            var pSmal = (double)trainData.Where(x => x.Survived == 1 && x.FamilySize == FamilySize.Small).Count() / tSmal;
            var pLarg = (double)trainData.Where(x => x.Survived == 1 && x.FamilySize == FamilySize.Large).Count() / tLarg;
            Console.WriteLine("p(Single) = {0}", pSing);
            Console.WriteLine("p(Small) = {0}", pSmal);
            Console.WriteLine("p(Large) = {0}", pLarg);
            Console.WriteLine("------------------------------------------------------");

            var tCh = trainData.Where(x => x.AgeOrdinal == AgeOrdinal.Child).Count();
            var tY = trainData.Where(x => x.AgeOrdinal == AgeOrdinal.Young).Count();
            var tA = trainData.Where(x => x.AgeOrdinal == AgeOrdinal.Adult).Count();
            var tO = trainData.Where(x => x.AgeOrdinal == AgeOrdinal.Old).Count();
            Console.WriteLine("Survival rate by Age");
            var pCh = (double)trainData.Where(x => x.Survived == 1 && x.AgeOrdinal == AgeOrdinal.Child).Count() / tCh;
            var pY = (double)trainData.Where(x => x.Survived == 1 && x.AgeOrdinal == AgeOrdinal.Young).Count() / tY;
            var pA = (double)trainData.Where(x => x.Survived == 1 && x.AgeOrdinal == AgeOrdinal.Adult).Count() / tA;
            var pO = (double)trainData.Where(x => x.Survived == 1 && x.AgeOrdinal == AgeOrdinal.Old).Count() / tO;
            Console.WriteLine("p(Child) = {0}", pCh);
            Console.WriteLine("p(Young) = {0}", pY);
            Console.WriteLine("p(Adult) = {0}", pA);
            Console.WriteLine("p(Old) = {0}", pO);
            Console.WriteLine("------------------------------------------------------");

            // Officer, Royalty, Mrs, Miss, Mr, Master
            var tOfficer = trainData.Where(x => x.Title == Titles.Officer).Count();
            var tRoyalty = trainData.Where(x => x.Title == Titles.Royalty).Count();
            var tMrs = trainData.Where(x => x.Title == Titles.Mrs).Count();
            var tMiss = trainData.Where(x => x.Title == Titles.Miss).Count();
            var tMr = trainData.Where(x => x.Title == Titles.Mr).Count();
            var tMaster = trainData.Where(x => x.Title == Titles.Master).Count();
            Console.WriteLine("Survival rate by Titles");
            var pOfficer = (double)trainData.Where(x => x.Survived == 1 && x.Title == Titles.Officer).Count() / tOfficer;
            var pRoyalty = (double)trainData.Where(x => x.Survived == 1 && x.Title == Titles.Royalty).Count() / tRoyalty;
            var pMrs = (double)trainData.Where(x => x.Survived == 1 && x.Title == Titles.Mrs).Count() / tMrs;
            var pMiss = (double)trainData.Where(x => x.Survived == 1 && x.Title == Titles.Miss).Count() / tMiss;
            var pMr = (double)trainData.Where(x => x.Survived == 1 && x.Title == Titles.Mr).Count() / tMr;
            var pMaster = (double)trainData.Where(x => x.Survived == 1 && x.Title == Titles.Master).Count() / tMaster;
            Console.WriteLine("p(Officer) = {0}", pOfficer);
            Console.WriteLine("p(Royalty) = {0}", pRoyalty);
            Console.WriteLine("p(Mrs) = {0}", pMrs);
            Console.WriteLine("p(Miss) = {0}", pMiss);
            Console.WriteLine("p(Mr) = {0}", pMr);
            Console.WriteLine("p(Master) = {0}", pMaster);
            Console.WriteLine("------------------------------------------------------");

            // A, B, C, D, E, F, G, T, U
            var tCabA = trainData.Where(x => x.CabinType == CabinType.A).Count();
            var tCabB = trainData.Where(x => x.CabinType == CabinType.B).Count();
            var tCabC = trainData.Where(x => x.CabinType == CabinType.C).Count();
            var tCabD = trainData.Where(x => x.CabinType == CabinType.D).Count();
            var tCabE = trainData.Where(x => x.CabinType == CabinType.E).Count();
            var tCabF = trainData.Where(x => x.CabinType == CabinType.F).Count();
            var tCabU = trainData.Where(x => x.CabinType == CabinType.U).Count();
            Console.WriteLine("Survival rate by Cabin");
            var pCabA = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.A).Count() / tCabA;
            var pCabB = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.B).Count() / tCabB;
            var pCabC = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.C).Count() / tCabC;
            var pCabD = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.D).Count() / tCabD;
            var pCabE = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.E).Count() / tCabE;
            var pCabF = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.F).Count() / tCabF;
            var pCabU = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.U).Count() / tCabU;
            Console.WriteLine("p(CabA) = {0}", pCabA);
            Console.WriteLine("p(CabB) = {0}", pCabB);
            Console.WriteLine("p(CabC) = {0}", pCabC);
            Console.WriteLine("p(CabD) = {0}", pCabD);
            Console.WriteLine("p(CabE) = {0}", pCabE);
            Console.WriteLine("p(CabF) = {0}", pCabF);
            Console.WriteLine("p(CabU) = {0}", pCabU);
            Console.WriteLine("------------------------------------------------------");

            var tMYes = trainData.Where(x => x.Mother == Mother.Yes).Count();
            var tMNo = trainData.Where(x => x.Mother == Mother.No).Count();
            Console.WriteLine("Survival rate by Mother");
            var pMYes = (double)trainData.Where(x => x.Survived == 1 && x.Mother == Mother.Yes).Count() / tMYes;
            var pMNo = (double)trainData.Where(x => x.Survived == 1 && x.Mother == Mother.No).Count() / tMNo;
            Console.WriteLine("p(Mother) = {0}", pMYes);
            Console.WriteLine("p(Not Mother) = {0}", pMNo);
            Console.WriteLine("------------------------------------------------------");

            var tFlo = trainData.Where(x => x.FareRate == FareRate.Lo).Count();
            var tFme = trainData.Where(x => x.FareRate == FareRate.Me).Count();
            var tFhi = trainData.Where(x => x.FareRate == FareRate.Hi).Count();
            Console.WriteLine("Survival rate by FareRate");
            var pFlo = (double)trainData.Where(x => x.Survived == 1 && x.FareRate == FareRate.Lo).Count() / tFlo;
            var pFme = (double)trainData.Where(x => x.Survived == 1 && x.FareRate == FareRate.Me).Count() / tFme;
            var pFhi = (double)trainData.Where(x => x.Survived == 1 && x.FareRate == FareRate.Hi).Count() / tFhi;
            Console.WriteLine("p(Lo) = {0}", pFlo);
            Console.WriteLine("p(Me) = {0}", pFme);
            Console.WriteLine("p(Hi) = {0}", pFhi);
            Console.WriteLine("------------------------------------------------------");

            var tP1 = trainData.Where(x => x.PclassLevel == PclassLevel.One).Count();
            var tP2 = trainData.Where(x => x.PclassLevel == PclassLevel.Two).Count();
            var tP3 = trainData.Where(x => x.PclassLevel == PclassLevel.Three).Count();
            Console.WriteLine("Survival rate by PclassLevel");
            var pP1 = (double)trainData.Where(x => x.Survived == 1 && x.PclassLevel == PclassLevel.One).Count() / tP1;
            var pP2 = (double)trainData.Where(x => x.Survived == 1 && x.PclassLevel == PclassLevel.Two).Count() / tP2;
            var pP3 = (double)trainData.Where(x => x.Survived == 1 && x.PclassLevel == PclassLevel.Three).Count() / tP3;
            Console.WriteLine("p(Pclass 1) = {0}", pP1);
            Console.WriteLine("p(Pclass 2) = {0}", pP2);
            Console.WriteLine("p(Pclass 3) = {0}", pP3);
            Console.WriteLine("------------------------------------------------------");

            var tFYes = trainData.Where(x => x.HaveParch == HaveParch.Yes).Count();
            var tFNo = trainData.Where(x => x.HaveParch == HaveParch.No).Count();
            Console.WriteLine("Survival rate by HaveParch");
            var pFYes = (double)trainData.Where(x => x.Survived == 1 && x.HaveParch == HaveParch.Yes).Count() / tFYes;
            var pFNo = (double)trainData.Where(x => x.Survived == 1 && x.HaveParch == HaveParch.No).Count() / tFNo;
            Console.WriteLine("p(HaveParch.Yes) = {0}", pFYes);
            Console.WriteLine("p(HaveParch.No) = {0}", pFNo);
            Console.WriteLine("------------------------------------------------------");

            var tSYes = trainData.Where(x => x.HaveSibsp == HaveSibsp.Yes).Count();
            var tSNo = trainData.Where(x => x.HaveSibsp == HaveSibsp.No).Count();
            Console.WriteLine("Survival rate by HaveSibsp");
            var pSYes = (double)trainData.Where(x => x.Survived == 1 && x.HaveSibsp == HaveSibsp.Yes).Count() / tSYes;
            var pSNo = (double)trainData.Where(x => x.Survived == 1 && x.HaveSibsp == HaveSibsp.No).Count() / tSNo;
            Console.WriteLine("p(HaveSibsp.Yes) = {0}", pSYes);
            Console.WriteLine("p(HaveSibsp.No) = {0}", pSNo);
            Console.WriteLine("------------------------------------------------------");
        }
        List<string> gi_order_fields = new List<string>
        {
            "Sex",
            "PclassLevel",
            "Title",
            "CabinType",
            "FareRate",
            "FamilySize",
            "Embarked",
            "Mother",
            "AgeOrdinal",
            "HaveParch",
            "HaveSibsp"
        };
        [TestMethod]
        public void cal_gi()
        {
            var dicofGi = new Dictionary<string, double>();
            foreach (var fieldName in gi_order_fields)
            {
                var gi = GI(fieldName, trainData);
                dicofGi.Add(fieldName, gi);
            }

            foreach (var item in dicofGi)
            {
                Console.WriteLine("{0} = {1}", item.Key, item.Value);
            }

           // gi_order_fields = dicofGi.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

            //var giSex = GI<Gender>("Sex", trainData);
            //var giEmbarked = GI<Embarked>("Embarked", trainData);
            //var giFamilySize = GI<FamilySize>("FamilySize", trainData);
            //var giAgeOrdinal = GI<AgeOrdinal>("AgeOrdinal", trainData);
            //var giTitle = GI<Titles>("Title", trainData);
            //var giCabinType = GI<CabinType>("CabinType", trainData);
            //var giMother = GI<Mother>("Mother", trainData);
            //var giFareRate = GI<FareRate>("FareRate", trainData);
            //var giPclass = GI<PclassLevel>("PclassLevel", trainData);
            //var giHaveParch = GI<HaveParch>("HaveParch", trainData);
            //var giHaveSibsp = GI<HaveSibsp>("HaveSibsp", trainData);

            //Console.WriteLine("Sex = {0}", giSex);
            //Console.WriteLine("FamilySize = {0}", giFamilySize);
            //Console.WriteLine("Embarked = {0}", giEmbarked);
            //Console.WriteLine("Mother = {0}", giMother);
            //Console.WriteLine("AgeOrdinal = {0}", giAgeOrdinal);
            //Console.WriteLine("Title = {0}", giTitle);
            //Console.WriteLine("CabinType = {0}", giCabinType);
            //Console.WriteLine("FareRate = {0}", giFareRate);
            //Console.WriteLine("PclassLevel = {0}", giPclass);
            //Console.WriteLine("HaveParch = {0}", giHaveParch);
            //Console.WriteLine("HaveSibsp = {0}", giHaveSibsp);
        }

        [TestMethod]
        public void validate()
        {
            // p(M) = 0.188908145580589
            // p(F) = 0.74203821656051

            int colect = 0;
            int incolect = 0;

            var predictDataSet = new List<int>();
            var targetFieldList = GetMaximizeGi(gi_order_fields, trainData);

            var model = BuildDecisionTree(targetFieldList, trainData);

            foreach (var item in trainData)
            {
                var sur = model.Predict(item, targetFieldList);

                if (sur.HasValue)
                {
                    predictDataSet.Add(sur.Value);
                }
            }

            for (int i = 0; i < predictDataSet.Count; i++)
            {
                if (predictDataSet[i] == trainData[i].Survived.GetValueOrDefault())
                {
                    ++colect;
                }
                else
                {
                    ++incolect;
                }
            }

            Console.WriteLine("accuracy = {0}", (double)colect / trainData.Count);

            // accuracy = 0.89337822671156
            // accuracy = 0.874298540965208
            // accuracy = 0.898989898989899
            // accuracy = 0.904601571268238
        }

        [TestMethod]
        public void prediction()
        {
            var targetFieldList = GetMaximizeGi(gi_order_fields, trainData);
            var model = BuildDecisionTree(targetFieldList, trainData);
            foreach (var item in testData)
            {
                // Outlook
                var sur = model.Predict(item, targetFieldList);

                item.Survived = sur.Value;
            }

            testData.WriteSubmission(Path.Combine(submission_root, "gender_submission.csv"));
        }

        public double GI(string pFieldName, List<Passenger> dataSet)
        {
            double gi = 0;
            var listOfDistinData = dataSet.Select(x => UnitTest1.GetPropValue(x, pFieldName)).Distinct();
            double total = dataSet.Count;
            double cYes = (from x in dataSet where x.Survived == 1 select x).Count();

            List<double> childsE = new List<double>();
            List<double> childsP = new List<double>();
            foreach (var item in listOfDistinData)
            {
                //var en = (E)item;

                double t = (from x in dataSet where UnitTest1.GetPropValue(x, pFieldName).Equals(item) select x).Count();
                double cCYes = (from x in dataSet where x.Survived == 1 && UnitTest1.GetPropValue(x, pFieldName).Equals(item) select x).Count();

                if (t == 0) continue;

                var e = UnitTest1.Entropy(cCYes / t);
                var p = t / total;

                childsE.Add(e);
                childsP.Add(p);
            }

            var eP = UnitTest1.Entropy(cYes / total);

            double wEC = 0;
            for (int i = 0; i < childsE.Count; i++)
            {
                wEC += childsE[i] * childsP[i];
            }

            gi = eP - wEC;

            return gi;
        }

        public double GI<E>(string pFieldName, List<Passenger> dataSet)
            where E : struct, IComparable, IFormattable, IConvertible
        {
            double gi = 0;

            double total = dataSet.Count;
            double cYes = (from x in dataSet where x.Survived == 1 select x).Count();

            List<double> childsE = new List<double>();
            List<double> childsP = new List<double>();
            foreach (var item in Enum.GetValues(typeof(E)))
            {
                var en = (E)item;

                double t = (from x in dataSet where UnitTest1.GetPropValue(x, pFieldName).Equals(en) select x).Count();
                double cCYes = (from x in dataSet where x.Survived == 1 && UnitTest1.GetPropValue(x, pFieldName).Equals(en) select x).Count();

                if (t == 0) continue;

                var e = UnitTest1.Entropy(cCYes / t);
                var p = t / total;

                childsE.Add(e);
                childsP.Add(p);
            }

            var eP = UnitTest1.Entropy(cYes / total);

            double wEC = 0;
            for (int i = 0; i < childsE.Count; i++)
            {
                wEC += childsE[i] * childsP[i];
            }

            gi = eP - wEC;

            return gi;
        }

        [Obsolete]
        public void BuildChildNode<E>(string pFieldName, TitanicNode parent)
            where E : struct, IComparable, IFormattable, IConvertible
        {
            foreach (var node1 in Enum.GetValues(typeof(E)))
            {
                var e = (E)node1;
                var node_1_list = parent.DataSet.Where(x => UnitTest1.GetPropValue(x, pFieldName).Equals(e)).ToList();

                var n1 = new TitanicNode
                {
                    FieldName = e.ToString(),
                    Count = node_1_list.Count,
                    CountYes = node_1_list.Where(x => x.Survived == 1).Count(),
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

        public void BuildChildNode(List<string> pFieldNames, TitanicNode parent)
        {
            if (pFieldNames.Count == 0) return;

            var pFieldName = pFieldNames.First();
            pFieldNames = pFieldNames.Skip(1).ToList();

            BuildChildNode(pFieldName, parent);
            foreach (var item in parent.Children)
            {
                BuildChildNode(pFieldNames, item);
            }
        }
        public void BuildChildNode(string pFieldName, TitanicNode parent)
        {
            var listOfDistinData = parent.DataSet.Select(x => UnitTest1.GetPropValue(x, pFieldName)).Distinct();
            foreach (var node1 in listOfDistinData)
            {
                var node_1_list = parent.DataSet.Where(x => UnitTest1.GetPropValue(x, pFieldName).Equals(node1)).ToList();
                var n1 = new TitanicNode
                {
                    FieldName = string.Format("{0}.{1}", pFieldName, node1),
                    Count = node_1_list.Count,
                    CountYes = node_1_list.Where(x => x.Survived == 1).Count(),
                    DataSet = node_1_list,
                };
                parent.Children.Add(n1);
            }
        }

        public TitanicNode BuildDecisionTree(List<string> targetFieldNames, List<Passenger> trainData)
        {
            var root = new TitanicNode
            {
                FieldName = "Root",
                Count = trainData.Count,

                CountYes = trainData.Where(x => x.Survived == 1).Count(),
                DataSet = trainData
            };

            //"Sex", "PclassLevel", "Title", "CabinType", "FareRate", "FamilySize", "Embarked", "Mother", "AgeOrdinal", "HaveParch", "HaveSibsp"
            //var bufferList = GetMaximizeGi(gi_order_fields, trainData);
            //var bufferList = gi_order_fields.ToList();

            BuildChildNode(targetFieldNames, root);
            //foreach (var item in root.Children)
            //{
            //    BuildChildNode<PclassLevel>("PclassLevel", item);

            //    foreach (var item2 in item.Children)
            //    {
            //        BuildChildNode<Titles>("Title", item2);

            //        foreach (var item3 in item2.Children)
            //        {
            //            BuildChildNode<CabinType>("CabinType", item3);
            //            foreach (var item4 in item3.Children)
            //            {
            //                BuildChildNode<FareRate>("FareRate", item4);

            //                foreach (var item5 in item4.Children)
            //                {
            //                    BuildChildNode<FamilySize>("FamilySize", item5);

            //                    foreach (var item6 in item5.Children)
            //                    {
            //                        BuildChildNode<Embarked>("Embarked", item6);

            //                        foreach (var item7 in item6.Children)
            //                        {
            //                            BuildChildNode<Mother>("Mother", item7);

            //                            foreach (var item8 in item7.Children)
            //                            {
            //                                BuildChildNode<AgeOrdinal>("AgeOrdinal", item8);

            //                                //foreach (var item9 in item8.Children)
            //                                //{
            //                                //    BuildChildNode<HaveParch>("HaveParch", item9);

            //                                //    foreach (var item10 in item9.Children)
            //                                //    {
            //                                //        BuildChildNode<HaveSibsp>("HaveSibsp", item10);
            //                                //    }
            //                                //}
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            return root;
        }

        public List<string> GetMaximizeGi(List<string> targetFields, List<Passenger> trainData)
        {
            var dicofGi = new Dictionary<string, double>();
            foreach (var fieldName in targetFields)
            {
                var gi = GI(fieldName, trainData);
                dicofGi.Add(fieldName, gi);
            }

            return dicofGi.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
        }
    }

    public class Passenger
    {
        private static readonly Dictionary<string, Titles> Title_Dictionary = new Dictionary<string, Titles>();
        static Passenger()
        {
            Title_Dictionary.Add("Capt", Titles.Officer);
            Title_Dictionary.Add("Col", Titles.Officer);
            Title_Dictionary.Add("Major", Titles.Officer);
            Title_Dictionary.Add("Jonkheer", Titles.Royalty);
            Title_Dictionary.Add("Don", Titles.Royalty);
            Title_Dictionary.Add("Sir", Titles.Royalty);
            Title_Dictionary.Add("Dr", Titles.Officer);
            Title_Dictionary.Add("Rev", Titles.Officer);
            Title_Dictionary.Add("the Countess", Titles.Royalty);
            Title_Dictionary.Add("Dona", Titles.Royalty);
            Title_Dictionary.Add("Mme", Titles.Mrs);
            Title_Dictionary.Add("Mlle", Titles.Miss);
            Title_Dictionary.Add("Ms", Titles.Mrs);
            Title_Dictionary.Add("Mr", Titles.Mr);
            Title_Dictionary.Add("Mrs", Titles.Mrs);
            Title_Dictionary.Add("Miss", Titles.Miss);
            Title_Dictionary.Add("Master", Titles.Master);
            Title_Dictionary.Add("Lady", Titles.Royalty);
        }

        public long Id { get; set; }
        public int? Survived { get; set; }
        public int Pclass { get; set; }
        public string Name { get; set; }
        public Sex? Sex { get; set; }
        public int? Age { get; set; }
        public int SibSp { get; set; }
        public int Parch { get; set; }
        public string Ticket { get; set; }
        public decimal? Fare { get; set; }
        public string Cabin { get; set; }
        public Embarked? Embarked { get; set; }

        public FamilySize FamilySize
        {
            get
            {
                var fSize = SibSp + Parch + 1;
                if (fSize == 1) return FamilySize.Single;
                if (1 < fSize && fSize <= 4) return FamilySize.Small;
                if (4 < fSize) return FamilySize.Large;
                return FamilySize.Single;
            }
        }

        public AgeOrdinal AgeOrdinal
        {
            get
            {
                if (Age < 12) return AgeOrdinal.Child;
                if (12 < Age && Age <= 20) return AgeOrdinal.Young;
                if (20 < Age && Age <= 59) return AgeOrdinal.Adult;
                if (59 < Age) return AgeOrdinal.Old;
                return AgeOrdinal.Adult;
            }
        }

        public Titles Title
        {
            get
            {
                foreach (var key in Title_Dictionary.Keys)
                {
                    if (Name.ToLower().Contains(key.ToLower()))
                    {
                        return Title_Dictionary[key];
                    }
                }
                return Titles.Nan;
            }
        }

        public CabinType CabinType
        {
            get
            {
                if (string.IsNullOrEmpty(Cabin) || Cabin == "T") return CabinType.U;
                var cabit = Cabin.First().ToString();
                if (cabit == "G") cabit = "F";
                return (CabinType)Enum.Parse(typeof(CabinType), cabit, true);
            }
        }

        public Mother Mother
        {
            get
            {
                if (Sex == Tests.Sex.Female
                    && Parch > 0
                    && Age > 18
                    && Title != Titles.Miss
                    )
                {
                    return Mother.Yes;
                }

                return Mother.No;
            }
        }

        public FareRate FareRate
        {
            get
            {
                if (Fare < 15) return FareRate.Lo;
                if (15 < Fare && Fare <= 40) return FareRate.Me;
                if (40 < Fare) return FareRate.Hi;
                return FareRate.Me;
            }
        }

        public PclassLevel PclassLevel
        {
            get
            {
                if (Pclass == 1) return PclassLevel.One;
                if (Pclass == 2) return PclassLevel.Two;
                if (Pclass == 3) return PclassLevel.Three;
                return PclassLevel.Two;
            }
        }

        public HaveParch HaveParch
        {
            get
            {
                if (Parch > 0)
                {
                    return HaveParch.Yes;
                }

                return HaveParch.No;
            }
        }

        public HaveSibsp HaveSibsp
        {
            get
            {
                if (SibSp > 0)
                {
                    return HaveSibsp.Yes;
                }

                return HaveSibsp.No;
            }
        }
    }

    public enum PclassLevel
    {
        One, Two, Three
    }

    public enum Survived
    {
        Yes, No
    }

    public enum Sex
    {
        Male, Female
    }

    public enum Embarked
    {
        C, Q, S
    }

    public enum FamilySize
    {
        Single, Small, Large
    }

    public enum AgeOrdinal
    {
        Child, Young, Adult, Old
    }

    public enum Titles
    {
        Nan, Officer, Royalty, Mrs, Miss, Mr, Master
    }

    public enum CabinType
    {
        A, B, C, D, E, F, U
    }

    public enum Mother
    {
        Yes, No
    }

    public enum FareRate
    {
        Lo, Me, Hi
    }

    public enum HaveParch
    {
        Yes, No
    }

    public enum HaveSibsp
    {
        Yes, No
    }

    public static class PassengerExtension
    {
        public static Passenger ToPassenger(this CsvReader csv)
        {
            var p = new Passenger
            {
                Id = csv.GetField<long>("PassengerId"),
                Pclass = csv.GetField<int>("Pclass"),
                Name = csv.GetField<string>("Name"),
                SibSp = csv.GetField<int>("SibSp"),
                Parch = csv.GetField<int>("Parch"),
                Ticket = csv.GetField<string>("Ticket"),
                Cabin = csv.GetField<string>("Cabin"),
                //Embarked = (Embarked)Enum.Parse(typeof(Embarked), csv.GetField<string>("Embarked"), true)
            };

            int? s = null;
            if (csv.TryGetField<int?>("Survived", out s))
            {
                p.Survived = s;
            }

            string sex = null;
            if (csv.TryGetField<string>("Sex", out sex))
            {
                p.Sex = (Sex)Enum.Parse(typeof(Sex), sex, true);
            }

            int? age = null;
            if (csv.TryGetField<int?>("Age", out age))
            {
                p.Age = age;
            }

            decimal? fare = null;
            if (csv.TryGetField<decimal?>("Fare", out fare))
            {
                p.Fare = fare;
            }

            string embarked = null;
            if (csv.TryGetField<string>("Embarked", out embarked))
            {
                if (!string.IsNullOrEmpty(embarked))
                {
                    p.Embarked = (Embarked)Enum.Parse(typeof(Embarked), embarked, true);
                }
            }
            return p;
        }
        public static void FillNullValues(this IList<Passenger> dataSet)
        {
            var age_mean = dataSet.Average(x => x.Age);
            foreach (var item in dataSet.Where(x => !x.Age.HasValue))
            {
                item.Age = (int)age_mean;
            }

            foreach (var item in dataSet.Where(x => !x.Embarked.HasValue))
            {
                item.Embarked = Embarked.C;
            }
        }
        public static void WriteSubmission(this IList<Passenger> dataSet, string fileName)
        {
            using (var writer = File.CreateText(fileName))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteField("PassengerId");
                csv.WriteField("Survived");
                csv.NextRecord();
                foreach (var item in dataSet)
                {
                    csv.WriteField(item.Id);
                    csv.WriteField(item.Survived);
                    csv.NextRecord();
                }
            }
        }
    } 

    // model
    public class TitanicNode
    {
        private static Random s_Generator = new Random();

        public TitanicNode()
        {
            Children = new List<TitanicNode>();
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

        public double P
        {
            get
            {
                return (double)CountYes / Count;
            }
        }

        public List<TitanicNode> Children { get; set; }
        public List<Passenger> DataSet { get; set; }

        public int? Predict(Passenger item, IEnumerable<string> fields)
        {
            if (CountNo == 0) return 1;
            if (fields.Count() == 0)
            {
                //int result = s_Generator.NextDouble() <= P ? 1 : 0;
                //return result;
                int result = P > 0.5 ? 1 : 0;
                return result;
            }
            var field = fields.First();
            //string.Format("{0}.{1}", pFieldName, node1),
            var val = UnitTest1.GetPropValue(item, field).ToString();
            var fieldName = string.Format("{0}.{1}", field, val);
            var ch1 = Children.Where(x => x.FieldName == fieldName).Single();
            if (ch1.CountNo == 0) return 1;
            return ch1.Predict(item, fields.Skip(1));
        }
    }

}
