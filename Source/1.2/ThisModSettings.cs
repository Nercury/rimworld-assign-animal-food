using UnityEngine;
using Verse;

namespace AssignAnimalFood
{
    public class ThisModSettings : ModSettings
    {
        public bool preventEatingDrugs = true;
        public bool preventEatingHarvestablesFromAreas = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref preventEatingDrugs, "assignAnimalFoodPreventEatingAddictiveDrugs", true);
            Scribe_Values.Look(ref preventEatingHarvestablesFromAreas, "preventEatingHarvestablesFromAreasDesignatedToCut", true);
        }

        public bool ManagesGrowingPlants => preventEatingHarvestablesFromAreas;
    }

    public class ThisMod : Mod
    {
        ThisModSettings settings;

        public static string FoodRestrictionAnimal
            => "FoodRestrictionAnimal".TryTranslate(out TaggedString t) ? t.RawText : "Animal";
        public static string AssignAnimalFood
            => "AssignAnimalFood".TryTranslate(out TaggedString t) ? t.RawText : "Assign Animal Food";
        public static string PreventEatingDrugs
            => "AssignAnimalFoodPreventEatingDrugs".TryTranslate(out TaggedString t) ? t.RawText : "Prevent animals from eating drugs";
        public static string PreventEatingFromGrowAreas
            => "AssignAnimalFoodPreventEatingHarvestablesFromAreas".TryTranslate(out TaggedString t) ? t.RawText : "Prevent eating harvestables from grow areas";

        public ThisMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ThisModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled(PreventEatingDrugs, ref settings.preventEatingDrugs);
            listingStandard.CheckboxLabeled(PreventEatingFromGrowAreas, ref settings.preventEatingHarvestablesFromAreas);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return AssignAnimalFood;
        }
    }
}
