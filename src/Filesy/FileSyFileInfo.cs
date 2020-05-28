namespace Filesy
{
    /// <summary>
    /// Base class for Filesy file entries.
    /// </summary>
    public abstract class FileSyFileInfo : FileSyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSyFileInfo"/> class.
        /// </summary>
        /// <param name="fullName">Full path of the file. Assumed to not be null therefor a null value should never be passed.</param>
        protected FileSyFileInfo(string fullName)
            : base(fullName, FileSyInfoType.File)
        {
        }

        /// <summary>
        /// Gets the size, in bytes, of the file.
        /// </summary>
        /// <remarks>Must return -1 when an exception occurred while trying get this info.</remarks>
        public abstract long Length { get; }
    }
}
