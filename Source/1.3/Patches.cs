using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace AssignAnimalFood
{
	[HarmonyPatch(typeof(PawnComponentsUtility), "AddAndRemoveDynamicComponents")]
	internal static class RimWorld_PawnComponentsUtility_AddAndRemoveDynamicComponents
	{
		private static void Postfix(Verse.Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike) return;
			if (pawn.foodRestriction != null) return;

			if (pawn.Faction == null || !pawn.Faction.IsPlayer)
			{
				if (pawn.HostFaction == null || !pawn.HostFaction.IsPlayer)
				{
					return;
				}
			}

			pawn.foodRestriction = new Pawn_FoodRestrictionTracker(pawn);

			var animalFoodRestriction = Current.Game.foodRestrictionDatabase.GetAnimalFoodRestriction();
			if (animalFoodRestriction != null)
			{
				pawn.foodRestriction.CurrentFoodRestriction = animalFoodRestriction;
			}
			else
			{
				Logger.Message($"It's unclear what food restriction to set for {pawn.Name} ({pawn.GetUniqueLoadID()}), \"Animal\" food restriction most likely was deleted. " +
					$"To resolve this, create a new food restriction named \"Animal\", and reload the game. " +
					$"After that, you can rename the \"Animal\" if needed.");
			}
		}
	}

	[HarmonyPatch(typeof(Pawn_FoodRestrictionTracker), "Configurable", MethodType.Getter)]
	internal static class RimWorld_Pawn_FoodRestrictionTracker_Configurable
	{
		private static void Postfix(ref Pawn_FoodRestrictionTracker __instance, ref bool __result)
		{
			__result = __result 
				|| (!__instance.pawn.Destroyed && !__instance.pawn.RaceProps.Humanlike && (__instance.pawn.Faction == Faction.OfPlayer || __instance.pawn.HostFaction == Faction.OfPlayer));
		}
	}

	[HarmonyPatch(typeof(FoodRestrictionDatabase), "TryDelete")]
	internal static class RimWorld_FoodRestrictionDatabase_TryDelete
	{
		private static void Postfix(ref FoodRestrictionDatabase __instance, ref AcceptanceReport __result)
		{
			if (__result.Accepted)
			{
				Current.Game.foodRestrictionDatabase.RemoveAnimalRestrictionIdIfMissing();
			}
		}
	}

	[HarmonyPatch(typeof(PawnUtility), "TrySpawnHatchedOrBornPawn")]
	internal static class RimWorld_PawnUtility_TrySpawnHatchedOrBornPawn
	{
		private static void Postfix(Pawn pawn, Thing motherOrEgg, ref bool __result)
		{
			if (__result)
			{
				if (pawn != null && motherOrEgg != null)
				{
					var motherPawn = motherOrEgg as Pawn;
					if (motherPawn != null)
					{
						//Logger.Message($"Congrats {motherOrEgg.Label} for giving birth to {pawn.Label}");
						if (pawn.foodRestriction != null && motherPawn.foodRestriction != null)
						{
							pawn.foodRestriction.CurrentFoodRestriction = motherPawn.foodRestriction.CurrentFoodRestriction;
						}

						return;
					}

					var compHatcher = motherOrEgg.TryGetComp<CompHatcher>();
					if (compHatcher?.hatcheeParent != null)
                    {
						//Logger.Message($"Congrats {compHatcher?.hatcheeParent.Label} for hatching {pawn.Label}");
						if (pawn.foodRestriction != null && compHatcher?.hatcheeParent?.foodRestriction != null)
						{
							pawn.foodRestriction.CurrentFoodRestriction = compHatcher?.hatcheeParent?.foodRestriction.CurrentFoodRestriction;
						}
					} 
					else
                    {
						Logger.Message($"{motherOrEgg.Label} ({motherOrEgg.GetUniqueLoadID()}) compHatcher not found");
					}
				}
			}
		}
	}

    [HarmonyPatch(typeof(FoodUtility), "BestFoodSourceOnMap")]
    internal static class RimWorld_FoodUtility_BestFoodSourceOnMap
    {
        private static bool Prefix(Pawn eater, ref bool allowDrug, ref bool allowHarvest, ref bool allowPlant)
        {
            if (AssignAnimalFoodMod.Settings.preventEatingDrugs)
            {
                if (eater != null)
                {
                    if (eater.RaceProps != null
                       && eater.RaceProps.Animal
                       && ((eater.Faction != null && eater.Faction.IsPlayer) || (eater.HostFaction != null && eater.HostFaction.IsPlayer)))
                    {
                        //Logger.Message($"Preventing {eater.Label} eating drugs");
                        allowDrug = false;
                    }
                }
            }

            return true;
        }
    }

	[HarmonyPatch(typeof(FoodUtility), "WillEat", new Type[] { typeof(Pawn), typeof(Thing), typeof(Pawn), typeof(bool) })]
	internal static class RimWorld_FoodUtility_WillEat
	{
		private static void Postfix(this Pawn p, Thing food, ref bool __result)
		{
			if (!AssignAnimalFoodMod.Settings.ManagesGrowingPlants) return;

			if (p != null && __result)
			{
				if (p.RaceProps != null
					   && p.RaceProps.Animal
					   && ((p.Faction != null && p.Faction.IsPlayer) || (p.HostFaction != null && p.HostFaction.IsPlayer)))
				{
					var plant = food as Plant;
					if (plant?.def?.plant != null && plant.def.plant.Harvestable)
					{
						var zone = GridsUtility.GetZone(plant.Position, plant.Map) as Zone_Growing;
						if (zone != null)
						{
							if (zone.allowCut)
                            {
								if (AssignAnimalFoodMod.Settings.preventEatingHarvestablesFromAreasDesignatedToCut)
                                {
									//Logger.Message($"{p.Label} will not eat {plant.def.label} in zone {zone?.label}");
									__result = false;
								}
                            } 
							else
                            {
								if (AssignAnimalFoodMod.Settings.preventEatingHarvestablesFromAreasNotDesignatedToCut)
                                {
									//Logger.Message($"{p.Label} will not eat {plant.def.label} in zone {zone?.label}");
									__result = false;
								}
                            }
						}
					}
				}
			}
		}
	}
}
