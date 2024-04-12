using RimWorld;
using Verse;

namespace AssignAnimalFood
{
    public static class NewFoodRestrictionExtensions
    {
		public static FoodPolicy GetAnimalFoodRestriction(this FoodRestrictionDatabase database)
		{
			var food = Current.Game.GetComponent<AssignAnimalFoodComponent>();
			if (food == null)
			{
				Logger.Message($"Get: Game component was not loaded");
				return null;
			}

			if (food.RestrictionId > -1)
			{
				if (food.StartingUp)
                {
					var res = AddAnimalFoodRestriction(database);
					food.RestrictionId = res.id;
					return res;
				}

				return database.AllFoodRestrictions.FirstOrFallback(r => r.id == food.RestrictionId);
			}

			return null;
		}
		
		public static void RemoveAnimalRestrictionIdIfMissing(this FoodRestrictionDatabase database)
		{
			var food = Current.Game.GetComponent<AssignAnimalFoodComponent>();
			if (food == null)
			{
				Logger.Message($"Get: Game component was not loaded");
				return;
			}

			if (food.RestrictionId == -2) 
				return;
			if (food.RestrictionId == -1) 
				return;

			if (database.AllFoodRestrictions.FirstOrFallback(r => r.id == food.RestrictionId) == null)
			{
				food.RestrictionId = -2;
			}
		}

		public static FoodPolicy AddOrGetAnimalFoodRestriction(this FoodRestrictionDatabase database)
        {
            FoodPolicy res = null;

			var food = Current.Game.GetComponent<AssignAnimalFoodComponent>();
			if (food == null)
            {
				Logger.Message($"Add/Get: Game component was not loaded");
				return res;
			}

			// restriction was deleted by user

			if (food.RestrictionId == -2)
            {
				res = database.AllFoodRestrictions.FirstOrFallback(r => r.label == "Animal");
				if (res == null)
				{
					Logger.Message($"Add/Get: No restriction named \"Animal\", recovery failed. New animal pawns will have undefined default food restriction.");
				}
				else
				{
					Logger.Message($"Add/Get: Recovered \"{res.label}\" restriction as the default");
					food.RestrictionId = res.id;
				}
				return res;
			}

			// restriction was created automatically

			if (food.RestrictionId != -1)
            {
				res = database.AllFoodRestrictions.FirstOrFallback(r => r.id == food.RestrictionId);
			}

			// restriction was never created

			if (res == null)
			{
				Logger.Message($"Add/Get: Restriction is null, creating");

				res = AddAnimalFoodRestriction(database);
				food.RestrictionId = res.id;
			}

			return res;
        }

		public static FoodPolicy AddAnimalFoodRestriction(this FoodRestrictionDatabase database)
		{
            var res = database.MakeNewFoodRestriction();
            res.label = ThisMod.FoodRestrictionAnimal;
			Logger.Message($"Add/Get: Creating {ThisMod.FoodRestrictionAnimal} food restriction, id {res.id}");
			res.filter.SetDisallowAll();
			res.filter.SetAllow(ThingCategoryDefOf.Corpses, true);
			res.filter.SetAllow(ThingCategoryDefOf.PlantFoodRaw, true);
			res.filter.SetAllow(ThingCategoryDefOf.MeatRaw, true);

			foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
			{
				if (def.ingestible != null)
				{
					var allowKibblesAndNutrientPastes = def.ingestible.preferability == FoodPreferability.MealAwful || def.ingestible.preferability == FoodPreferability.RawBad;
					var allowPemmican = def == ThingDefOf.Pemmican;
					var allowSimpleMeal = def == ThingDefOf.MealSimple;
					var allowHay = def == ThingDefOf.Hay;

					if (allowKibblesAndNutrientPastes || allowPemmican || allowSimpleMeal || allowHay)
					{
						res.filter.SetAllow(def, true);
					}
				}
			}

			return res;
		}
	}
}
