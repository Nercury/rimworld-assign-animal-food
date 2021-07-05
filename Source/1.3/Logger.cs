namespace AssignAnimalFood
{
    public static class Logger
    {
        public static void Message(string message)
        {
            Verse.Log.Message($"[{AnimalMod.Name} v{AnimalMod.Version}] {message}", false);
        }
    }
}
