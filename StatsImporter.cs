using System.Xml;
using System.Collections;
using System;
using System.Diagnostics.Contracts;
using System.Linq;


namespace stats_converter
{
    public class ImportObject
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? StatType { get; set; }

        public List<string>? RawStats { get; set; }

        // new/key, type, and using lines
        // if it's an ItemCombinationResult, will contain the entirety of the object
        public OrderedDictionary<string, string?>? ObjHeader { get; set; }

        // if value is 3, it means > 2 lines
        public int? LineCount { get; set; }

        // remember to use foreach for these
        // all of the data lines
        public OrderedDictionary<string, List<string?>?> ObjBody { get; set; }
        public OrderedDictionary<string, List<string?>?> ObjFooter { get; set; }

    }


    public class ImportFile
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? StatType { get; set; }
        public XmlDocument XmlStatsFile = new();
        public IEnumerator? StatsNodes { get; set; }
        // remember to use foreach for these
        public List<ImportObject>? StatsObjects = new();
    }


    public class StatsImporter
    {
        // public static void BuildStatsHeader(List<string> statsQueue)
        public static OrderedDictionary<string, string?>? BuildStatsHeader(List<string>? rawDataList, string StatType)
        {
            OrderedDictionary<string, string?>? tempObjHeader = new();

            tempObjHeader.TryAdd("type", StatType);

            for (int i = 0; i < rawDataList.Count; i++)
            {
                if (rawDataList[i] == "Name")
                {
                    tempObjHeader.Insert(0, "new entry", StatsFuncs.AddQuotes(rawDataList[i + 2]));

                }
                else if (rawDataList[i] == "Using")
                {
                    tempObjHeader.Insert(2, "using", StatsFuncs.AddQuotes(rawDataList[i + 2]));
                }
            }

            return tempObjHeader;
        }


        public static OrderedDictionary<string, List<string?>?> BuildStatsBody(List<string>? rawDataList)
        {
            OrderedDictionary<string, List<string?>?> tempObjBody = new();

            for (int i = 0; i < rawDataList.Count; i++)
            {
                List<string?> tempList = new();
                if 
                    (
                        !StatsData.HeaderFields.Contains(rawDataList[i]) &&
                        ((i + 2) < rawDataList.Count) &&
                        FileImporter.ValueFilter(rawDataList[i]) != null
                    )
                {
                    Console.WriteLine(rawDataList[i]);
                    Console.WriteLine(StatsFuncs.AddQuotes(rawDataList[i + 2]));
                    tempList.Append(StatsFuncs.AddQuotes(rawDataList[i + 2]));
                    tempObjBody.TryAdd("data", tempList);

                    // Console.WriteLine(i);
                }
            }

            return tempObjBody;
        }


        // public static void BuildImportObject(ImportFile tempFile)
        public static ImportObject BuildImportObject(ImportFile tempFile, XmlNode fieldNodes)
        {
            ImportObject tempStatObject = new();

            tempStatObject.StatType = StatsFuncs.GetStatType(tempFile.FileName);
            tempStatObject.RawStats = FileImporter.StatLineParser(tempStatObject.StatType, fieldNodes);
            tempStatObject.ObjHeader = BuildStatsHeader(tempStatObject.RawStats, tempStatObject.StatType);
            tempStatObject.ObjBody = BuildStatsBody(tempStatObject.RawStats);

            /*foreach (KeyValuePair<string, string?> headerLine in tempStatObject.ObjHeader)
            {
                Console.WriteLine(headerLine.Key + " " + headerLine.Value);
            }*/

            foreach (KeyValuePair<string, List<string?>?> bodyLine in tempStatObject.ObjBody)
            {
                foreach (string? value in bodyLine.Value)
                {
                    Console.WriteLine(bodyLine.Key + " " + value);
                }
            }


            return tempStatObject;
        }

    }


    public class FileImporter
    {
        public static string? ValueFilter(string value)
        {
            if (value.Contains("FieldDefinition"))
            {
                return null;
            }
            return value;
        }


        // make this a yield? nah
        public static List<string>? StatLineParser(string StatType, XmlNode fieldNodes)
        // public static ImportObject? StatLineParser(string file, IEnumerator statsNodes)
        {
            List<string> tempStatQueue = new();

            for (int i = 0; i < fieldNodes.ChildNodes.Count; i++) 
            {
                for (int j = 0; j < fieldNodes.ChildNodes[i].Attributes.Count; j++)
                { 
                    var attrValue = fieldNodes.ChildNodes[i].Attributes[j].Value;

                    if (attrValue != null )
                    {
                        tempStatQueue.Add(attrValue);
                    }
                }
            }

            if (tempStatQueue.Count > 0)
            {
                return tempStatQueue;
            }
            return null;
        }

        public static void Main()
        //public static void ImportStatsFile()
        {
            string StatsFolderPath = "..\\..\\..\\test_files\\";
            // string[] StatsFiles = Array.ConvertAll(Directory.GetFiles(StatsFolderPath, "*.stats"), file => Path.GetFileName(file));
            // string[] StatsFiles = ["carry weight extra_Data.stats"];
            string[] StatsFiles = ["rangerbuff_Character.stats"];

            foreach (string file in StatsFiles)
            {
                ImportFile tempFile = new();

                // need to deal with StatsFolderPath being empty
                tempFile.FileName = Path.GetFileNameWithoutExtension(file);
                tempFile.FilePath = StatsFolderPath + file;
                tempFile.XmlStatsFile.Load(tempFile.FilePath);
                tempFile.StatsNodes = tempFile.XmlStatsFile.DocumentElement.FirstChild.GetEnumerator();

                while (tempFile.StatsNodes.MoveNext())
                {
                    XmlNode? statsNode = (XmlNode)tempFile.StatsNodes.Current;
                    tempFile.StatsObjects.Add(StatsImporter.BuildImportObject(tempFile, statsNode.FirstChild));
                }   
            }
        }
    }
}

