using Verse;

namespace AssignAnimalFood
{
    public class AnimalMod
    {
        public const string Id = "Everyone.AssignAnimalFood";
        public const string Name = "Assign Animal Food";
        public const string Version = "1.0";

        public static string FoodRestrictionAnimal 
            => "FoodRestrictionAnimal".TryTranslate(out TaggedString t) ? t.RawText : "Animal";
    }
}
