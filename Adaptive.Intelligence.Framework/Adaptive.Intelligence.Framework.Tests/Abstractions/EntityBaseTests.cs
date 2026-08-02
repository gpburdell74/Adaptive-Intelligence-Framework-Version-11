using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Framework.Tests.Mocks;

namespace Adaptive.Intelligence.Framework.Tests.Abstractions
{
    /// <summary>
    /// Provides the tests for the <see cref="EntityBase{T}"/> abstract record.
    /// </summary>
    public class EntityBaseTests
    {
        /// <summary>
        /// Tests that a new instance initializes with default values.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Initial_State_Uses_Default_Values.
        /// </summary>
        public void Initial_State_Uses_Default_Values()
        {
            MockEntityBase entity = new();

            Assert.Equal(0, entity.Id);
            Assert.False(entity.Deleted);
            Assert.Null(entity.CreatedDate);
            Assert.Null(entity.ModifiedDate);
            Assert.False(entity.MockIsDisposed);
        }

        /// <summary>
        /// Tests that the entity properties can be assigned and read.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Properties_Can_Be_Set_And_Read.
        /// </summary>
        public void Properties_Can_Be_Set_And_Read()
        {
            MockEntityBase entity = new();
            DateTimeOffset created = DateTimeOffset.UtcNow.AddDays(-2);
            DateTimeOffset modified = DateTimeOffset.UtcNow;

            entity.Id = 42;
            entity.Deleted = true;
            entity.CreatedDate = created;
            entity.ModifiedDate = modified;

            Assert.Equal(42, entity.Id);
            Assert.True(entity.Deleted);
            Assert.Equal(created, entity.CreatedDate);
            Assert.Equal(modified, entity.ModifiedDate);
        }

        /// <summary>
        /// Tests that disposing resets all entity values to defaults.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Resets_Entity_Values_To_Defaults.
        /// </summary>
        public void Dispose_Resets_Entity_Values_To_Defaults()
        {
            MockEntityBase entity = new()
            {
                Id = 99,
                Deleted = true,
                CreatedDate = DateTimeOffset.UtcNow.AddDays(-1),
                ModifiedDate = DateTimeOffset.UtcNow
            };

            entity.Dispose();

            Assert.Equal(0, entity.Id);
            Assert.False(entity.Deleted);
            Assert.Null(entity.CreatedDate);
            Assert.Null(entity.ModifiedDate);
            Assert.True(entity.MockIsDisposed);
        }

        /// <summary>
        /// Tests that disposing the entity raises the <see cref="DisposableRecordBase.Disposed"/> event once.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Raises_Disposed_Event_Once.
        /// </summary>
        public void Dispose_Raises_Disposed_Event_Once()
        {
            MockEntityBase entity = new();
            int eventCount = 0;

            entity.Disposed += (_, _) => eventCount++;

            entity.Dispose();
            entity.Dispose();
            entity.Dispose();

            Assert.Equal(1, eventCount);
        }

        /// <summary>
        /// Tests that multiple dispose calls are safe and keep the instance disposed.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Can_Be_Called_Multiple_Times_Safely.
        /// </summary>
        public void Dispose_Can_Be_Called_Multiple_Times_Safely()
        {
            MockEntityBase entity = new();

            entity.Dispose();
            entity.Dispose();
            entity.Dispose();

            Assert.True(entity.MockIsDisposed);
            Assert.Equal(0, entity.Id);
            Assert.False(entity.Deleted);
        }

        /// <summary>
        /// Tests that record equality compares entity value members.
        /// </summary>
        [Fact]
        /// <summary>
        /// Gets the definition for Record_Equality_Compares_Value_Members.
        /// </summary>
        public void Record_Equality_Compares_Value_Members()
        {
            DateTimeOffset created = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset modified = DateTimeOffset.UtcNow;

            MockEntityBase left = new()
            {
                Id = 7,
                Deleted = false,
                CreatedDate = created,
                ModifiedDate = modified
            };

            MockEntityBase right = new()
            {
                Id = 7,
                Deleted = false,
                CreatedDate = created,
                ModifiedDate = modified
            };

            Assert.Equal(left, right);

            right.Deleted = true;
            Assert.NotEqual(left, right);
        }
    }
}