using System.Collections;
using System.Collections.Generic;
// using System.Collections.Concurrent; <<-- ConcurrentDictionary ??

using Zenject;


namespace common
{
    public class Directory<T> : IInitializable
    {
        private Dictionary<string, T> _items;

        public void Initialize()
        {
            _items = new Dictionary<string, T>();
        }

        public void AddItem(string key, T item)
        {
            _items.Add(key, item);
        }

        public void RemoveItem(string key)
        {
            _items.Remove(key);
        }

        public T GetItem(string key)
        {
            if(_items.TryGetValue(key, out T val))
                return val;
            else
                return default(T);
        }

        public int GetCount()
        {
            return _items.Count;
        }
    }
}