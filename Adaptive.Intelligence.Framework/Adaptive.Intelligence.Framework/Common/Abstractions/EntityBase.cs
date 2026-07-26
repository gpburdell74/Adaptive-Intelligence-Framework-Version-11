namespace Adaptive.Intelligence.Common.Abstractions
{
    /// <summary>
    /// Provides a base implementation for creating data entity instances.
    /// </summary>
    public abstract record EntityBase<T> : DisposableRecordBase 
        where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase{T}"/> class.
        /// </summary>
        protected EntityBase()
        {
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="EntityBase{T}"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether to release both managed and unmanaged resources (<b>true</b>) or only unmanaged resources (<b>false</b>).
        /// </param>
        protected override void Dispose(bool disposing)
        {
            CreatedDate = null;
            Deleted = false;
            ModifiedDate = null;
            Id = default;
            
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets the date/time the entity or related record was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTimeOffset"/> value, or <b>null</b>.
        /// </value>
        public DateTimeOffset? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is marked as deleted.
        /// </summary>
        /// <value>
        /// <b>true</b> if using soft-deletion and the record is marked as deleted; otherwise, <b>false</b>.
        /// </value>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        /// <value>
        /// A unique ID value of <typeparamref name="T"/>.
        /// </value>
        public T Id { get; set; }

        /// <summary>
        /// Gets or sets the date/time the entity or related record was last modified.
        /// </summary>
        /// <value>
        /// A <see cref="DateTimeOffset"/> value, or <b>null</b>.
        /// </value>
        public DateTimeOffset? ModifiedDate { get; set; }

    }
}
