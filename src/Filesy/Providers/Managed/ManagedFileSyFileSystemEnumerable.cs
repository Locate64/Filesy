using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Filesy.Internals;

namespace Filesy.Providers.Managed
{
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification = "Enumerable suffix is preferred.")]
    internal class ManagedFileSyFileSystemEnumerable : IEnumerable<FileSyInfo>
    {
        private readonly DirectoryInfo rootPath;
        private readonly string pattern;
        private readonly FileSyEnumerateOptions enumerateOptions;
        private readonly Optional<ManagedFileSyDirectoryInfo> rootDirectory;
        private Optional<ManagedFileSyDirectoryInfo> parentDirectory;

        public ManagedFileSyFileSystemEnumerable(DirectoryInfo rootPath, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            this.rootPath = rootPath;
            this.rootDirectory = Optional.Wrap(Try.Execute(() => new ManagedFileSyDirectoryInfo(rootPath), null));
            this.pattern = pattern;
            this.enumerateOptions = enumerateOptions;
        }

        internal ManagedFileSyFileSystemEnumerable(DirectoryInfo rootPath, string pattern, FileSyEnumerateOptions enumerateOptions, Optional<ManagedFileSyDirectoryInfo> parentDirectory)
        {
            this.rootPath = rootPath;
            this.rootDirectory = Optional.Wrap(Try.Execute(() => new ManagedFileSyDirectoryInfo(rootPath), null));
            this.pattern = pattern;
            this.enumerateOptions = enumerateOptions;
            this.parentDirectory = parentDirectory;
        }

        public IEnumerator<FileSyInfo> GetEnumerator()
        {
            if (rootPath == null || !rootPath.Exists || rootDirectory.Value == null)
            {
                yield break;
            }

            var fileMatches = SafeEnumerable.Wrap(() => rootPath.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly));

            foreach (var file in fileMatches)
            {
                if (!parentDirectory.HasValue)
                {
                    parentDirectory = Optional.Wrap(Try.Execute(
                        () => rootPath.Parent == null ? null : new ManagedFileSyDirectoryInfo(rootPath.Parent), null));
                }

                yield return new ManagedFileSyFileInfo(file, parentDirectory);
            }

            if (enumerateOptions.Option == SearchOption.TopDirectoryOnly)
            {
                yield break;
            }

            var subfolders = SafeEnumerable.Wrap(() => rootPath.EnumerateDirectories(pattern, SearchOption.TopDirectoryOnly));

            foreach (var subfolder in subfolders)
            {
                var subFolderInfo = new ManagedFileSyDirectoryInfo(subfolder, rootDirectory);

                yield return subFolderInfo;

                if (!enumerateOptions.EnumerateReparse && (subfolder.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                {
                    continue;
                }

                var fileSystemInfos = new ManagedFileSyFileSystemEnumerable(subfolder, pattern, enumerateOptions, rootDirectory);

                foreach (var entry in fileSystemInfos)
                {
                    yield return entry;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
