namespace StringDuplicate
{
    [System.Serializable]
    public struct ComponentWordItem
    {
        public string Original;
        public string Target;
        public int Distance;

        public ComponentWordItem(string original, string target)
        {
            Original = original;
            Target = target;
            Distance = original.DamerauLevenshteinDistanceTo(target);
        }
    }
}