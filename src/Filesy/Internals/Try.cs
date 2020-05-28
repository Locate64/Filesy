using System;

namespace Filesy.Internals
{
    internal static class Try
    {
        internal static TResult Execute<TResult>(Func<TResult> action, TResult fallbackValue)
        {
            try
            {
                return action();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return fallbackValue;
            }
        }
    }
}
