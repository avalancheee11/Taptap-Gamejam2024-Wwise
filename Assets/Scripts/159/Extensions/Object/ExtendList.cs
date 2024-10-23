using System;
using System.Collections;
using System.Collections.Generic;

//public class ExtendList<T> : IList<T>
public class ExtendList<T>
{
    static T[] staticArray = new T[]{};

    List<T> _list = new List<T>(4);
    public List<T> list { get {return _list;}}

    T[] _array = staticArray;
    public T[] Array {
        get { 
            if (this.autoUpdate) {
                update(0);
            }
            return _array;
        }
    }
    public T[] ToArray {
        get {
            update(0);
            return _array;
        }
    }

    public bool autoUpdate = true;
    bool _needUpdate;

    ~ExtendList()
    {
        _list = null;
        _array = null;
    }

    public void update(float dt)
    {
        if (!_needUpdate) {
            return;
        }
        _needUpdate = false;
        updateArray();
    }

    void updateArray()
    {
        _array = _list.ToArray();
    }

    //list
    //Properties
    public int Count;
//    public int Count { get {return _list.Count;}}
    public bool IsFixedSize { get { return false;}}
    public bool IsReadOnly { get { IList<T> ilist = _list; return ilist.IsReadOnly;}}
    public bool IsSynchronized { get { return true;}}
    public T this[int index] { get { return _list[index];} set { _list[index] = value;}}
    public List<T> SyncRoot { get { return _list;}}

    //Methods
    public void Add(T item)
    {
        _needUpdate = true;
        _list.Add(item);
        this.Count = _list.Count;
    }

    public void AddRange(IEnumerable<T> collection)
    {
        _needUpdate = true;
        _list.AddRange(collection);
        this.Count = _list.Count;
    }

    public void Clear()
    {
        _needUpdate = true;
        _list.Clear();
        this.Count = _list.Count;
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo (T[] array, int arrayIndex) 
    {
        _list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        return GetEnumerator();
//    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _needUpdate = true;
        _list.Insert(index, item);
        this.Count = _list.Count;
    }

    public bool Remove(T item)
    {
        _needUpdate = true;
        bool ret = _list.Remove(item);
        this.Count = _list.Count;
        return ret;
    }

    public void RemoveAt(int index)
    {
        _needUpdate = true;
        _list.RemoveAt(index);
        this.Count = _list.Count;
    }

    public int RemoveAll(Predicate<T> match)
    {
        _needUpdate = true;
        int ret = _list.RemoveAll(match);
        this.Count = _list.Count;
        return ret;
    }

    public void RemoveRange(int index, int count)
    {
        _needUpdate = true;
        _list.RemoveRange(index, count);
        this.Count = _list.Count;
    }

    public T Find(Predicate<T> match)
    {
        return _list.Find(match);
    }

    public List<T> FindAll(Predicate<T> match)
    {
        return _list.FindAll(match);
    }

    public int FindIndex(Predicate<T> match)
    {
        return _list.FindIndex(match);
    }

    public void Sort(IComparer<T> comparer)
    {
        _list.Sort(comparer);
        _needUpdate = true;
    }

    public void Sort(Comparison<T> comparison)
    {
        _list.Sort(comparison);
        _needUpdate = true;
    }

    public virtual void Recycle()
    {
        _needUpdate = false;
        this.autoUpdate = true;
        _list.Clear();
        _array = staticArray;
        this.Count = 0;
    }
}
