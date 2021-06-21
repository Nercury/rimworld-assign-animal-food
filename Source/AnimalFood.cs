using HarmonyLib;
using Verse;

namespace AssignAnimalFood
{
    [StaticConstructorOnStartup]
    public static class AssignAnimalFoodMod
    {
        static AssignAnimalFoodMod()
        {
            new Harmony(AnimalMod.Id).PatchAll();
            Logger.Message("Initialized");
        }
    }
}