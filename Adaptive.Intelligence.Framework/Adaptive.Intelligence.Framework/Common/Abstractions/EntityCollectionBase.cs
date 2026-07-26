namespace Adaptive.Intelligence.Common.Abstractions
{
    /// <summary>
    /// Provides a base implementation for creating collections of data entity instances.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the entity instance.
    /// </typeparam>
    /// <typeparam name="TIdType">
    /// The data type of the ID value of the entity instance.
    /// </typeparam>
    public abstract class EntityCollectionBase<T, TIdType> : AdaptiveCollectionBase<T> 
        where T : EntityBase<TIdType>
        where TIdType : struct
    {
    }
}
