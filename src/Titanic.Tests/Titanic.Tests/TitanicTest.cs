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

            var tM = trainData.Where(x => x.Sex == Gender.Male).Count();
            var tF = trainData.Where(x => x.Sex == Gender.Female).Count();
            Console.WriteLine("Survival rate by Sex");
            var pM = (double)trainData.Where(x => x.Survived == 1 && x.Sex == Gender.Male).Count() / tM;
            var pF = (double)trainData.Where(x => x.Survived == 1 && x.Sex == Gender.Female).Count() / tF;
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
            var tCabG = trainData.Where(x => x.CabinType == CabinType.G).Count();
            var tCabT = trainData.Where(x => x.CabinType == CabinType.T).Count();
            var tCabU = trainData.Where(x => x.CabinType == CabinType.U).Count();
            Console.WriteLine("Survival rate by Cabin");
            var pCabA = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.A).Count() / tCabA;
            var pCabB = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.B).Count() / tCabB;
            var pCabC = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.C).Count() / tCabC;
            var pCabD = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.D).Count() / tCabD;
            var pCabE = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.E).Count() / tCabE;
            var pCabF = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.F).Count() / tCabF;
            var pCabG = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.G).Count() / tCabG;
            var pCabT = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.T).Count() / tCabT;
            var pCabU = (double)trainData.Where(x => x.Survived == 1 && x.CabinType == CabinType.U).Count() / tCabU;
            Console.WriteLine("p(CabA) = {0}", pCabA);
            Console.WriteLine("p(CabB) = {0}", pCabB);
            Console.WriteLine("p(CabC) = {0}", pCabC);
            Console.WriteLine("p(CabD) = {0}", pCabD);
            Console.WriteLine("p(CabE) = {0}", pCabE);
            Console.WriteLine("p(CabF) = {0}", pCabF);
            Console.WriteLine("p(CabG) = {0}", pCabG);
            Console.WriteLine("p(CabT) = {0}", pCabT);
            Console.WriteLine("p(CabU) = {0}", pCabU);
            Console.WriteLine("------------------------------------------------------");

            var tMYes = trainData.Where(x => x.Mother == Mother.Yes).Count();
            var tMNo = trainData.Where(x => x.Mother == Mother.No).Count();
            Console.WriteLine("Survival rate by Mother");
            var pMYes = (double)trainData.Where(x => x.Survived == 1 && x.Mother == Mother.Yes).Count() / tMYes;
            var pMNo = (double)trainData.Where(x => x.Survived == 1 && x.Mother == Mother.No).Count() / tMNo;
            Console.WriteLine("p(Mother) = {0}", pMYes);
            Console.WriteLine("p(Not Mother) = {0}", pMNo);
        }

        [TestMethod]
        public void cal_gi()
        {
            var giSex = GI<Gender>("Sex", trainData);
            var giEmbarked = GI<Embarked>("Embarked", trainData);
            var giFamilySize = GI<FamilySize>("FamilySize", trainData);
            var giAgeOrdinal = GI<AgeOrdinal>("AgeOrdinal", trainData);
            var giTitle = GI<Titles>("Title", trainData);
            var giCabinType = GI<CabinType>("CabinType", trainData);
            var giMother = GI<Mother>("Mother", trainData);

            Console.WriteLine("Sex = {0}", giSex);
            Console.WriteLine("FamilySize = {0}", giFamilySize);
            Console.WriteLine("Embarked = {0}", giEmbarked);
            Console.WriteLine("Mother = {0}", giMother);
            Console.WriteLine("AgeOrdinal = {0}", giAgeOrdinal);
            Console.WriteLine("Title = {0}", giTitle);
            Console.WriteLine("CabinType = {0}", giCabinType);
        }

        [TestMethod]
        public void validate()
        {
            // p(M) = 0.188908145580589
            // p(F) = 0.74203821656051

            int colect = 0;
            int incolect = 0;

            var predictDataSet = new List<int>();
            var model = BuildDecisionTree(trainData);
            foreach (var item in trainData)
            {
                var sur = model.Predict(item, new List<string>
                {
                    "Sex", "FamilySize", "Embarked", "Mother", "AgeOrdinal", "Title"
                });

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
        }

        [TestMethod]
        public void prediction()
        {
            var model = BuildDecisionTree(trainData);
            foreach (var item in testData)
            {
                // Outlook
                var sur = model.Predict(item, new List<string>
                {
                    "Sex", "FamilySize", "Embarked", "Mother", "AgeOrdinal", "Title"
                });

                item.Survived = sur.Value;
            }

            testData.WriteSubmission(Path.Combine(submission_root, "gender_submission.csv"));
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

        public TitanicNode BuildDecisionTree(List<Passenger> trainData)
        {
            var root = new TitanicNode
            {
                FieldName = "Root",
                Count = trainData.Count,

                CountYes = trainData.Where(x => x.Survived == 1).Count(),
                DataSet = trainData
            };

            //BuildChildNode<Outlook>(root, "Outlook", dataSet);

            BuildChildNode<Gender>("Sex", root);
            foreach (var item in root.Children)
            {
                BuildChildNode<FamilySize>("FamilySize", item);

                foreach (var item2 in item.Children)
                {
                    BuildChildNode<Embarked>("Embarked", item2);

                    foreach (var item3 in item2.Children)
                    {
                        BuildChildNode<Mother>("Mother", item3);
                        foreach (var item4 in item3.Children)
                        {
                            BuildChildNode<AgeOrdinal>("AgeOrdinal", item4);

                            foreach (var item5 in item4.Children)
                            {
                                BuildChildNode<Titles>("Title", item5);

                                foreach (var item6 in item5.Children)
                                {
                                    BuildChildNode<CabinType>("CabinType", item6);
                                }
                            }
                        }
                    }
                }
            }

            return root;
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
        public Gender? Sex { get; set; }
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
                if (string.IsNullOrEmpty(Cabin)) return CabinType.U;
                var cabit = Cabin.First().ToString();
                return (CabinType)Enum.Parse(typeof(CabinType), cabit, true);
            }
        }

        public Mother Mother
        {
            get
            {
                if (Sex == Gender.Female
                    && Parch > 0
                    && Age > 18
                    && Title != Titles.Miss)
                {
                    return Mother.Yes;
                }

                return Mother.No;
            }
        }

        
    }

    public enum Survived
    {
        Yes, No
    }

    public enum Gender
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
        A, B, C, D, E, F, G, T, U
    }

    public enum Mother
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
                p.Sex = (Gender)Enum.Parse(typeof(Gender), sex, true);
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
            var ch1 = Children.Where(x => x.FieldName == UnitTest1.GetPropValue(item, fields.First()).ToString()).Single();
            if (ch1.CountNo == 0) return 1;
            return ch1.Predict(item, fields.Skip(1));
        }
    }

}
