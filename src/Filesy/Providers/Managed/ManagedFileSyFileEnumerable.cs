using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Filesy.Internals;

namespace Filesy.Providers.Managed
{
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification = "Enumerable suffix is preferred.")]
    internal class ManagedFileSyFileEnumerable : IEnumerable<FileSyFileInfo>
    {
        private readonly DirectoryInfo rootPath;
        private readonly string pattern;
        private readonly FileSyEnumerateOptions enumerateOptions;
        private readonly Optional<ManagedFileSyDirectoryInfo> rootDirectory;
        private Optional<ManagedFileSyDirectoryInfo> parentDirectory;

        public ManagedFileSyFileEnumerable(DirectoryInfo rootPath, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            this.rootPath = rootPath;
            this.rootDirectory = Optional.Wrap(Try.Execute(() => new ManagedFileSyDirectoryInfo(rootPath), null));
            this.pattern = pattern;
            this.enumerateOptions = enumerateOptions;
        }

        internal ManagedFileSyFileEnumerable(DirectoryInfo rootPath, string pattern, FileSyEnumerateOptions enumerateOptions, Optional<ManagedFileSyDirectoryInfo> parentDirectory)
        {
            this.rootPath = rootPath;
            this.rootDirectory = Optional.Wrap(Try.Execute(() => new ManagedFileSyDirectoryInfo(rootPath), null));
            this.pattern = pattern;
            this.enumerateOptions = enumerateOptions;
            this.parentDirectory = parentDirectory;
        }

        public IEnumerator<FileSyFileInfo> GetEnumerator()
        {
            if (rootPath == null || !rootPath.Exists || rootDirectory.Value == null)
            {
                yield break;
            }

            IEnumerable<FileInfo> fileMatches = SafeEnumerable.Wrap(() => rootPath.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly));

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

            IEnumerable<DirectoryInfo> subfolders =
                SafeEnumerable.Wrap(() => rootPath.EnumerateDirectories("*", SearchOption.TopDirectoryOnly));

            foreach (var subfolder in subfolders)
            {
                if (!enumerateOptions.EnumerateReparse && (subfolder.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                {
                    continue;
                }

                var fileSystemInfos = new ManagedFileSyFileEnumerable(subfolder, pattern, enumerateOptions, rootDirectory);

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
