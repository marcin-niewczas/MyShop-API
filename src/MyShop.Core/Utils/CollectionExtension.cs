using MyShop.Core.Models.BaseEntities;
using System.Diagnostics.CodeAnalysis;

namespace MyShop.Core.Utils;
public static class CollectionExtension
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? collection)
        => collection is null || !collection.Any();

    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this T[]? collection)
        => collection is null or { Length: <= 0 };

    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IReadOnlyCollection<T>? collection)
        => collection is null or { Count: <= 0 };

    public static TObject? SingleWhenOnly<TObject>(this IEnumerable<TObject> collection, Func<TObject, bool> predicate) where TObject : class
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        var results = collection
                            .Where(predicate)
                            .Take(2)
                            .ToArray();

        return results.Length is 1 ? results[0] : null;
    }

    public static bool HasExactlyOne<TObject>(this IReadOnlyCollection<TObject> collection, Func<TObject, bool> predicate) where TObject : class
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        if (collection.Count <= 0)
        {
            return false;
        }

        var results = collection
                            .Where(predicate)
                            .Take(2)
                            .ToArray();

        return results.Length is 1;
    }

    public static bool HasAtLeastOne<TObject>(this IReadOnlyCollection<TObject> collection, Func<TObject, bool> predicate) where TObject : class
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        if (collection.Count <= 0)
        {
            return false;
        }

        var results = collection
                            .Where(predicate)
                            .Take(2)
                            .ToArray();

        return results.Length >= 1;
    }

    public static bool HasEntityDuplicates<TEntity>(this IReadOnlyCollection<TEntity> entities) where TEntity : class, IEntity
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Count <= 1)
        {
            return false;
        }

        return entities.DistinctBy(e => e.Id).Count() != entities.Count;
    }

    public static bool HasDuplicate(this IReadOnlyCollection<string> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count <= 1)
        {
            return false;
        }

        collection = collection.Select(i => i.ToLower().ToTrimmedString()).ToArray();

        var distinctCount = collection
                            .Distinct().Count();

        return distinctCount != collection.Count;
    }

    public static bool HasDuplicate<TStruct>(this IReadOnlyCollection<TStruct> collection) where TStruct : struct
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (collection.Count <= 1)
        {
            return false;
        }

        return collection.Distinct().Count() != collection.Count;
    }

    public static bool HasDuplicateBy<TObject, TPropherty>(
        this IEnumerable<TObject> collection,
        Func<TObject, TPropherty> func
        ) where TObject : class
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(func);

        return collection.DistinctBy(func).Count() != collection.Count();
    }

    public static bool HasDuplicateBy<TObject, TPropherty>(
        this IReadOnlyCollection<TObject> collection,
        Func<TObject, TPropherty> func
        ) where TObject : class
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(func);

        return collection.DistinctBy(func).Count() != collection.Count;
    }
}
