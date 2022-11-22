using UnityEngine;
using Verse;

namespace AssignAnimalFood
{
    public class ThisModSettings : ModSettings
    {
    }

    public class ThisMod : Mod
    {
        ThisModSettings settings;

        public static string FoodRestrictionAnimal
            => "FoodRestrictionAnimal".TryTranslate(out TaggedString t) ? t.RawText : "Animal";
        public static string AssignAnimalFood
            => "AssignAnimalFood".TryTranslate(out TaggedString t) ? t.RawText : "Assign Animal Food";

        public ThisMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ThisModSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label($"Settings were removed in 1.4 version, because they are either not relevant or too complex to maintain.");
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return AssignAnimalFood;
        }
    }
}
