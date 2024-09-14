using System.Xml;
using System.Collections;
using System.IO.Enumeration;
using System;


public class StatsExporter
{
    // oh god the terror
}


public class StatsImporter_Old
{
    public static string[] IgnoreFields = 
    [
        "UUID", 
        "type", 
        "is_substat",
        "TranslatedStringTableFieldDefinition", 
        "StringTableFieldDefinition", 
        "NameTableFieldDefinition", 
        "IdTableFieldDefinition",
        "EnumerationListTableFieldDefinition",
        "BaseClassTableFieldDefinition",
        "EnumerationTableFieldDefinition",
        "GuidObjectTableFieldDefinition",
        "CastAnimationsTableFieldDefinition",
        "IntegerTableFieldDefinition",
        "CommentTableFieldDefinition",
        "StatReferenceTableFieldDefinition",
        "StatusIdsTableFieldDefinition",
        "RootTemplateTableFieldDefinition",
        "FloatTableFieldDefinition",
        "DiceTableFieldDefinition"
    ];

    public static string[] QuintFields =
    [
        "Properties",
        "PreviewCursor",
        "VerbalIntent",
        "SpellFlags",
        "SpellSchool",
        "HitAnimationType",
        "AnimationIntentType",
        "SpellStyleGroup",
        "DamageType",
        "Cooldown",
        "StatusPropertyFlags",
        "StatusGroups",
        "Rarity",
        "Autocast",
        "Weapon Group",
        "Weapon Properties",
        "Proficiency Group",
        "Damage Type",
        "IngredientType",
        "IngredientTransformType"
    ];

    public static string[] SpellTypes =
    [
        "Projectile",
        "ProjectileStrike",
        "Rush",
        "Shout",
        "SpellSet",
        "Target",
        "Teleportation",
        "Throw",
        "Wall",
        "Zone"
    ];

    public static string[] StatusTypes =
    [
        "BOOST",
        "DOWNED",
        "EFFECT",
        "FEAR",
        "INCAPACITATED",
        "INVISIBLE",
        "KNOCKED_DOWN",
        "POLYMORPHED"
    ];

    public static string[] DescDisp = 
    [
        "DisplayName", 
        "Description"
    ];

    public static string[] StatTypes =
    [
        "Status",
        "Object",
        "Interrupt",
        "Passive",
        "Weapon",
        "Character"
    ];

    // needs to be in an object :catyes:
    public static XmlDocument XmlStatsFile = new XmlDocument();
    public static string StatsFolderPath = "..\\..\\..\\test_files\\";
    public static List<string> StatsArray = [];
    public static string FileName = "";
    public static string ItemName = "";
    public static bool FirstEntry = true;


    public static int GroupAmount(string name)
    {
        if (DescDisp.Contains(name))
        {
            return 3;
        }
        return 2;
    }


    public static string GetStatType(string name)
    {
        foreach (string type in StatTypes)
        {
            if (name.Contains(type))
            {
                if (name.Contains("Interrupt") || name.Contains("Passive") || (name.Contains("Status") || NameContains(StatusTypes, name)))
                {
                    return '"' + type + "Data\"";
                }
                return '"' + type + '"';
            }
            else if (SpellTypes.Contains(name) || NameContains(SpellTypes, name))
            {
                return "\"SpellData\"";
            }
        }
        return name;
    }


    public static bool NameContains(string[] data, string name)
    {
        foreach (string type in data)
        {
            if (name.Contains(type))
            {
                return name.Contains(type);
            }
        }
        return false;
    }


    public static void StatsWriter(List<string> statsStrings)
    {
        if (!string.IsNullOrEmpty(FileName))
        {
            StatsWriter(statsStrings, FileName);
            return;
        }

        StatsWriter(statsStrings, "Stats");
    }

    public static void StatsWriter(List<string> statsStrings, string name)
    {
        string StatsFilePath = Path.Combine(StatsFolderPath, name + ".txt");
        using (StreamWriter StatsStream = new StreamWriter(StatsFilePath, append: true))
        {
            foreach (string statLine in statsStrings)
            {
                StatsStream.Write(statLine + Environment.NewLine);
            }
            
            StatsArray.Clear();
        }
    }


    public static void NodeParser(XmlNode node)
    {
        if (node.HasChildNodes)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                NodeParser(child);
            }
        }

        if (node.Attributes != null)
        {
            int i = 0;
            while (i < node.Attributes.Count)
            {
                XmlNode nodeAttr = node.Attributes[i];

                string fullField = "data ";
                string builtField = "";

                int attrTemp = 0;

                if (!IgnoreFields.Contains(nodeAttr.Name))
                {
                    for (int j = 0; j <= GroupAmount(nodeAttr.Value); j++)
                    {
                        if (i < node.Attributes.Count)
                        {
                            // these are separate as the attr variable above is "outdated"
                            var attrVal = node.Attributes[i].Value.ToString();
                            var attrName = node.Attributes[i].Name.ToString();

                            if (attrVal.Equals("UUID"))
                            {
                                i += 2;
                            }

                            if (!IgnoreFields.Contains(attrVal))
                            {
                                if (GroupAmount(nodeAttr.Value) == 3 && !IgnoreFields.Contains(node.Attributes[i + 1].Value.ToString()))
                                {
                                    builtField += '"' + attrVal + ";" + node.Attributes[i + 1].Value.ToString() + '"';
                                    i += 2;
                                }
                                else if (FileName.Contains("Combos"))
                                {
                                    if (attrVal.Equals("Name") || attrVal.Contains("Result"))
                                    {
                                        if (attrVal.Equals("Name"))
                                        {
                                            ItemName = node.Attributes[i + 2].Value.ToString();

                                            fullField = "new ItemCombination ";
                                            builtField += '"' + ItemName + '"';
                                        }
                                        else if (attrVal.Contains("Result"))
                                        {
                                            fullField = "new ItemCombinationResult ";
                                            builtField += '"' + ItemName + "_1" + '"';

                                            attrTemp = i;
                                        }

                                        i += 2;
                                        break;
                                    }
                                    else
                                    {
                                        builtField += '"' + attrVal + '"' + " ";
                                        j++;
                                    }
                                }
                                else if (FileName.Contains("XP") || FileName.Contains("Data"))
                                {
                                    if (!fullField.Contains("key "))
                                    {
                                        fullField = "key ";
                                    }

                                    if (attrVal.Equals("Name"))
                                    {
                                        builtField += '"' + node.Attributes[i + 2].Value.ToString() + '"' + ", ";
                                        builtField += '"' + node.NextSibling.Attributes[i + 2].Value.ToString() + '"';

                                        break;
                                    }
                                }
                                else if (FileName.Contains("TreasureTable"))
                                {
                                    int spawnFreq = 7;

                                    if (attrVal.Equals("Name") || attrVal.Equals("DropCount"))
                                    {
                                        if (attrVal.Equals("Name"))
                                        {
                                            fullField = "new treasuretable ";
                                        }
                                        else if (attrVal.Equals("DropCount"))
                                        {
                                            fullField = "new subtable ";
                                        }

                                        builtField += '"' + node.Attributes[i + 2].Value.ToString() + '"';
                                    }

                                    if (attrVal.Equals("ObjectCategory"))
                                    {
                                        fullField = "object category ";
                                        builtField += '"' + node.Attributes[i + 2].Value.ToString() + '"' + ",";
                                        builtField += node.NextSibling.Attributes[i + 2].Value.ToString() + ",";

                                        for (int k = 0; k < spawnFreq; k++)
                                        {
                                            builtField += "0";

                                            if (k < (spawnFreq - 1))
                                            {
                                                builtField += ",";
                                            }
                                        }
                                    }
                                    else if (attrVal.Equals("CanMerge"))
                                    {
                                        fullField = "";
                                        builtField += attrVal + " ";
                                        builtField += node.Attributes[i + 4].Value.ToString();

                                        i += 4;
                                        break;
                                    }

                                    i += 2;
                                    break;
                                }
                                else if (attrVal.Equals("Using"))
                                {
                                    fullField = "";
                                    builtField += attrVal.ToLower() + " ";
                                }
                                else if (attrVal.Equals("Name") && string.IsNullOrEmpty(fullField))
                                {
                                    fullField = "new entry ";
                                    builtField += '"' + node.Attributes[i + 2].Value.ToString() + '"';

                                    i += 2;
                                    break;
                                }
                                else if (QuintFields.Contains(attrVal.ToString()))
                                {
                                    builtField += '"' + attrVal + '"' + " " + '"' + node.Attributes[i + 2].Value.ToString() + '"';

                                    i += 4;
                                    break;
                                }
                                else
                                {
                                    builtField += '"' + attrVal + '"' + " ";
                                }

                                i++;
                            }

                            i++;
                        }
                    }

                    if (!string.IsNullOrEmpty(builtField))
                    {
                        if (fullField.Contains("new"))
                        {
                            if (FirstEntry)
                            {
                                FirstEntry = false;
                            }
                            else
                            {
                                StatsArray.Add(" ");
                            }
                        }

                        fullField += builtField;
                        StatsArray.Add(fullField);
                        string typeData = "data ";

                        // this could be a loop but like. i don't wanna
                        // nvm i made it a loop heh
                        if (fullField.Contains("new entry") || fullField.Contains("ItemCombinationResult"))
                        {
                            string typeField = "";

                            if (!fullField.Contains("ItemCombinationResult"))
                            {
                                typeField = "type ";
                                typeField += GetStatType(FileName);
                            }

                            if (typeField.Contains("StatusData"))
                            {
                                typeData += "\"StatusType\"" + " ";

                                foreach (string status in StatusTypes)
                                {
                                    if (FileName.Contains(status))
                                    {
                                        typeData += '"' + status + '"';
                                    }
                                }
                            }

                            else if (typeField.Contains("SpellData"))
                            {
                                typeData += "\"SpellType\"" + " ";

                                foreach (string spellType in SpellTypes)
                                {
                                    if (FileName.Contains(spellType))
                                    {
                                        typeData += '"' + spellType + '"';
                                    }
                                }
                            }
                            else if (fullField.Contains("ItemCombinationResult"))
                            {
                                typeData += '"' + node.Attributes[attrTemp].Value.ToString() + '"' + " " + '"' + node.Attributes[attrTemp + 2].Value.ToString() + '"';
                                StatsArray.Add(typeData);

                                break;
                            }

                            if (typeData.Equals("data "))
                            {
                                StatsArray.Add(typeField);
                            }
                            else if (typeField.Equals("type "))
                            {
                                StatsArray.Add(typeData);
                            }
                            else
                            {
                                StatsArray.Add(typeField);
                                StatsArray.Add(typeData);
                            }
                        }
                    }
                }

                i++;
            }
        }
    }


    public static void OldMain()
    {
        string StatsFolderPath = "..\\..\\..\\test_files\\";
        string[] StatsFiles = Array.ConvertAll(Directory.GetFiles(StatsFolderPath, "*.stats"), file => Path.GetFileName(file));
        // string[] StatsFiles = ["carry weight extra_Data.stats"];

        foreach (string file in StatsFiles)
        {
            XmlStatsFile.Load(StatsFolderPath + file);
            XmlNode statsRoot = XmlStatsFile.DocumentElement;
            IEnumerator statsNodes = statsRoot.GetEnumerator();
            XmlNode statsNode;

            while (statsNodes.MoveNext())
            {
                FileName = Path.GetFileNameWithoutExtension(file);
                statsNode = (XmlNode) statsNodes.Current;

                NodeParser(statsNode);
                StatsWriter(StatsArray);
            }
        }
    }
}

