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

        // new/key, type, and using lines
        // if it's an ItemCombinationResult, will contain the entirety of the object
        public List<string>? ObjHeader { get; set; }

        // if value is 3, it means > 2 lines
        public int? LineCount { get; set; }

        // remember to use foreach for these
        // all of the data lines
        public Queue<string>? DataLines { get; set; }

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
        public static List<string>? BuildStatsHeader(Queue statsQueue)
        {
            // detect stat type
            // set expected LineCount
            // Dict with string key and list value 
        }


        // public static void BuildImportObject(ImportFile tempFile)
        public static ImportObject BuildImportObject(ImportFile tempFile, XmlNode fieldNodes)
        {
            ImportObject tempStatObject = new();

            tempStatObject.StatType = StatsFuncs.GetStatType(tempFile.FileName);

            // TODO: need a function to add intro lines and a way to ignore those in the StatLineParser() func
            // make it change based on StatType
            // tempStatObject.DataLines =

            tempStatObject.DataLines = FileImporter.StatLineParser(tempStatObject.StatType, fieldNodes);

            // Console.WriteLine(tempStatObject.StatType);

            foreach (string line in tempStatObject.DataLines)
            {
                // Console.WriteLine(line);
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
        public static Queue<string>? StatLineParser(string StatType, XmlNode fieldNodes)
        // public static ImportObject? StatLineParser(string file, IEnumerator statsNodes)
        {
            Queue<string> tempStatQueue = new();

            for (int i = 0; i < fieldNodes.ChildNodes.Count; i++) 
            {
                for (int j = 0; j < fieldNodes.ChildNodes[i].Attributes.Count; j++)
                {
                    string attrName = fieldNodes.ChildNodes[i].Attributes[j].Name;
                    var attrValue = ValueFilter(fieldNodes.ChildNodes[i].Attributes[j].Value);

                    if (attrValue != null )
                    {
                        tempStatQueue.Enqueue(attrValue);
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

