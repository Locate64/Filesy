using System;
using System.Collections.Generic;
using System.IO;
using Filesy.Providers.Managed;

namespace Filesy.Providers
{
    /// <summary>
    /// This class provides a managed implementation of <see cref="IFileSyProvider"/>.
    /// </summary>
    public sealed class ManagedFileSyProvider : IFileSyProvider
    {
        /// <inheritdoc />
        public IEnumerable<FileSyDirectoryInfo> EnumerateDirectories(string path, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(path));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pattern));
            }

            // TODO: pattern should be checked ahead of time to see if it is valid and mimic the built in behavior

            var rootPath = new DirectoryInfo(path);
            var enumerable = new ManagedFileSyDirectoryEnumerable(rootPath, pattern, enumerateOptions);

            return enumerable;
        }

        /// <inheritdoc />
        public IEnumerable<FileSyFileInfo> EnumerateFiles(string path, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(path));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pattern));
            }

            // TODO: pattern should be checked ahead of time to see if it is valid and mimic the built in behavior

            var rootPath = new DirectoryInfo(path);
            var enumerable = new ManagedFileSyFileEnumerable(rootPath, pattern, enumerateOptions);

            return enumerable;
        }

        /// <inheritdoc />
        public IEnumerable<FileSyInfo> EnumerateFileSystemInfos(string path, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(path));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pattern));
            }

            // TODO: pattern should be checked ahead of time to see if it is valid and mimic the built in behavior

            var rootPath = new DirectoryInfo(path);
            var enumerable = new ManagedFileSyFileSystemEnumerable(rootPath, pattern, enumerateOptions);

            return enumerable;
        }
    }
}
