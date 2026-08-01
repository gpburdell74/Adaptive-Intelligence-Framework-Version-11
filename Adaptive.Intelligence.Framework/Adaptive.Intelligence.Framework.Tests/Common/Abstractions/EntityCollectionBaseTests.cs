using Adaptive.Intelligence.Common.Abstractions;
using Adaptive.Intelligence.Framework.Tests.Mocks;
using System.Collections.Specialized;

namespace Adaptive.Intelligence.Framework.Tests.Common.Abstractions;

/// <summary>
/// Provides the tests for the <see cref="EntityCollectionBase{T, TIdType}"/> abstract class.
/// </summary>
public class EntityCollectionBaseTests
{
    /// <summary>
    /// Tests that a new instance starts empty.
    /// </summary>
    [Fact]
    public void Initial_State_Is_Empty()
    {
        MockEntityCollectionBase collection = new();

        Assert.NotNull(collection);
        Assert.Empty(collection);
    }

    /// <summary>
    /// Tests that the source-list constructor uses the specified capacity.
    /// </summary>
    [Fact]
    public void Constructor_With_Capacity_Works()
    {
        MockEntityCollectionBase collection = new(10);

        Assert.NotNull(collection);
        Assert.Empty(collection);
        Assert.Equal(10, collection.Capacity);
    }
    /// <summary>
    /// Tests that the source-list constructor copies entities into the collection.
    /// </summary>
    [Fact]
    public void Constructor_With_SourceList_Populates_Collection()
    {
        MockEntityBase first = new() { Id = 1 };
        MockEntityBase second = new() { Id = 2 };

        MockEntityCollectionBase collection = new(new[] { first, second });

        Assert.Equal(2, collection.Count);
        Assert.Same(first, collection[0]);
        Assert.Same(second, collection[1]);
    }

    /// <summary>
    /// Tests that adding an entity stores the same instance and updates count.
    /// </summary>
    [Fact]
    public void Add_Stores_Entity_Instance()
    {
        MockEntityCollectionBase collection = new();
        MockEntityBase entity = new() { Id = 17, Deleted = false };

        collection.Add(entity);

        Assert.Single(collection);
        Assert.Same(entity, collection[0]);
        Assert.Equal(17, collection[0].Id);
    }

    /// <summary>
    /// Tests that removing by index removes the expected entity.
    /// </summary>
    [Fact]
    public void RemoveAt_Removes_Expected_Entity()
    {
        MockEntityBase first = new() { Id = 1 };
        MockEntityBase second = new() { Id = 2 };
        MockEntityBase third = new() { Id = 3 };
        MockEntityCollectionBase collection = new();
        collection.AddRange([first, second, third]);

        collection.RemoveAt(1);

        Assert.Equal(2, collection.Count);
        Assert.Equal(1, collection[0].Id);
        Assert.Equal(3, collection[1].Id);
    }

    /// <summary>
    /// Tests that <c>Clear</c> removes all entities and raises a reset notification.
    /// </summary>
    [Fact]
    public void Clear_Removes_All_Entities_And_Raises_Reset_Action()
    {
        MockEntityCollectionBase collection = new();
        collection.AddRange([
            new MockEntityBase { Id = 1 },
            new MockEntityBase { Id = 2 }
        ]);

        List<NotifyCollectionChangedAction> actions = [];
        collection.CollectionChanged += (_, args) => actions.Add(args.Action);

        collection.Clear();

        Assert.Empty(collection);
        Assert.Equal([NotifyCollectionChangedAction.Reset], actions);
    }

    /// <summary>
    /// Tests that sorting entities by ID reorders the collection.
    /// </summary>
    [Fact]
    public void Sort_With_Comparison_Reorders_By_Id()
    {
        MockEntityCollectionBase collection = new();
        collection.AddRange([
            new MockEntityBase { Id = 3 },
            new MockEntityBase { Id = 1 },
            new MockEntityBase { Id = 2 }
        ]);

        collection.Sort((left, right) => left.Id.CompareTo(right.Id));

        Assert.Equal(1, collection[0].Id);
        Assert.Equal(2, collection[1].Id);
        Assert.Equal(3, collection[2].Id);
    }

    /// <summary>
    /// Tests that reverse changes the order of entities in the collection.
    /// </summary>
    [Fact]
    public void Reverse_Reorders_Entities()
    {
        MockEntityCollectionBase collection = new();
        collection.AddRange([
            new MockEntityBase { Id = 10 },
            new MockEntityBase { Id = 20 },
            new MockEntityBase { Id = 30 }
        ]);

        collection.Reverse();

        Assert.Equal(30, collection[0].Id);
        Assert.Equal(20, collection[1].Id);
        Assert.Equal(10, collection[2].Id);
    }

    /// <summary>
    /// Tests that entities remain accessible and mutable after insertion.
    /// </summary>
    [Fact]
    public void Inserted_Entities_Remain_Mutable()
    {
        MockEntityCollectionBase collection = new();
        MockEntityBase entity = new() { Id = 5, Deleted = false };
        collection.Add(entity);

        collection[0].Deleted = true;
        collection[0].ModifiedDate = DateTimeOffset.UtcNow;

        Assert.True(entity.Deleted);
        Assert.Equal(collection[0].ModifiedDate, entity.ModifiedDate);
    }
}