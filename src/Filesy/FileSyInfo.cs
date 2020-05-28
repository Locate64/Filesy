using System;
using System.IO;

namespace Filesy
{
    /// <summary>
    /// Base class for Filesy entries.
    /// </summary>
    public abstract class FileSyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSyInfo"/> class.
        /// </summary>
        /// <param name="fullName">Full path of the entry. Assumed to not be null therefor a null value should never be passed.</param>
        /// <param name="type">Filesy entry type.</param>
        protected FileSyInfo(string fullName, FileSyInfoType type)
        {
            FullName = fullName;
            Type = type;
        }

        /// <summary>
        /// Gets the type of filesy entry.
        /// </summary>
        public FileSyInfoType Type { get; }

        /// <summary>
        /// Gets the full path of the filesy entry.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets the name part of the filesy entry.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the extension of the filesy entry.
        /// </summary>
        public virtual string Extension
        {
            get
            {
                // This default logic is made to mimic the .NET behavior
                var fullName = FullName;

                int length = fullName.Length;

                for (int i = length - 1; i >= 0; i--)
                {
                    char ch = fullName[i];
                    if (ch == '.')
                    {
                        return fullName.Substring(i, length - i);
                    }

                    if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar || ch == Path.VolumeSeparatorChar)
                    {
                        break;
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the filesy entry exists and that the info could be read properly.
        /// </summary>
        /// <remarks>Must return a false if an exception was thrown trying to get this info.</remarks>
        public abstract bool Exists { get; }

        /// <summary>
        /// Gets the attributes for the current filesy entry.
        /// </summary>
        /// <remarks>Must return a 0 flag if an exception was thrown trying to get this info.</remarks>
        public abstract FileAttributes Attributes { get; }

        /// <summary>
        /// Gets the last access time (UTC) for the current filesy entry.
        /// </summary>
        /// <remarks>Must return DateTime.MinValue if an exception was thrown trying to get this info.</remarks>
        public abstract DateTime LastAccessTimeUtc { get; }

        /// <summary>
        /// Gets the last write time (UTC) for the current filesy entry.
        /// </summary>
        /// <remarks>Must return DateTime.MinValue if an exception was thrown trying to get this info.</remarks>
        public abstract DateTime LastWriteTimeUtc { get; }

        /// <summary>
        /// Gets the creation time time (UTC) for the current filesy entry.
        /// </summary>
        /// <remarks>Must return DateTime.MinValue if an exception was thrown trying to get this info.</remarks>
        public abstract DateTime CreationTimeUtc { get; }

        /// <summary>
        /// Gets the parent directory, if any.
        /// </summary>
        public abstract FileSyDirectoryInfo Directory { get; }

        /// <summary>
        /// Attempt to refresh the cached information, if any.
        /// </summary>
        /// <remarks>Must throw an exception on failure.</remarks>
        public abstract void Refresh();
    }
}
