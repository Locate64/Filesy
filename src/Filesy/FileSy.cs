using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Filesy.Providers;

namespace Filesy
{
    /// <summary>
    /// This class can be used to create a wrapper around a given <see cref="IFileSyProvider"/>.
    ///
    /// Also provides a static field Managed that provides access to global instance of a FileSy instance backed by the <see cref="ManagedFileSyProvider"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Namespace casing is not the same to prevent basic collisions.")]
    public class FileSy
    {
        /// <summary>
        /// Access a static instance of the default FileSy instance backed by the Managed FileSy provider.
        /// </summary>
        public static readonly FileSy Managed = new FileSy(new ManagedFileSyProvider());

        private readonly IFileSyProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSy"/> class.
        /// </summary>
        /// <param name="provider">Instance of a <see cref="IFileSyProvider"/> to be used as the provider. Cannot be null.</param>
        public FileSy(IFileSyProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Enumerate the given directory path (and optionally its subdirectories) for directory entries matching the given pattern.
        /// </summary>
        /// <param name="path">Path to enumerate. Cannot be null or empty. Invalid paths must throw an exception before enumeration.</param>
        /// <param name="pattern">Valid search pattern. Cannot be null or empty. An invalid search pattern should throw an exception..</param>
        /// <param name="enumerateOptions">Enumerate options to configure the enumeration.</param>
        /// <returns>An IEnumerable&lt;FileSyDirectoryInfo&gt; that will enumerate the given path for directories on execution.</returns>
        public IEnumerable<FileSyDirectoryInfo> EnumerateDirectories(string path, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            return provider.EnumerateDirectories(path, pattern, enumerateOptions);
        }

        /// <summary>
        /// Enumerate the given directory path (and optionally its subdirectories) for file entries matching the given pattern.
        /// </summary>
        /// <param name="path">Path to enumerate. Cannot be null or empty. Invalid paths must throw an exception before enumeration.</param>
        /// <param name="pattern">Valid search pattern. Cannot be null or empty. An invalid search pattern should throw an exception..</param>
        /// <param name="enumerateOptions">Enumerate options to configure the enumeration.</param>
        /// <returns>An IEnumerable&lt;FileSyFileInfo&gt; that will enumerate the given path for files on execution.</returns>
        public IEnumerable<FileSyFileInfo> EnumerateFiles(string path, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            return provider.EnumerateFiles(path, pattern, enumerateOptions);
        }

        /// <summary>
        /// Enumerate the given directory path (and optionally its subdirectories) for file entries matching the given pattern.
        /// </summary>
        /// <param name="path">Path to enumerate. Cannot be null or empty. Invalid paths must throw an exception before enumeration.</param>
        /// <param name="pattern">Valid search pattern. Cannot be null or empty. An invalid search pattern should throw an exception..</param>
        /// <param name="enumerateOptions">Enumerate options to configure the enumeration.</param>
        /// <returns>An IEnumerable&lt;FileSyFileInfo&gt; that will enumerate the given path for files on execution.</returns>
        public IEnumerable<FileSyInfo> EnumerateFileSystemInfos(string path, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            return provider.EnumerateFileSystemInfos(path, pattern, enumerateOptions);
        }
    }
}
