using Adaptive.Intelligence.Common.Abstractions;
using System.Collections.Specialized;

namespace Adaptive.Intelligence.Framework.Tests.Common.Abstractions;

/// <summary>
/// Provides the tests for the <see cref="AdaptiveCollectionBase{T}"/> class.
/// </summary>
public class AdaptiveCollectionBaseTests
{
    /// <summary>
    /// Tests that the default constructor creates an empty collection instance.
    /// </summary>
    [Fact]
    public void Constructor_Default_Creates_Empty_Collection()
    {
        TestAdaptiveCollection<int> collection = new();

        Assert.NotNull(collection);
        Assert.Empty(collection);
    }

    /// <summary>
    /// Tests that the capacity constructor creates an empty collection with the requested capacity.
    /// </summary>
    [Fact]
    public void Constructor_With_Capacity_Sets_Capacity_And_Is_Empty()
    {
        TestAdaptiveCollection<int> collection = new(12);

        Assert.NotNull(collection);
        Assert.Empty(collection);
        Assert.True(collection.Capacity >= 12);
    }

    /// <summary>
    /// Tests that the source-list constructor copies items in their original order.
    /// </summary>
    [Fact]
    public void Constructor_With_SourceList_Populates_Items_In_Order()
    {
        int[] source = [2, 4, 6, 8];
        TestAdaptiveCollection<int> collection = new(source);

        Assert.Equal(4, collection.Count);
        Assert.Equal([2, 4, 6, 8], collection);
    }

    /// <summary>
    /// Tests that a null source list produces an empty collection.
    /// </summary>
    [Fact]
    public void Constructor_With_Null_SourceList_Creates_Empty_Collection()
    {
        TestAdaptiveCollection<string> collection = new((IEnumerable<string>?)null);

        Assert.NotNull(collection);
        Assert.Empty(collection);
    }

    /// <summary>
    /// Tests that <c>Add</c> inserts the provided item.
    /// </summary>
    [Fact]
    public void Add_Adds_Item()
    {
        TestAdaptiveCollection<int> collection = new();

        collection.Add(10);

        Assert.Single(collection);
        Assert.Equal(10, collection[0]);
    }

    /// <summary>
    /// Tests that <c>AddRange</c> appends all provided items.
    /// </summary>
    [Fact]
    public void AddRange_Adds_Items()
    {
        TestAdaptiveCollection<int> collection = new();

        collection.AddRange([1, 2, 3]);

        Assert.Equal([1, 2, 3], collection);
    }

    /// <summary>
    /// Tests that <c>Clear</c> removes all items and raises a reset change action.
    /// </summary>
    [Fact]
    public void Clear_Removes_All_Items_And_Raises_Reset_Action()
    {
        TestAdaptiveCollection<int> collection = new([5, 6, 7]);
        List<NotifyCollectionChangedAction> actions = CaptureActions(collection);

        collection.Clear();

        Assert.Empty(collection);
        Assert.Equal([NotifyCollectionChangedAction.Reset], actions);
    }

    /// <summary>
    /// Tests that <c>RemoveAt</c> removes the element at a valid index.
    /// </summary>
    [Fact]
    public void RemoveAt_Valid_Index_Removes_Item()
    {
        TestAdaptiveCollection<int> collection = new([10, 20, 30]);

        collection.RemoveAt(1);

        Assert.Equal([10, 30], collection);
    }

    /// <summary>
    /// Tests that <c>RemoveAt</c> ignores invalid indexes and does not raise change notifications.
    /// </summary>
    [Fact]
    public void RemoveAt_Invalid_Index_Does_Not_Modify_Collection_Or_Raise_Event()
    {
        TestAdaptiveCollection<int> collection = new([10, 20, 30]);
        List<NotifyCollectionChangedAction> actions = CaptureActions(collection);

        collection.RemoveAt(-1);
        collection.RemoveAt(3);

        Assert.Equal([10, 20, 30], collection);
        Assert.Empty(actions);
    }

    /// <summary>
    /// Tests that <c>Remove</c> removes the first matching instance.
    /// </summary>
    [Fact]
    public void Remove_Existing_Instance_Removes_First_Occurrence()
    {
        TestAdaptiveCollection<string> collection = new(["a", "b", "a"]);

        collection.Remove("a");

        Assert.Equal(["b", "a"], collection);
    }

    /// <summary>
    /// Tests that <c>Remove</c> ignores null or missing instances and does not raise change notifications.
    /// </summary>
    [Fact]
    public void Remove_Null_Or_Missing_Instance_Does_Not_Modify_Collection_Or_Raise_Event()
    {
        TestAdaptiveCollection<string> collection = new(["x", "y"]);
        List<NotifyCollectionChangedAction> actions = CaptureActions(collection);

        collection.Remove(null!);
        collection.Remove("z");

        Assert.Equal(["x", "y"], collection);
        Assert.Empty(actions);
    }

    /// <summary>
    /// Tests that <c>RemoveAll</c> removes all items matching the predicate.
    /// </summary>
    [Fact]
    public void RemoveAll_Removes_Matching_Items()
    {
        TestAdaptiveCollection<int> collection = new([1, 2, 3, 4, 5, 6]);

        collection.RemoveAll(x => x % 2 == 0);

        Assert.Equal([1, 3, 5], collection);
    }

    /// <summary>
    /// Tests that <c>RemoveRange</c> removes the requested segment of items.
    /// </summary>
    [Fact]
    public void RemoveRange_Removes_Items()
    {
        TestAdaptiveCollection<int> collection = new([1, 2, 3, 4, 5]);

        collection.RemoveRange(1, 3);

        Assert.Equal([1, 5], collection);
    }

    /// <summary>
    /// Tests that <c>Reverse</c> reorders items in reverse order.
    /// </summary>
    [Fact]
    public void Reverse_Reorders_Items()
    {
        TestAdaptiveCollection<int> collection = new([1, 2, 3, 4]);

        collection.Reverse();

        Assert.Equal([4, 3, 2, 1], collection);
    }

    /// <summary>
    /// Tests that the default <c>Sort</c> overload orders items ascending.
    /// </summary>
    [Fact]
    public void Sort_Default_Reorders_Items()
    {
        TestAdaptiveCollection<int> collection = new([4, 1, 3, 2]);

        collection.Sort();

        Assert.Equal([1, 2, 3, 4], collection);
    }

    /// <summary>
    /// Tests that the comparison-based <c>Sort</c> overload honors the supplied comparer logic.
    /// </summary>
    [Fact]
    public void Sort_With_Comparison_Reorders_Items()
    {
        TestAdaptiveCollection<int> collection = new([1, 4, 2, 3]);

        collection.Sort((left, right) => right.CompareTo(left));

        Assert.Equal([4, 3, 2, 1], collection);
    }

    /// <summary>
    /// Tests that range sorting affects only the specified range.
    /// </summary>
    [Fact]
    public void Sort_Range_With_Comparer_Sorts_Only_Range()
    {
        TestAdaptiveCollection<int> collection = new([10, 3, 1, 2, 20]);

        collection.Sort(1, 3, Comparer<int>.Default);

        Assert.Equal([10, 1, 2, 3, 20], collection);
    }

    /// <summary>
    /// Tests that adding with a collection-changed subscriber throws the current argument exception behavior after mutation.
    /// </summary>
    [Fact]
    public void Add_With_CollectionChanged_Handler_Throws_ArgumentException_After_Adding_Item()
    {
        TestAdaptiveCollection<int> collection = new();
        collection.CollectionChanged += (_, _) =>
        {
        };

        ArgumentException ex = Assert.Throws<ArgumentException>(() => collection.Add(10));

        Assert.Equal("action", ex.ParamName);
        Assert.Single(collection);
        Assert.Equal(10, collection[0]);
    }

    /// <summary>
    /// Tests that removing all with a collection-changed subscriber throws the current argument exception behavior after mutation.
    /// </summary>
    [Fact]
    public void RemoveAll_With_CollectionChanged_Handler_Throws_ArgumentException_After_Removing_Items()
    {
        TestAdaptiveCollection<int> collection = new([1, 2, 3, 4, 5, 6]);
        collection.CollectionChanged += (_, _) =>
        {
        };

        ArgumentException ex = Assert.Throws<ArgumentException>(() => collection.RemoveAll(x => x % 2 == 0));

        Assert.Equal("action", ex.ParamName);
        Assert.Equal([1, 3, 5], collection);
    }

    /// <summary>
    /// Tests that reversing with a collection-changed subscriber throws the current argument exception behavior after mutation.
    /// </summary>
    [Fact]
    public void Reverse_With_CollectionChanged_Handler_Throws_ArgumentException_After_Reordering_Items()
    {
        TestAdaptiveCollection<int> collection = new([1, 2, 3, 4]);
        collection.CollectionChanged += (_, _) =>
        {
        };

        ArgumentException ex = Assert.Throws<ArgumentException>(() => collection.Reverse());

        Assert.Equal("action", ex.ParamName);
        Assert.Equal([4, 3, 2, 1], collection);
    }

    private static List<NotifyCollectionChangedAction> CaptureActions<T>(AdaptiveCollectionBase<T> collection)
    {
        List<NotifyCollectionChangedAction> actions = [];
        collection.CollectionChanged += (_, args) => actions.Add(args.Action);
        return actions;
    }

    private sealed class TestAdaptiveCollection<T> : AdaptiveCollectionBase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAdaptiveCollection{T}"/> class.
        /// </summary>
        public TestAdaptiveCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAdaptiveCollection{T}"/> class with an initial capacity.
        /// </summary>
        /// <param name="capacity">
        /// The initial number of items to allocate space for.
        /// </param>
        public TestAdaptiveCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAdaptiveCollection{T}"/> class from a source sequence.
        /// </summary>
        /// <param name="sourceList">
        /// The source sequence used to populate the collection.
        /// </param>
        public TestAdaptiveCollection(IEnumerable<T>? sourceList)
            : base(sourceList)
        {
        }
    }
}