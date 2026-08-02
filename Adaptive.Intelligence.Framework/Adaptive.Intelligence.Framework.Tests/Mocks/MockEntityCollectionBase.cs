using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Framework.Tests.Mocks
{
    /// <summary>
    /// Provides a concrete test wrapper for the <see cref="EntityCollectionBase{T, TIdType}"/> abstract class.
    /// </summary>
    public class MockEntityCollectionBase : EntityCollectionBase<MockEntityBase, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockEntityCollectionBase"/> class.
        /// </summary>
        public MockEntityCollectionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockEntityCollectionBase"/> class.
        /// </summary>
        public MockEntityCollectionBase(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockEntityCollectionBase"/> class.
        /// </summary>
        /// <param name="sourceList">
        /// An <see cref="IEnumerable{MockEntityBase}"/> instance containing the objects used to
        /// populate the collection.
        /// </param>
        public MockEntityCollectionBase(IEnumerable<MockEntityBase> sourceList) : base(sourceList)
        {

        }
    }
}