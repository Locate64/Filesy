using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Filesy.Internals;

namespace Filesy.Providers.Managed
{
    [SuppressMessage("Microsoft.Naming", "CA1710", Justification = "Enumerable suffix is preferred.")]
    internal class ManagedFileSyDirectoryEnumerable : IEnumerable<FileSyDirectoryInfo>
    {
        private readonly DirectoryInfo rootPath;
        private readonly string pattern;
        private readonly FileSyEnumerateOptions enumerateOptions;
        private readonly Optional<ManagedFileSyDirectoryInfo> rootDirectory;
        private Optional<ManagedFileSyDirectoryInfo> parentDirectory;

        public ManagedFileSyDirectoryEnumerable(DirectoryInfo rootPath, string pattern, FileSyEnumerateOptions enumerateOptions)
        {
            this.rootPath = rootPath;
            this.rootDirectory = Optional.Wrap(Try.Execute(() => new ManagedFileSyDirectoryInfo(rootPath), null));
            this.pattern = pattern;
            this.enumerateOptions = enumerateOptions;
        }

        internal ManagedFileSyDirectoryEnumerable(DirectoryInfo rootPath, string pattern, FileSyEnumerateOptions enumerateOptions, Optional<ManagedFileSyDirectoryInfo> parentDirectory)
        {
            this.rootPath = rootPath;
            this.rootDirectory = Optional.Wrap(Try.Execute(() => new ManagedFileSyDirectoryInfo(rootPath), null));
            this.pattern = pattern;
            this.enumerateOptions = enumerateOptions;
            this.parentDirectory = parentDirectory;
        }

        public IEnumerator<FileSyDirectoryInfo> GetEnumerator()
        {
            if (rootPath == null || !rootPath.Exists || rootDirectory.Value == null)
            {
                yield break;
            }

            IEnumerable<DirectoryInfo> matches = SafeEnumerable.Wrap(() =>
                enumerateOptions.Option == SearchOption.AllDirectories && pattern == "*"
                    ? rootPath.EnumerateDirectories(pattern, SearchOption.TopDirectoryOnly).ToArray()
                    : rootPath.EnumerateDirectories(pattern, SearchOption.TopDirectoryOnly));

            foreach (var directory in matches)
            {
                if (!parentDirectory.HasValue)
                {
                    parentDirectory = Optional.Wrap(Try.Execute(
                        () => rootPath.Parent == null ? null : new ManagedFileSyDirectoryInfo(rootPath.Parent), null));
                }

                yield return new ManagedFileSyDirectoryInfo(directory, parentDirectory);
            }

            if (enumerateOptions.Option != SearchOption.AllDirectories)
            {
                yield break;
            }

            var subfolderMatches = pattern == "*"
                ? matches
                : SafeEnumerable.Wrap(() => rootPath.EnumerateDirectories("*", SearchOption.TopDirectoryOnly));

            foreach (var subfolder in subfolderMatches)
            {
                if (!enumerateOptions.EnumerateReparse && (subfolder.Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint)
                {
                    continue;
                }

                var fileSystemInfos = new ManagedFileSyDirectoryEnumerable(subfolder, pattern, enumerateOptions, rootDirectory);

                foreach (var match in fileSystemInfos)
                {
                    yield return match;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
