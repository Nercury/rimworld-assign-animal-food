using Verse;

namespace AssignAnimalFood
{
    public class AssignAnimalFoodComponent : GameComponent
    {
        public int RestrictionId = 0;

        public AssignAnimalFoodComponent(Game game) : base()
        {
        }

        public bool StartingUp => RestrictionId == 0;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref RestrictionId, "AnimalFood_RestrictionId", -1);
        }

        public override void LoadedGame()
        {
            Current.Game.foodRestrictionDatabase.AddOrGetAnimalFoodRestriction();
        }

        public override void StartedNewGame()
        {
            Current.Game.foodRestrictionDatabase.AddOrGetAnimalFoodRestriction();
        }
    }
}
