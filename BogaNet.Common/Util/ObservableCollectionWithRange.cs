using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BogaNet.Util;

/// <summary>
/// Specialized ObservableCollection with AddRange.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObservableCollectionWithRange<T> : ObservableCollection<T>
{
    /// <summary>
    /// Adds all elements of the specified collection to the end of the collection.
    /// </summary>
    /// <param name="collection">Collection with the elements</param>
    public void AddRange(IEnumerable<T> collection)
    {
        CheckReentrancy();

        foreach (var item in collection)
        {
            Items.Add(item);
        }

        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}