using System.Collections;
using System.Collections.Generic;

namespace Filesy.Internals
{
    internal class SafeEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> source;

        internal SafeEnumerator(IEnumerator<T> source)
        {
            this.source = source;
        }

        public T Current => source.Current;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            try
            {
                return source.MoveNext();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return false;
            }
        }

        public void Reset()
        {
            source.Reset();
        }

        public void Dispose()
        {
            source.Dispose();
        }
    }
}
