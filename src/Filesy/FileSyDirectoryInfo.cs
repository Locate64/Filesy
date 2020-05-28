namespace Filesy
{
    /// <summary>
    /// Base class for Filesy directory entries.
    /// </summary>
    public abstract class FileSyDirectoryInfo : FileSyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSyDirectoryInfo"/> class.
        /// </summary>
        /// <param name="fullName">Full path of the directory. Assumed to not be null therefor a null value should never be passed.</param>
        protected FileSyDirectoryInfo(string fullName)
            : base(fullName, FileSyInfoType.Directory)
        {
        }
    }
}
