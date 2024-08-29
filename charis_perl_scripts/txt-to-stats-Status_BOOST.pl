#https://perlmaven.com/

#Description: Automatic conversion script from .txt to .stats
#Script author: Charis Legenaire
#Date: 20-07-2024
use 5.010;
use strict;
use warnings;
 
say "Converting Status_BOOST.txt to Status_BOOST.stats";

my $stat_object_definition_id = "e2a8d59b-0e34-4a7c-bf5f-db7a2bb34cde";

my $fileRead = 'Status_BOOST.txt';
open(my $fhRead, '<:encoding(UTF-8)', $fileRead)
  or die "Could not open file '$fileRead' $!";

my $fileWrite = 'Status_BOOST.stats';
open(my $fhWrite, '>', $fileWrite)
  or die "Could not open file '$fileWrite' $!";

#Header
print $fhWrite "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
print $fhWrite "<stats stat_object_definition_id=\"$stat_object_definition_id\">\n";
print $fhWrite "  <stat_objects>\n";
print $fhWrite "    <stat_object is_substat=\"false\">\n";
print $fhWrite "      <fields>\n";

while (my $row = <$fhRead>) { #read each line
  if ($row eq "\n"){
    print $fhWrite "      </fields>\n";
    print $fhWrite "    </stat_object>\n";
    print $fhWrite "    <stat_object is_substat=\"false\">\n";
    print $fhWrite "      <fields>\n";
  }
  if (substr($row, 0, 10) eq "new entry "){
	my $sub_string = substr($row, 10);
	chomp $sub_string;
    print $fhWrite "        <field name=\"Name\" type=\"NameTableFieldDefinition\" value=$sub_string />\n";
  }
  #if (substr($row, 0, 6) eq "using "){ #does not support names, has to be the UUID
  #	my $sub_string = substr($row, 6);
  #	chomp $sub_string;
  #  print $fhWrite "        <field name=\"Using\" type=\"BaseClassTableFieldDefinition\" value=$sub_string />\n";
  #}
  if (substr($row, 0, 5) eq "data "){
	my $nameindexend = index($row, '"', 7);
	my $fieldname = substr($row, 5, ($nameindexend - 4));
	if ($fieldname ne "\"SpellType\"" && $fieldname ne "\"StatusType\"") {
	  if ($fieldname eq "\"SpellAnimationIntentType\"") { #exception: SpellAnimationIntentType -> AnimationIntentType
		  $fieldname = "\"AnimationIntentType\"";
	  }
	  print $fhWrite "        <field name=$fieldname";
	  
	  my $value = substr($row, $nameindexend + 2);
	  chomp $value;
		  
	  #field types
	  my @IdTableFieldDefinition = ( #not data
		"\"UUID\"" #<field name="UUID" type="IdTableFieldDefinition" value="09a57b46-bdd5-4b2c-b87d-e5b91e3589a4" />
	  );
	  my @NameTableFieldDefinition = ( #not data
		"\"Name\"" #<field name="Name" type="NameTableFieldDefinition" value="Name" />
	  );
	  my @BaseClassTableFieldDefinition = ( #not data
		"\"Using\"" #<field name="Using" type="BaseClassTableFieldDefinition" value="4725df1b-a2d0-422a-8770-1b470f6bd689" />
	  );
	  my @CommentTableFieldDefinition = (
		"\"Owner\"" #<field name="Owner" type="CommentTableFieldDefinition" value="Test" />
	  );
	  my @IntegerTableFieldDefinition = (
		"\"Level\"", #<field name="Level" type="IntegerTableFieldDefinition" value="0" />
		"\"AreaRadius\"",
		"\"ExplodeRadius\"",
		"\"ProjectileDelay\"",
		"\"CastTargetHitDelay\"",
		"\"Angle\"",
		"\"SpellActionTypePriority\"",
		"\"ForkChance\"",
		"\"MaxForkCount\"",
		"\"ForkLevels\"",
		"\"SpellPrepareCost\"",
		"\"PowerLevel\"",
		"\"DelayTurnsCount\"",
		"\"DelayRollTarget\"",
		"\"MaximumTargets\"",
		"\"FXScale\"",
		"\"MaximumTotalTargetHP\"",
		"\"MaxDistance\"",
		"\"Lifetime\"",
		"\"SurfaceRadius\"",
		"\"SurfaceLifetime\"",
		"\"SurfaceGrowStep\"",
		"\"SurfaceGrowInterval\"",
		"\"FrontOffset\"",
		"\"Range\"",
		"\"Base\"",
		"\"Angle\"",
		"\"MaterialFadeAmount\"",
		"\"MaterialOverlayOffset\"",
		"\"StackPriority\"",
		"\"AuraRadius\""
	  );
	  my @EnumerationTableFieldDefinition = (
		"\"SpellSchool\"", #<field name="SpellSchool" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="SpellSchool" version="1" />
		"\"Cooldown\"", #<field name="Cooldown" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="CooldownType" version="1" />
		"\"DeathType\"", #<field name="DeathType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="Death Type" version="1" />
		"\"PreviewCursor\"", #<field name="PreviewCursor" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="CursorMode" version="1" />
		"\"ProjectileTerrainOffset\"", #<field name="ProjectileTerrainOffset" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"ProjectileType\"", #<field name="ProjectileType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="ProjectileType" version="1" />
		"\"VerbalIntent\"", #<field name="VerbalIntent" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="VerbalIntent" version="1" />
		"\"SpellStyleGroup\"", #<field name="SpellStyleGroup" type="EnumerationTableFieldDefinition" value="Class" enumeration_type_name="SpellStyleGroup" version="1" />
		"\"SpellActionType\"", #<field name="SpellActionType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="SpellActionType" version="1" />
		"\"HitAnimationType\"", #<field name="HitAnimationType" type="EnumerationTableFieldDefinition" value="Default" enumeration_type_name="HitAnimationType" version="1" />
		"\"AnimationIntentType\"", #<field name="AnimationIntentType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="SpellAnimationIntentType" version="1" />
		"\"SpellJumpType\"", #<field name="SpellJumpType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="SpellJumpType" version="1" />
		"\"DamageType\"", #<field name="DamageType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="Damage Type" version="1" />
		"\"DelayRollDie\"", #<field name="DelayRollDie" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="DieType" version="1" />
		"\"SpellSoundMagnitude\"", #<field name="SpellSoundMagnitude" type="EnumerationTableFieldDefinition" value="Normal" enumeration_type_name="SpellSoundMagnitude" version="1" />
		"\"Sheathing\"", #<field name="Sheathing" type="EnumerationTableFieldDefinition" value="Somatic" enumeration_type_name="SpellSheathing" version="1" />
		"\"Autocast\"", #<field name="Autocast" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"CinematicArenaFlags\"", #<field name="CinematicArenaFlags" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="CinematicArenaFlags" version="1" />
		"\"SpellAnimationType\"", #<field name="SpellAnimationType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="SpellAnimationType" version="1" />
		"\"SurfaceType\"", #<field name="SurfaceType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="Surface Type" version="1" />
		"\"FormatColor\"", #<field name="FormatColor" type="EnumerationTableFieldDefinition" value="White" enumeration_type_name="FormatStringColor" version="1" />
		"\"MaterialType\"", #<field name="MaterialType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="MaterialType" version="1" />
		"\"MaterialApplyBody\"", #<field name="MaterialApplyBody" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"MaterialApplyArmor\"", #<field name="MaterialApplyArmor" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"MaterialApplyWeapon\"", #<field name="MaterialApplyWeapon" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"MaterialApplyNormalMap\"", #<field name="MaterialApplyNormalMap" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"StillAnimationType\"", #<field name="StillAnimationType" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="StatusAnimationType" version="1" />
		"\"StillAnimationPriority\"", #<field name="StillAnimationPriority" type="EnumerationTableFieldDefinition" value="Suffocating" enumeration_type_name="StillAnimPriority" version="1" />
		"\"UseLyingPickingState\"", #<field name="UseLyingPickingState" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"SoundVocalStart\"", #<field name="SoundVocalStart" type="EnumerationTableFieldDefinition" value="NONE" enumeration_type_name="SoundVocalType" version="1" />
		"\"SoundVocalLoop\"", #<field name="SoundVocalLoop" type="EnumerationTableFieldDefinition" value="NONE" enumeration_type_name="SoundVocalType" version="1" />
		"\"SoundVocalEnd\"", #<field name="SoundVocalEnd" type="EnumerationTableFieldDefinition" value="NONE" enumeration_type_name="SoundVocalType" version="1" />
		"\"ImmuneFlag\"", #<field name="ImmuneFlag" type="EnumerationTableFieldDefinition" value="None" enumeration_type_name="AttributeFlags" version="1" />
		"\"StackType\"", #field name="StackType" type="EnumerationTableFieldDefinition" value="Stack" enumeration_type_name="StatusStackType" version="1" />
		"\"ForceStackOverwrite\"", #<field name="ForceStackOverwrite" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"Toggle\"", #<field name="Toggle" type="EnumerationTableFieldDefinition" value="No" enumeration_type_name="YesNo" version="1" />
		"\"TickType\"", #<field name="TickType" type="EnumerationTableFieldDefinition" value="StartTurn" enumeration_type_name="TickType" version="1" />
		"\"LEDEffect\"", #<field name="LEDEffect" type="EnumerationTableFieldDefinition" value="NONE" enumeration_type_name="LEDEffectType" version="1" />
		"\"ManagedStatusEffectType\"", #<field name="ManagedStatusEffectType" type="EnumerationTableFieldDefinition" value="Positive" enumeration_type_name="ManagedStatusEffectType" version="1" />
	  );
	  my @enumerationtable_type_name = (
		"\"SpellSchool\"",
		"\"CooldownType\"",
		"\"Death Type\"",
		"\"CursorMode\"",
		"\"YesNo\"",
		"\"ProjectileType\"",
		"\"VerbalIntent\"",
		"\"SpellStyleGroup\"",
		"\"SpellActionType\"",
		"\"HitAnimationType\"",
		"\"SpellAnimationIntentType\"",
		"\"SpellJumpType\"",
		"\"Damage Type\"",
		"\"DieType\"",
		"\"SpellSoundMagnitude\"",
		"\"SpellSheathing\"",
		"\"YesNo\"",
		"\"CinematicArenaFlags\"",
		"\"SpellAnimationType\"",
		"\"Surface Type\"",
		"\"FormatStringColor\"",
		"\"MaterialType\"",
		"\"YesNo\"",
		"\"YesNo\"",
		"\"YesNo\"",
		"\"YesNo\"",
		"\"StatusAnimationType\"",
		"\"StillAnimPriority\"",
		"\"YesNo\"",
		"\"SoundVocalType\"",
		"\"SoundVocalType\"",
		"\"SoundVocalType\"",
		"\"AttributeFlags\"",
		"\"StatusStackType\"",
		"\"YesNo\"",
		"\"YesNo\"",
		"\"TickType\"",
		"\"LEDEffectType\"",
		"\"ManagedStatusEffectType\""
	  );
	  my @StringTableFieldDefinition = (
		"\"SpellContainerID\"", #<field name="SpellContainerID" type="StringTableFieldDefinition" value="Test" />
		"\"ContainerSpells\"",
		"\"ConcentrationSpellID\"",
		"\"SpellProperties\"",
		"\"TargetCeiling\"",
		"\"TargetFloor\"",
		"\"TargetRadius\"",
		"\"AddRangeFromAbility\"",
		"\"AmountOfTargets\"",
		"\"SpellRoll\"",
		"\"SpellSuccess\"",
		"\"SpellFail\"",
		"\"TargetConditions\"",
		"\"ProjectileCount\"",
		"\"Trajectories\"",
		"\"Icon\"",
		"\"DescriptionParams\"",
		"\"ExtraDescriptionParams\"",
		"\"ShortDescriptionParams\"",
		"\"TooltipDamageList\"",
		"\"TooltipAttackSave\"",
		"\"TooltipStatusApply\"",
		"\"TooltipOnMiss\"",
		"\"TooltipOnSave\"",
		"\"TooltipUpcastDescriptionParams\"",
		"\"TooltipPermanentWarnings\"",
		"\"PrepareSound\"",
		"\"PrepareLoopSound\"",
		"\"CastSound\"",
		"\"VocalComponentSound\"",
		"\"CastTextEvent\"",
		"\"AlternativeCastTextEvents\"",
		"\"InstrumentComponentPrepareSound\"",
		"\"InstrumentComponentLoopingSound\"",
		"\"InstrumentComponentCastSound\"",
		"\"InstrumentComponentImpactSound\"",
		"\"MovingObjectSummonTemplate\"",
		"\"AiCalculationSpellOverride\"",
		"\"CycleConditions\"",
		"\"UseCosts\"",
		"\"DualWieldingUseCosts\"",
		"\"HitCosts\"",
		"\"RechargeValues\"",
		"\"Requirements\"",
		"\"ForkingConditions\"",
		"\"RootSpellID\"",
		"\"RequirementConditions\"",
		"\"InterruptPrototype\"",
		"\"FollowUpOriginalSpell\"",
		"\"AoEConditions\"",
		"\"TargetSound\"",
		"\"CastEffectTextEvent\"",
		"\"WeaponBones\"",
		"\"RitualCosts\"",
		"\"HighlightConditions\"",
		"\"SpawnEffect\"",
		"\"Shape\"",
		"\"CycleConditions\"",
		"\"StatusEffectOverrideForItems\"",
		"\"StatusEffectOverride\"",
		"\"Material\"",
		"\"MaterialParameters\"",
		"\"SoundStart\"",
		"\"SoundLoop\"",
		"\"SoundStop\"",
		"\"OnApplyConditions\"",
		"\"StackId\"",
		"\"AuraStatuses\"",
		"\"SurfaceChange\"",
		"\"Spells\"",
		"\"Items\"",
		"\"WeaponOverride\"",
		"\"ResetCooldowns\"",
		"\"LeaveAction\"",
		"\"DieAction\"",
		"\"Boosts\"",
		"\"Passives\"",
		"\"TooltipSave\"",
		"\"TooltipDamage\"",
		"\"PerformEventName\""
	  );
	  my @TranslatedStringTableFieldDefinition = (
		"\"DisplayName\"", #<field name="DisplayName" type="TranslatedStringTableFieldDefinition" handle="he9fe2b39g1ae2g2899ga5ebg2183d8aad9d4" version="1" />
		"\"Description\"",
		"\"ExtraDescription\"",
		"\"ShortDescription\""
	  );
	  my @EnumerationListTableFieldDefinition = (
		"\"AIFlags\"", #<field name="AIFlags" type="EnumerationListTableFieldDefinition" value="CanNotUse" enumeration_type_name="AIFlags" version="1" />
		"\"WeaponType\"", #<field name="WeaponType" type="EnumerationListTableFieldDefinition" value="Ammunition;Light" enumeration_type_name="WeaponFlags" version="1" />
		"\"SpellFlags\"", #<field name="SpellFlags" type="EnumerationListTableFieldDefinition" value="IsAttack;HasHighGroundRangeExtension;RangeIgnoreVerticalThreshold;IsHarmful;CanDualWield;HasVerbalComponent" enumeration_type_name="SpellFlagList" version="1" />
		"\"SpellCategory\"", #<field name="SpellCategory" type="EnumerationListTableFieldDefinition" value="SC_None" enumeration_type_name="SpellCategoryFlags" version="1" />
		"\"RequirementEvents\"", #<field name="RequirementEvents" type="EnumerationListTableFieldDefinition" value="None" enumeration_type_name="StatusEvent" version="1" />
		"\"TooltipSpellDCAbilities\"", #<field name="TooltipSpellDCAbilities" type="EnumerationListTableFieldDefinition" value="Strength" enumeration_type_name="AbilityFlags" version="1" />
		"\"LineOfSightFlags\"", #<field name="LineOfSightFlags" type="EnumerationListTableFieldDefinition" value="AddSourceHeight" enumeration_type_name="LineOfSightFlags" version="1" />
		"\"AuraFlags\"", #<field name="AuraFlags" type="EnumerationListTableFieldDefinition" value="CanAffectInvisibleItems" enumeration_type_name="AuraFlags" version="1" />
		"\"RemoveEvents\"", #<field name="RemoveEvents" type="EnumerationListTableFieldDefinition" value="OnTurn" enumeration_type_name="StatusEvent" version="1" />
		"\"StatusPropertyFlags\"", #<field name="StatusPropertyFlags" type="EnumerationListTableFieldDefinition" value="Performing" enumeration_type_name="StatusPropertyFlags" version="1" />
		"\"StatusGroups\"" #<field name="StatusGroups" type="EnumerationListTableFieldDefinition" value="SG_Condition" enumeration_type_name="StatusGroupFlags" version="1" />
	  );
	  my @enumerationlist_type_name = (
		"\"AIFlags\"",
		"\"WeaponFlags\"",
		"\"SpellFlagList\"",
		"\"SpellCategoryFlags\"",
		"\"StatusEvent\"",
		"\"AbilityFlags\"",
		"\"LineOfSightFlags\"",
		"\"AuraFlags\"",
		"\"StatusEvent\"",
		"\"StatusPropertyFlags\"",
		"\"StatusGroupFlags\""
	  );
	  my @GuidObjectTableFieldDefinition = (
		"\"TooltipUpcastDescription\"", #<field name="PrepareEffect" type="GuidObjectTableFieldDefinition" value="f58b6b6e-0403-4078-9c72-ccf06632bcf0" />
		"\"PrepareEffect\"",
		"\"CastEffect\"",
		"\"TargetEffect\"",
		"\"HitEffect\"",
		"\"TargetHitEffect\"",
		"\"TargetGroundEffect\"",
		"\"PositionEffect\"",
		"\"BeamEffect\"",
		"\"SpellEffect\"",
		"\"SelectedCharacterEffect\"",
		"\"SelectedObjectEffect\"",
		"\"SelectedPositionEffect\"",
		"\"DisappearEffect\"",
		"\"ReappearEffect\"",
		"\"ImpactEffect\"",
		"\"AuraFX\"",
		"\"ApplyEffect\"",
		"\"StatusEffect\"",
		"\"StatusEffectOnTurn\"",
		"\"ManagedStatusEffectGroup\"",
		"\"EndEffect\"",
		"\"DynamicAnimationTag\""
	  );
	  my @CastAnimationsTableFieldDefinition = (
		"\"SpellAnimation\"", #<field name="SpellAnimation" type="CastAnimationsTableFieldDefinition" value="f920a0a6-f257-4ce4-8d17-2dcaa7bb7bbb,,;7e67bfd0-2fc2-4d10-bed5-cfda9e660de5,,;eb054308-7fce-4b85-bf4c-7a0031fda7ac,,;0b0dc35b-4953-45c0-a9eb-8d3fef5e798a,,;6ec808e1-e128-44ef-9361-a713bf86de8f,,;b2e9c771-3497-444c-b360-23b4441985a1,,;f920a0a6-f257-4ce4-8d17-2dcaa7bb7bbb,,;,,;,," />
		"\"DualWieldingSpellAnimation\""
	  );
	  my @FloatTableFieldDefinition = (
		"\"MinJumpDistance\"", #<field name="MinJumpDistance" type="FloatTableFieldDefinition" value="0" />
		"\"SteerSpeedMultipler\"",
		"\"SplatterDirtAmount\"",
		"\"SplatterBloodAmount\"",
		"\"SplatterSweatAmount\""
	  );
	  my @BoolTableFieldDefinition = (
		"\"SourceLimbIndex\"", #<field name="SourceLimbIndex" type="BoolTableFieldDefinition" value="True" />
		"\"IsUnique\""
	  );
	  my @FixedStringTableFieldDefinition = (
		"\"SpellSoundAftermathTrajectory\"", #<field name="SpellSoundAftermathTrajectory" type="FixedStringTableFieldDefinition" value="Test" />
		"\"ItemWall\"",
		"\"ItemWallStatus\"",
		"\"WallStartEffect\"",
		"\"WallEndEffect\"",
		"\"StatusSoundState\""
	  );
	  my @StatReferenceTableFieldDefinition = (
		"\"CombatAIOverrideSpell\"" #<field name="CombatAIOverrideSpell" type="StatReferenceTableFieldDefinition" value="Projectile_AI_HELPERS" />
	  );
	  my @TimelineResourceIdTableFieldDefinition = (
		"\"CinematicArenaTimelineOverride\"" #<field name="CinematicArenaTimelineOverride" type="TimelineResourceIdTableFieldDefinition" value="7f62b7da-718c-1527-5529-fef68bb6722f" />
	  );
	  my @RollConditionsTableFieldDefinition = (
		"\"RemoveConditions\"", #<field name="RemoveConditions" type="RollConditionsTableFieldDefinition" value="1" />
		"\"OnApplyRoll\"",
		"\"OnTickRoll\"",
		"\"OnRemoveRoll\""
	  );
	  my @FunctorsTableFieldDefinition = (
		"\"TickFunctors\"", #<field name="TickFunctors" type="FunctorsTableFieldDefinition" value="1" />
		"\"OnApplyFunctors\"",
		"\"OnRemoveFunctors\"",
		"\"OnApplySuccess\"",
		"\"OnApplyFail\"",
		"\"OnTickSuccess\"",
		"\"OnTickFail\"",
		"\"OnRemoveSuccess\"",
		"\"OnRemoveFail\""
	  );
	  
	  
	  foreach (@CommentTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"CommentTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@IntegerTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"IntegerTableFieldDefinition\" value=$value />\n";
		}
	  }
	  for my $i (0 .. $#EnumerationTableFieldDefinition) {
		if ($fieldname eq $EnumerationTableFieldDefinition[$i]) {
		  print $fhWrite " type=\"EnumerationTableFieldDefinition\" value=$value enumeration_type_name=$enumerationtable_type_name[$i] version=\"1\" />\n";
		}
	  }
	  foreach (@StringTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"StringTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@TranslatedStringTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"TranslatedStringTableFieldDefinition\" handle=$value version=\"1\" />\n";
		}
	  }
	  for my $i (0 .. $#EnumerationListTableFieldDefinition) {
		if ($fieldname eq $EnumerationListTableFieldDefinition[$i]) {
		  print $fhWrite " type=\"EnumerationListTableFieldDefinition\" value=$value enumeration_type_name=$enumerationlist_type_name[$i] version=\"1\" />\n";
		}
	  }
	  foreach (@GuidObjectTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"GuidObjectTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@CastAnimationsTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"CastAnimationsTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@FloatTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"FloatTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@BoolTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"BoolTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@FixedStringTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"FixedStringTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@StatReferenceTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"StatReferenceTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@TimelineResourceIdTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"TimelineResourceIdTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@RollConditionsTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"RollConditionsTableFieldDefinition\" value=$value />\n";
		}
	  }
	  foreach (@FunctorsTableFieldDefinition) {
		if ($fieldname eq $_) {
		  print $fhWrite " type=\"FunctorsTableFieldDefinition\" value=$value />\n";
		}
	  }
	}
  }
}

print $fhWrite "      </fields>\n";
print $fhWrite "    </stat_object>\n";
print $fhWrite "  </stat_objects>\n";
print $fhWrite "</stats>";

close $fhWrite;

print "done\n";