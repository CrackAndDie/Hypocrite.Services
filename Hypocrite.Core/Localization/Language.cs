namespace Hypocrite.Core.Localization
{
    public struct Language
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Language(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
