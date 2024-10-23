using System;
using System.Collections;
using System.Collections.Generic;

public class ExtendDictionary<TKey, TValue>
{
    static TKey[] staticKeys = new TKey[]{ };
    static TValue[] staticValues = new TValue[]{ };

    Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
    public Dictionary<TKey, TValue> dictionary { get {return _dictionary;}}

    TValue[] _values = staticValues;
    public TValue[] ToValues {
        get {
            update(0);
            return _values;
        }
    }

    bool _needUpdate;

    ~ExtendDictionary()
    {
        _dictionary = null;
        _values = null;
    }

    public void update(float dt)
    {
        if (!_needUpdate) {
            return;
        }
        _needUpdate = false;
        updateDictionary();
    }

    void updateDictionary()
    {
        _values = _dictionary.getValues();
    }

    public int Count;
    public bool IsFixedSize { get { return false;}}
    //    public bool IsReadOnly { get { IList<T> ilist = _list; return ilist.IsReadOnly;}}
    public bool IsSynchronized { get { return true;}}
    public TValue this[TKey key] { get { return _dictionary[key];} set { _dictionary[key] = value;}}

    //Methods
    public void Add(TKey tKey, TValue tValue)
    {
        _needUpdate = true;
        _dictionary.Add(tKey, tValue);
        this.Count = _dictionary.Count;
    }

    public void Clear()
    {
        _needUpdate = true;
        _dictionary.Clear();
        this.Count = _dictionary.Count;
    }

    public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public TValue objectValue(TKey key, TValue defaultValue = default(TValue))
    {
        if (_dictionary == default(Dictionary<TKey,TValue>)) return defaultValue;
        TValue val;
        if (!_dictionary.TryGetValue(key, out val)) {
            val = defaultValue;
        }
        return val;
    }

    public List<TValue> getValuesList()
    {
        return new List<TValue>(_dictionary.Values);
    }
}