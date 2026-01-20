using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITEK_test_app
{
    public class ObservableDictionary<Tkey, Tvalue> : IDictionary<Tkey, Tvalue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private Dictionary<Tkey, Tvalue> _dict;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public ICollection<Tkey> Keys
        {
            get
            {
                return _dict.Keys;
            }
        }

        public ICollection<Tvalue> Values
        {
            get
            {
                return _dict.Values;
            }
        }

        public Tvalue this[Tkey key]
        {
            get
            {
                return _dict[key];
            }
            set
            {
                _dict[key] = value;
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, value);
                OnPropertyChanged(nameof(Values));
            }
        }

        public bool IsReadOnly { get; }

        public ObservableDictionary()
        {
            _dict = new Dictionary<Tkey, Tvalue>();
            IsReadOnly = false;
        }

        public void Add(Tkey key, Tvalue value)
        {
            if (!_dict.ContainsKey(key))
            {
                _dict.Add(key, value);
                OnCollectionChanged(NotifyCollectionChangedAction.Add, value);
                OnPropertyChanged(nameof(Count));
            }
        }

        public void Add(KeyValuePair<Tkey, Tvalue> pair)
        {
            if (!_dict.ContainsKey(pair.Key))
            {
                _dict.Add(pair.Key, pair.Value);
                OnCollectionChanged(NotifyCollectionChangedAction.Add, pair.Value);
                OnPropertyChanged(nameof(Count));
            }
        }

        public bool Remove(Tkey key)
        {
            if (_dict.ContainsKey(key))
            {
                Tvalue deletedItem = _dict[key];
                _dict.Remove(key);

                OnCollectionChanged(NotifyCollectionChangedAction.Remove, deletedItem);
                OnPropertyChanged(nameof(Count));
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<Tkey, Tvalue> pair)
        {
            if (_dict.ContainsKey(pair.Key))
            {
                Tvalue deletedItem = _dict[pair.Key];
                _dict.Remove(pair.Key);

                OnCollectionChanged(NotifyCollectionChangedAction.Remove, deletedItem);
                OnPropertyChanged(nameof(Count));
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _dict.Clear();

            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
            OnPropertyChanged(nameof(Count));
        }

        public bool ContainsKey(Tkey key)
        {
            return _dict.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<Tkey, Tvalue> pair)
        {
            return _dict.Contains(pair);
        }

        public bool TryGetValue(Tkey key, out Tvalue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<Tkey, Tvalue>>.CopyTo(KeyValuePair<Tkey, Tvalue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, Tvalue item = default)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerator<KeyValuePair<Tkey, Tvalue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
