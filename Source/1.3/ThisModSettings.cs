using UnityEngine;
using Verse;

namespace AssignAnimalFood
{
    public class ThisModSettings : ModSettings
    {
        public bool preventEatingDrugs = true;
        public bool preventEatingHarvestablesFromAreasDesignatedToCut = true;
        public bool preventEatingHarvestablesFromAreasNotDesignatedToCut = false;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref preventEatingDrugs, "assignAnimalFoodPreventEatingAddictiveDrugs", true);
            Scribe_Values.Look(ref preventEatingHarvestablesFromAreasDesignatedToCut, "preventEatingHarvestablesFromAreasDesignatedToCut", true);
            Scribe_Values.Look(ref preventEatingHarvestablesFromAreasNotDesignatedToCut, "preventEatingHarvestablesFromAreasNotDesignatedToCut", false);
        }

        public bool ManagesGrowingPlants => preventEatingHarvestablesFromAreasDesignatedToCut || preventEatingHarvestablesFromAreasNotDesignatedToCut;
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
        public static string PreventEatingFromGrowAreasDesignatedToCut
            => "AssignAnimalFoodPreventEatingHarvestablesFromAreasDesignatedToCut".TryTranslate(out TaggedString t) ? t.RawText : "Prevent eating harvestables from areas designated to cut";
        public static string PreventEatingFromGrowAreasNotDesignatedToCut
            => "AssignAnimalFoodPreventEatingHarvestablesFromAreasNotDesignatedToCut".TryTranslate(out TaggedString t) ? t.RawText : "Prevent eating harvestables from areas not designated to cut";

        public ThisMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ThisModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled(PreventEatingDrugs, ref settings.preventEatingDrugs);
            listingStandard.CheckboxLabeled(PreventEatingFromGrowAreasDesignatedToCut, ref settings.preventEatingHarvestablesFromAreasDesignatedToCut);
            listingStandard.CheckboxLabeled(PreventEatingFromGrowAreasNotDesignatedToCut, ref settings.preventEatingHarvestablesFromAreasNotDesignatedToCut);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return AssignAnimalFood;
        }
    }
}
