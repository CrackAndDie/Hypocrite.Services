namespace Hypocrite.Core.Container.Common
{
    public struct LightEntry<TValue>
    {
        public int HashCode;
        public string Name;
        public TValue Value;
        public int Next;

        public override string ToString()
        {
            return $"Name: {Name}, Val: {Value.ToString()}, Next: {Next}";
        }
    }
}
