using UnityEngine;
using Verse;

namespace AssignAnimalFood
{
    public class ThisModSettings : ModSettings
    {
        public bool preventEatingAddictiveDrugs = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref preventEatingAddictiveDrugs, "assignAnimalFoodPreventEatingAddictiveDrugs", true);
        }
    }

    public class ThisMod : Mod
    {
        ThisModSettings settings;

        public static string FoodRestrictionAnimal
            => "FoodRestrictionAnimal".TryTranslate(out TaggedString t) ? t.RawText : "Animal";
        public static string AssignAnimalFood
            => "AssignAnimalFood".TryTranslate(out TaggedString t) ? t.RawText : "Assign Animal Food";
        public static string PreventEatingAddictiveDrugs
            => "AssignAnimalFoodPreventEatingAddictiveDrugs".TryTranslate(out TaggedString t) ? t.RawText : "Prevent animals from eating addictive drugs";

        public ThisMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ThisModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled(PreventEatingAddictiveDrugs, ref settings.preventEatingAddictiveDrugs);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return AssignAnimalFood;
        }
    }
}
