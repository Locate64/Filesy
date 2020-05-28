using System;
using System.IO;

namespace Filesy
{
    /// <summary>
    /// This struct provides storage for enumeration related options.
    /// </summary>
    public readonly struct FileSyEnumerateOptions : IEquatable<FileSyEnumerateOptions>
    {
        /// <summary>
        /// Access the preconfigured options that will enumerate the top directory only.
        /// </summary>
        public static readonly FileSyEnumerateOptions TopDirectoryOnly = new FileSyEnumerateOptions(SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Access the preconfigured options that will enumerate the top directory and all sub directories, recursively. Reparse points will be enumerated by default.
        /// </summary>
        public static readonly FileSyEnumerateOptions AllDirectories = new FileSyEnumerateOptions(SearchOption.AllDirectories);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSyEnumerateOptions"/> struct.
        /// </summary>
        /// <param name="searchOption">Search option to use. Reparse points will be enumerated where applicable.</param>
        public FileSyEnumerateOptions(SearchOption searchOption)
            : this(searchOption, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSyEnumerateOptions"/> struct.
        /// </summary>
        /// <param name="searchOption">Search option to use.</param>
        /// <param name="enumerateReparse">Value indicating whether reparse points should be enumerated as well where applicable.</param>
        public FileSyEnumerateOptions(SearchOption searchOption, bool enumerateReparse)
        {
            Option = searchOption;
            EnumerateReparse = enumerateReparse;
        }

        /// <summary>
        /// Gets the configured search option.
        /// </summary>
        public SearchOption Option { get; }

        /// <summary>
        /// Gets a value indicating whether reparse points should be enumerated. This only affects sub directory enumeration.
        /// </summary>
        public bool EnumerateReparse { get; }

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Extract the SearchOption out of the options and return it.
        /// </summary>
        /// <param name="enumerateOptions">Source value to extract the SearchOption value from.</param>
        public static implicit operator SearchOption(FileSyEnumerateOptions enumerateOptions) => enumerateOptions.Option;
#pragma warning restore CA2225 // Operator overloads have named alternates

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Convert the SearchOption into a FileSyEnumerateOptions value with reparse point enumeration enabled.
        /// </summary>
        /// <param name="searchOption">Search option to convert.</param>
        public static implicit operator FileSyEnumerateOptions(SearchOption searchOption) => new FileSyEnumerateOptions(searchOption);
#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>
        /// Check values for equality.
        /// </summary>
        /// <param name="left">Left side value.</param>
        /// <param name="right">Right side value.</param>
        /// <returns>Whether or not the given values are equal.</returns>
        public static bool operator ==(FileSyEnumerateOptions left, FileSyEnumerateOptions right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Check values for inequality.
        /// </summary>
        /// <param name="left">Left side value.</param>
        /// <param name="right">Right side value.</param>
        /// <returns>Whether or not the given values are not equal.</returns>
        public static bool operator !=(FileSyEnumerateOptions left, FileSyEnumerateOptions right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Return a new FileSyEnumerateOptions based on the current value with the EnumerateReparse option changed to the given value.
        /// </summary>
        /// <param name="enumerateReparse">New value for the EnumerateReparse option.</param>
        /// <returns>A new FileSyEnumerateOptions value.</returns>
        public FileSyEnumerateOptions EnumeratesReparse(bool enumerateReparse)
        {
            return new FileSyEnumerateOptions(Option, enumerateReparse);
        }

        /// <inheritdoc/>
        public bool Equals(FileSyEnumerateOptions other)
        {
            return Option == other.Option && EnumerateReparse == other.EnumerateReparse;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is FileSyEnumerateOptions other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Option * 397) ^ EnumerateReparse.GetHashCode();
            }
        }
    }
}
