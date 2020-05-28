using System;
using System.Collections;
using System.Collections.Generic;

namespace Filesy.Internals
{
    internal static class SafeEnumerable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The intention of this method is to catch and ignore any exception thrown.")]
        public static SafeEnumerable<T> Wrap<T>(Func<IEnumerable<T>> sourceProvider)
        {
            try
            {
                return new SafeEnumerable<T>(sourceProvider());
            }
            catch
            {
                return new SafeEnumerable<T>(Array.Empty<T>());
            }
        }

        public static SafeEnumerable<T> Wrap<T>(IEnumerable<T> source)
        {
            return new SafeEnumerable<T>(source ?? Array.Empty<T>());
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    internal class SafeEnumerable<T> : IEnumerable<T>
#pragma warning restore SA1402 // File may only contain a single type
    {
        private readonly IEnumerable<T> source;

        internal SafeEnumerable(IEnumerable<T> source)
        {
            this.source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SafeEnumerator<T>(source.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
