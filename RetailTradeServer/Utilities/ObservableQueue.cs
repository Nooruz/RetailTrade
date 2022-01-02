﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SalePageServer.Utilities
{
    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }
        public new T Dequeue()
        {
            T item = base.Dequeue();

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0));

            return item;
        }
        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public new bool TryDequeue(out T item)
        {
            bool success = base.TryDequeue(out item);

            if (success)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0));
            }

            return success;
        }
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection.Any())
            {
                foreach (var item in collection)
                {
                    Enqueue(item);
                }
            }
        }
    }
}