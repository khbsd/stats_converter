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
        public string? TypeLine { get; set; }
        public int? LineCount { get; set; }
        public string[]? DataLines { get; set; }

    }


    public class ImportFile
    {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? StatType { get; set; }
        public XmlDocument XmlStatsFile = new();
        public ImportObject[]? StatsObject { get; set; }
    }


    public class StatsImporter
    {
        public static ImportObject? BuildImportObject(ImportFile tempFile)
        {
            ImportObject tempStatObject = new();
            tempFile.XmlStatsFile.Load(tempFile.FilePath);
            IEnumerator statsNodes = tempFile.XmlStatsFile.DocumentElement.FirstChild.GetEnumerator();

            // building code
            // Console.WriteLine(file);

            tempStatObject.StatType = StatsFuncs.GetStatType(tempFile.FileName);

            Console.WriteLine(tempStatObject.StatType);


            return null;


            // return tempStatObject;
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

        // make this a yield?
        public static void FileParser(string file, IEnumerator statsNodes)
        // public static ImportObject? FileParser(string file, IEnumerator statsNodes)
        {
            while (statsNodes.MoveNext())
            {
                int nodeIndex = 0;
                ImportObject tempStatObject = new();
                XmlNode? statsNode = (XmlNode)statsNodes.Current;

                tempStatObject = ConfigureStatObject(file, tempStatObject);
                

                foreach (XmlNode fieldsNode in statsNode.ChildNodes)
                {
                    // Console.WriteLine(fieldsNode.Name);

                    for (int i = 0; i < fieldsNode.ChildNodes.Count; i++) 
                    {
                        for (int j = 0; j < fieldsNode.ChildNodes[i].Attributes.Count; j++)
                        {
                            string attrName = fieldsNode.ChildNodes[i].Attributes[j].Name.ToString();
                            var attrValue = ValueFilter(fieldsNode.ChildNodes[i].Attributes[j].Value.ToString());

                            if (attrValue != null)
                            {
                                // Console.WriteLine(attrValue);
                            }

                        }

                    }
                    
                }

                if (tempStatObject != null)
                {
                    // return tempStatObject;
                }
            }

            // return null;
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
                tempFile.FilePath = file;



                try
                {
                    // var tempParsedFile = FileImporter.FileParser(file, statsNodes);
                    StatsImporter.BuildImportObject(tempFile);

                    /*if (tempParsedFile != null)
                    {
                        tempFile.StatsObject.Append(tempParsedFile);
                    } */   
                }
                catch { }
                
            }
        }
    }
}

