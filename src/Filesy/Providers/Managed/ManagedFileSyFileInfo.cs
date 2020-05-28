using System;
using System.IO;
using Filesy.Internals;

namespace Filesy.Providers.Managed
{
    /// <summary>
    /// This class provides a managed implementation for retrieving file information.
    /// </summary>
    public class ManagedFileSyFileInfo : FileSyFileInfo
    {
        private readonly Func<ManagedFileSyDirectoryInfo> directoryFactory;

        private int innerState;
        private FileInfo fileInfo;
        private Optional<ManagedFileSyDirectoryInfo> directory;

        internal ManagedFileSyFileInfo(FileInfo fileInfo, Optional<ManagedFileSyDirectoryInfo> parent)
            : base(fileInfo.FullName)
        {
            Name = fileInfo.Name;

            this.directory = parent;
            this.fileInfo = fileInfo;
            this.innerState = 0;
        }

        internal ManagedFileSyFileInfo(FileInfo fileInfo)
            : base(fileInfo.FullName)
        {
            Name = fileInfo.Name;

            this.directoryFactory = () => new ManagedFileSyDirectoryInfo(fileInfo.Directory);
            this.fileInfo = fileInfo;
            this.innerState = 0;
        }

        /// <inheritdoc />
        public override FileSyDirectoryInfo Directory
        {
            get
            {
                if (directory.HasValue || directoryFactory == null)
                {
                    return directory.Value;
                }

                directory = Optional.Wrap(Try.Execute(directoryFactory, null));

                return directory.Value;
            }
        }

        /// <inheritdoc />
        public override string Name
        {
            get;
        }

        /// <inheritdoc />
        public override bool Exists
        {
            get
            {
                return Try.Execute(
                    () =>
                    {
                        if (innerState == -1)
                        {
                            Refresh();
                        }

                        return innerState == 0 && fileInfo.Exists;
                    }, false);
            }
        }

        /// <inheritdoc />
        public override DateTime LastAccessTimeUtc
        {
            get
            {
                return Try.Execute(
                    () =>
                    {
                        if (innerState == -1)
                        {
                            Refresh();
                        }

                        return innerState != 0 ? DateTime.MinValue : fileInfo.LastAccessTimeUtc;
                    }, DateTime.MinValue);
            }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTimeUtc
        {
            get
            {
                return Try.Execute(
                    () =>
                    {
                        if (innerState == -1)
                        {
                            Refresh();
                        }

                        return innerState != 0 ? DateTime.MinValue : fileInfo.LastWriteTimeUtc;
                    }, DateTime.MinValue);
            }
        }

        /// <inheritdoc />
        public override DateTime CreationTimeUtc
        {
            get
            {
                return Try.Execute(
                    () =>
                    {
                        if (innerState == -1)
                        {
                            Refresh();
                        }

                        return innerState != 0 ? DateTime.MinValue : fileInfo.CreationTimeUtc;
                    }, DateTime.MinValue);
            }
        }

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                return Try.Execute(
                    () =>
                    {
                        if (innerState == -1)
                        {
                            Refresh();
                        }

                        return innerState != 0 ? -1 : fileInfo.Length;
                    }, -1);
            }
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get
            {
                return Try.Execute(
                    () =>
                {
                    if (innerState == -1)
                    {
                        Refresh();
                    }

                    if (innerState != 0)
                    {
                        return 0;
                    }

                    return fileInfo.Attributes;
                }, (FileAttributes)0);
            }
        }

        /// <inheritdoc />
        public sealed override void Refresh()
        {
            try
            {
                innerState = -1;

                if (fileInfo == null)
                {
                    fileInfo = new FileInfo(FullName);
                }

                fileInfo.Refresh();

                innerState = 0;
            }
            catch
            {
                innerState = 1;

                throw;
            }
        }
    }
}
