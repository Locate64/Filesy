using System;
using System.IO;
using Filesy.Internals;

namespace Filesy.Providers.Managed
{
    /// <summary>
    /// This class provides a managed implementation for retrieving directory information.
    /// </summary>
    public class ManagedFileSyDirectoryInfo : FileSyDirectoryInfo
    {
        private readonly Func<ManagedFileSyDirectoryInfo> directoryFactory;

        private int innerState;
        private DirectoryInfo directoryInfo;
        private Optional<ManagedFileSyDirectoryInfo> directory;

        internal ManagedFileSyDirectoryInfo(DirectoryInfo directoryInfo, Optional<ManagedFileSyDirectoryInfo> parent)
            : base(directoryInfo.FullName)
        {
            Name = directoryInfo.Name;

            this.directory = parent;
            this.directoryInfo = directoryInfo;
            this.innerState = 0;
        }

        internal ManagedFileSyDirectoryInfo(DirectoryInfo directoryInfo)
            : base(directoryInfo.FullName)
        {
            Name = directoryInfo.Name;

            this.directoryFactory = () => new ManagedFileSyDirectoryInfo(directoryInfo.Parent);
            this.directoryInfo = directoryInfo;
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

                        return innerState == 0 && directoryInfo.Exists;
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

                        return innerState != 0 ? DateTime.MinValue : directoryInfo.LastAccessTimeUtc;
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

                        return innerState != 0 ? DateTime.MinValue : directoryInfo.LastWriteTimeUtc;
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

                        return innerState != 0 ? DateTime.MinValue : directoryInfo.CreationTimeUtc;
                    }, DateTime.MinValue);
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

                    return directoryInfo.Attributes;
                }, (FileAttributes)0);
            }
        }

        /// <inheritdoc />
        public sealed override void Refresh()
        {
            try
            {
                innerState = -1;

                if (directoryInfo == null)
                {
                    directoryInfo = new DirectoryInfo(FullName);
                }

                directoryInfo.Refresh();

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
