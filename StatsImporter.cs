﻿using System.Xml;
using System.Collections;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;

/*
[filename][object name][object stat line][stat field]

>> ImportObject.RawStats["Companion_Wolf_7"]["UUID"]["value"]

returns: f862e70a-8a86-4a30-9b07-caa46007366e

<field name="UUID" type="IdTableFieldDefinition" value="f862e70a-8a86-4a30-9b07-caa46007366e" />
<field name="Name" type="NameTableFieldDefinition" value="Companion_Wolf_7" />
<field name="Using" type="BaseClassTableFieldDefinition" value="a9781b07-b2cc-4154-b09c-cd6e2ea5bf55" />
<field name="Passives" type="StringTableFieldDefinition" value="HuntersMark_RangerCompanion;PackTactics;ShortResting;CompanionsBond_Creature" />
<field name="DifficultyStatuses" type="StringTableFieldDefinition" value="STATUS_EASY:PLAYER_BONUSES_EASYMODE" />
<field name="Dexterity" type="IntegerTableFieldDefinition" value="24" />
<field name="Constitution" type="IntegerTableFieldDefinition" value="21" />
<field name="Vitality" type="IntegerTableFieldDefinition" value="61" />
*/

namespace stats_converter
{
    /* public struct StatFields
    {
        public string FieldName;
        public string FieldValue;
        public OrderedDictionary<string, string?>? Field;
        public StatFields(string name, string value)
        {
            FieldName = name;
            FieldValue = value;
            Field.TryAdd(name, value);

        }
        
    }


    public struct StatLines
    {
        public string LineName;
        public StatFields? LineDict;
        public OrderedDictionary<string, StatFields?>? Line;

        public StatLines(string name, StatFields? dict)
        {
            LineName = name;
            LineDict = dict;
            Line.TryAdd(name, dict);
        }
    }


    public struct StatObject
    {
        public string ObjectName;
        public StatLines? ObjectValue;
        public OrderedDictionary<string, StatLines?>? Object;

        public StatObject(string name, StatLines? value)
        {
            ObjectName = name;
            ObjectValue = value;
            Object.TryAdd(ObjectName, ObjectValue);
        }

    }*/


public class ImportObject
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? StatType { get; set; }

        public OrderedDictionary<string, OrderedDictionary<string, OrderedDictionary<string, string?>?>?>? RawStats { get; set; }

        // new/key, type, and using lines
        // if it's an ItemCombinationResult, will contain the entirety of the object
        public OrderedDictionary<string, string?>? ObjHeader { get; set; }

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
        public static OrderedDictionary<string, string?>? BuildStatsHeader(List<string>? rawDataList, string StatType)
        {
            OrderedDictionary<string, string?>? tempObjHeader = new();

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

            /* 
            foreach (var Obj in tempStatObject.RawStats)
            {
                Console.WriteLine();
                
                foreach (string stat in tempStatObject.RawStats[Obj].Keys)
                {
                    Console.WriteLine(stat + ": " + tempStatObject.RawStats[Obj][stat]);

                    foreach (string key in tempStatObject.RawStats[Obj][stat].Keys)
                    {
                        Console.WriteLine(key + ": " + tempStatObject.RawStats[Obj][stat][key]);
                    }
                }
            }
            
            
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
        public static OrderedDictionary<string, OrderedDictionary<string, OrderedDictionary<string, string?>?>?>? StatLineParser
        (
            string StatType, XmlNode fieldNodes
        )
        {
            string? statObjectName = "";
            string statLineName = "";

            OrderedDictionary<string, OrderedDictionary<string, OrderedDictionary<string, string?>?>?>? tempStatDict = new();
            OrderedDictionary<string, OrderedDictionary<string, string?>?>? tempLineDict;
            OrderedDictionary<string, string?>? tempFieldDict;

            for (int i = 0; i < fieldNodes.ChildNodes.Count; i++) 
            {
                int nameCounter = 0;
                while (string.IsNullOrEmpty(statObjectName))
                {
                    statObjectName = GetObjectName(fieldNodes.ChildNodes[nameCounter].Attributes);
                    // Console.WriteLine(statObjectName);
                    nameCounter++;
                }

                tempLineDict = new();
                tempStatDict.TryAdd(statObjectName, tempLineDict);

                for (int j = 0; j < fieldNodes.ChildNodes[i].Attributes.Count; j++)
                {
                    var attrInitValue = fieldNodes.ChildNodes[i].Attributes[j].Value;
                    var attrInitName = fieldNodes.ChildNodes[i].Attributes[j].Name;

                    int fieldSize = StatsFuncs.GetFieldCount(attrInitValue);

                    if (attrInitValue != null)
                    {
                        tempFieldDict = new();

                        int k = 0;
                        while (k < fieldSize)
                        {
                            if (j == 0)
                            {
                                statLineName = attrInitValue;
                            }

                            if (j + k < fieldNodes.ChildNodes[i].Attributes.Count)
                            {
                                var subAttrValue = fieldNodes.ChildNodes[i].Attributes[j + k].Value;
                                var subAttrName = fieldNodes.ChildNodes[i].Attributes[j + k].Name;

                                if (string.IsNullOrEmpty(subAttrValue))
                                {
                                    subAttrValue = "\"\"";
                                }

                                if (!string.IsNullOrEmpty(statLineName))
                                {
                                    tempLineDict[statLineName].Add(subAttrName, subAttrValue);

                                    // Console.WriteLine(tempLineDict[statLineName][subAttrName]);
                                    // Console.WriteLine(tempLineDict[statLineName].GetAt(k));
                                }
                            }
                            // Console.WriteLine(j);
                            k++;
                        }
                    }
                    // Console.WriteLine(statObjectName);
                    
                }
            }

            // Console.WriteLine(tempStatDict["Companion_Wolf"]);

            if (tempStatDict.Count > 0)
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

                foreach (ImportObject obj in tempFile.StatsObjects)
                {
                    foreach (var pair in obj.RawStats)
                    {
                        // Console.WriteLine(pair.Key);
                        // Console.WriteLine(pair.Value);

                        foreach (var sub_pair in pair.Value)
                        {
                            Console.WriteLine(sub_pair.Key);
                            // Console.WriteLine(sub_pair.Value);
                        }
                    }
                }
            }
        }
    }
}