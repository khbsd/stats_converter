using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stats_converter
{
    public class StatsData
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

        public static string[] PeskyFields =
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
    }

    internal class StatsFuncs
    {
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

        public static string GetStatType(string name)
        {
            foreach (string type in StatsData.StatTypes)
            {
                if (name.Contains(type))
                {
                    if (name.Contains("Interrupt") || name.Contains("Passive") || (name.Contains("Status") || StatsFuncs.NameContains(StatsData.StatusTypes, name)))
                    {
                        return '"' + type + "Data\"";
                    }
                    return '"' + type + '"';
                }
                else if (StatsData.SpellTypes.Contains(name) || StatsFuncs.NameContains(StatsData.SpellTypes, name))
                {
                    return "\"SpellData\"";
                }
            }
            return name;
        }

        public static int GroupAmount(string name)
        {
            if (StatsData.DescDisp.Contains(name))
            {
                return 3;
            }
            return 2;
        }
    }
}
