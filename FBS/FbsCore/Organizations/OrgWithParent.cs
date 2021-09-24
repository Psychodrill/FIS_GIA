namespace Fbs.Core.Organizations
{
    /// <summary>
    /// The org with parent.
    /// </summary>
    public class OrgWithParent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsPrivate.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets MainId.
        /// </summary>
        public int? MainId { get; set; }

        /// <summary>
        /// Gets or sets RegionId.
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Gets or sets ShortName.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets TypeId.
        /// </summary>
        public int TypeId { get; set; }

        #endregion
    }
}