using System.Collections.ObjectModel;

namespace Compass.Maui.Extensions;

public static class CollectionExtens
{
    /// <summary>
    /// Collection批量添加
    /// </summary>
    public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
    {
        if (collection == null)
        {
            throw new ArgumentNullException("collection");
        }

        if (items == null)
        {
            throw new ArgumentNullException("items");
        }

        foreach (T item in items)
        {
            collection.Add(item);
        }

        return collection;
    }
}