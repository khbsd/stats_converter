using System.Xml;
using System.Collections;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;


namespace stats_converter
{
    public class StatFields
    {
        public OrderedDictionary<string, string?>? Dict { get; set; }
    }


    public class StatLines
    {
        public OrderedDictionary<string, StatFields>? Dict { get; set; }
    }


    public class StatObject
    {
        public OrderedDictionary<string, StatLines>? Dict { get; set; }
    }


    public class ImportObject
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? StatType { get; set; }

        public StatObject? RawStats { get; set; }

        // new/key, type, and using lines
        // if it's an ItemCombinationResult, will contain the entirety of the object
        public StatFields? ObjHeader { get; set; }

        // if value is 3, it means > 2 lines
        public int? LineCount { get; set; }

        // remember to use foreach for these
        // all of the data lines
        public OrderedDictionary<string, List<string?>?>? ObjBody { get; set; }
        public OrderedDictionary<string, List<string?>?>? ObjFooter { get; set; }

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
        public static StatFields BuildStatsHeader(List<string>? rawDataList, string StatType)
        {
            StatFields tempObjHeader = new();

            tempObjHeader.Dict.TryAdd("type", StatType);

            for (int i = 0; i < rawDataList.Count; i++)
            {
                if (rawDataList[i] == "Name")
                {
                    tempObjHeader.Dict.Insert(0, "new entry", StatsFuncs.AddQuotes(rawDataList[i + 2]));

                }
                else if (rawDataList[i] == "Using")
                {
                    tempObjHeader.Dict.Insert(2, "using", StatsFuncs.AddQuotes(rawDataList[i + 2]));
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
                    (FileImporter.ValueFilter(rawDataList[i]) != null &&
                    FileImporter.ValueFilter(rawDataList[i + 2]) != null)
                )
                {

                }
            }

            return tempObjBody;
        }


        public static OrderedDictionary<string, List<string?>?> BuildStatsFooter(List<string>? rawDataList, string StatType)
        {
            OrderedDictionary<string, List<string?>?> tempObjFooter = new();
            List<string?> tempList = new();
            tempObjFooter.TryAdd("newline", tempList);

            tempObjFooter["newline"].Add("\n");

            return tempObjFooter;

        }


        // public static void BuildImportObject(ImportFile tempFile)
        public static ImportObject BuildImportObject(ImportFile tempFile, XmlNode fieldNodes)
        {
            ImportObject tempStatObject = new();

            tempStatObject.StatType = StatsFuncs.GetStatType(tempFile.FileName);
            tempStatObject.RawStats = FileImporter.StatLineParser(tempStatObject.StatType, fieldNodes);
            
            
            foreach (string Obj in tempStatObject.RawStats.Dict.Keys)
            {
                Console.WriteLine(Obj);
                
                foreach (string stat in tempStatObject.RawStats.Dict[Obj].Dict.Keys)
                {
                    Console.WriteLine(stat);

                    foreach (string value in tempStatObject.RawStats.Dict[Obj].Dict[stat].Dict.Values)
                    {
                        Console.WriteLine(value);
                    }
                }
            }
            /* 
            
            tempStatObject.ObjHeader = BuildStatsHeader(tempStatObject.RawStats, tempStatObject.StatType);
            tempStatObject.ObjBody = BuildStatsBody(tempStatObject.RawStats);
            tempStatObject.ObjFooter = BuildStatsFooter(tempStatObject.RawStats, tempStatObject.StatType);

            foreach (KeyValuePair<string, string?> headerLine in tempStatObject.ObjHeader)
            {
                Console.WriteLine(headerLine.Key + " " + headerLine.Value);
            }

            foreach (KeyValuePair<string, List<string?>?> bodyLine in tempStatObject.ObjBody)
            {
                foreach (string? value in bodyLine.Value)
                {
                    Console.WriteLine(bodyLine.Key + " " + value);
                }
            }
            foreach (KeyValuePair<string, List<string?>?> footerLine in tempStatObject.ObjFooter)
            {
                foreach (string? value in footerLine.Value)
                {
                    Console.WriteLine(value);
                }
            }*/

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


        public static string? GetObjectName(XmlAttributeCollection attributes)
        {
            for (int i = 0; i < attributes.Count; i++)
            {
                if (attributes[i].Value.Equals("Name"))
                {
                    return attributes[i + 2].Value;
                }
                
            }
            return null;
        }


        // make this a yield? nah
        public static StatObject StatLineParser
        (
            string StatType, XmlNode fieldNodes
        )
        {
            StatObject tempStatDict = new();

            string? statObjectName = "";

            for (int i = 0; i < fieldNodes.ChildNodes.Count; i++) 
            {
                int nameCounter = 0;
                while (string.IsNullOrEmpty(statObjectName))
                {
                    statObjectName = GetObjectName(fieldNodes.ChildNodes[nameCounter].Attributes);
                    // Console.WriteLine(statObjectName);
                    nameCounter++;
                }
                
                for (int j = 0; j < fieldNodes.ChildNodes[i].Attributes.Count; j++)
                { 
                    var attrInitValue = fieldNodes.ChildNodes[i].Attributes[j].Value;
                    var attrInitName = fieldNodes.ChildNodes[i].Attributes[j].Name;

                    int fieldSize = StatsFuncs.GetFieldCount(attrInitValue);

                    if (attrInitValue != null)
                    {
                        if (attrInitName == "name")
                        {
                            string statLineName = attrInitValue;

                            StatLines tempLineDict = new();
                            tempLineDict.Dict.TryAdd(statLineName, tempFieldDict);

                            // Console.WriteLine(statLineName);

                            for (int k = 1; k < fieldSize; k++)
                            {
                                if (j + k <  fieldNodes.ChildNodes[i].Attributes.Count)
                                {
                                    var subAttrValue = fieldNodes.ChildNodes[i].Attributes[j + k].Value;
                                    var subAttrName = fieldNodes.ChildNodes[i].Attributes[j + k].Name;

                                    // Console.WriteLine(subAttrName + ": " + subAttrValue);

                                    tempFieldDict.TryAdd(subAttrName, subAttrValue);
                                }
                                    
                            }

                            j = fieldSize;
                        }
                        
                    }
                }

                tempStatDict.TryAdd(statObjectName, tempLineDict);
            }

            if (tempStatDict.Dict.Count > 0)
            {
                return tempStatDict;
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

