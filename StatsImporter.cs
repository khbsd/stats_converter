using System.Xml;
using System.Collections;
using System;
using System.Diagnostics.Contracts;


namespace stats_converter
{
    public class ImportObject
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? StatType { get; set; }
        public string? TypeLine { get; set; }
        public int? LineCount { get; set; }
        public string[]? DataLines { get; set; }

    }


    public class ImportFile
    {
        public string? FileName { get; set; }
        public string? StatType { get; set; }
        public XmlDocument XmlStatsFile = new();
        public ImportObject[]? StatsObject { get; set; }
    }


    public class StatsImporter
    {
        public static ImportObject BuildImportObject()
        {
            ImportObject tempObj = new();

            // building code


            return tempObj;
        }

    }


    public class FileImporter
    {
        // make this a yield?
        public static ImportObject FileParser(string file, IEnumerator statsNodes)
        {
            while (statsNodes.MoveNext())
            {
                ImportObject tempStatObject = new();
                XmlNode? statsNode;
                statsNode = (XmlNode)statsNodes.Current;

                // needs to hook into BuildImportObject

                if (statsNode.NextSibling != null)
                {
                    if (statsNode.NextSibling.HasChildNodes)
                    {
                        foreach (XmlElement line in statsNode.NextSibling.ChildNodes)
                        {
                            Console.WriteLine(line.InnerXml);
                        }

                    }

                }
                return tempStatObject;
            }
        }

        public static void Main()
        //public static void ImportStatsFile()
        {
            ImportFile tempFile = new();

            string StatsFolderPath = "..\\..\\..\\test_files\\";
            string[] StatsFiles = Array.ConvertAll(Directory.GetFiles(StatsFolderPath, "*.stats"), file => Path.GetFileName(file));
            // string[] StatsFiles = ["carry weight extra_Data.stats"];

            foreach (string file in StatsFiles)
            {
                // need to deal with StatsFolderPath being empty
                // this bit should be a separate function since a lot is happening
                tempFile.FileName = Path.GetFileNameWithoutExtension(file);

                tempFile.XmlStatsFile.Load(StatsFolderPath + file);
                IEnumerator statsNodes = tempFile.XmlStatsFile.DocumentElement.FirstChild.GetEnumerator();

                tempFile.StatsObject.Append(FileImporter.FileParser(file, statsNodes));

                
            }
        }
    }
}

