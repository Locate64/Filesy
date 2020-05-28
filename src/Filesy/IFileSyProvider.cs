using System.Collections.Generic;

namespace Filesy
{
    /// <summary>
    /// Provides the interface that each FileSy provider must implement.
    /// </summary>
    public interface IFileSyProvider
    {
        /// <summary>
        /// Enumerate the given directory path (and optionally its subdirectories) for directory entries matching the given pattern.
        /// </summary>
        /// <param name="path">Path to enumerate. Cannot be null or empty. Invalid paths must throw an exception before enumeration.</param>
        /// <param name="pattern">Valid search pattern. Cannot be null or empty. An invalid search pattern should throw an exception..</param>
        /// <param name="enumerateOptions">Enumerate options to configure the enumeration.</param>
        /// <returns>An IEnumerable&lt;FileSyDirectoryInfo&gt; that will enumerate the given path for directories on execution.</returns>
        IEnumerable<FileSyDirectoryInfo> EnumerateDirectories(string path, string pattern, FileSyEnumerateOptions enumerateOptions);

        /// <summary>
        /// Enumerate the given directory path (and optionally its subdirectories) for file entries matching the given pattern.
        /// </summary>
        /// <param name="path">Path to enumerate. Cannot be null or empty. Invalid paths must throw an exception before enumeration.</param>
        /// <param name="pattern">Valid search pattern. Cannot be null or empty. An invalid search pattern should throw an exception..</param>
        /// <param name="enumerateOptions">Enumerate options to configure the enumeration.</param>
        /// <returns>An IEnumerable&lt;FileSyFileInfo&gt; that will enumerate the given path for files on execution.</returns>
        IEnumerable<FileSyFileInfo> EnumerateFiles(string path, string pattern, FileSyEnumerateOptions enumerateOptions);

        /// <summary>
        /// Enumerate the given directory path (and optionally its subdirectories) for file entries matching the given pattern.
        /// </summary>
        /// <param name="path">Path to enumerate. Cannot be null or empty. Invalid paths must throw an exception before enumeration.</param>
        /// <param name="pattern">Valid search pattern. Cannot be null or empty. An invalid search pattern should throw an exception..</param>
        /// <param name="enumerateOptions">Enumerate options to configure the enumeration.</param>
        /// <returns>An IEnumerable&lt;FileSyFileInfo&gt; that will enumerate the given path for files on execution.</returns>
        IEnumerable<FileSyInfo> EnumerateFileSystemInfos(string path, string pattern, FileSyEnumerateOptions enumerateOptions);
    }
}
