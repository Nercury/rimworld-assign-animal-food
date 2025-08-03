using HarmonyLib;
using Verse;

namespace AssignAnimalFood
{
    [StaticConstructorOnStartup]
    public static class AssignAnimalFoodMod
    {
        public static ThisModSettings Settings;

        static AssignAnimalFoodMod()
        {
            Settings = LoadedModManager.GetMod<ThisMod>().GetSettings<ThisModSettings>();

            new Harmony(AnimalMod.Id).PatchAll(); 
            Logger.Message("Initialized");
        }
    }
}