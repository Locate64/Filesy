namespace Filesy.Internals
{
    internal readonly struct Optional<TValue>
    {
        public readonly bool HasValue;
        public readonly TValue Value;

        public Optional(TValue value)
        {
            HasValue = true;
            Value = value;
        }
    }

    internal readonly struct Optional
    {
        public static Optional<TValue> Wrap<TValue>(TValue value)
        {
            return new Optional<TValue>(value);
        }
    }
}
