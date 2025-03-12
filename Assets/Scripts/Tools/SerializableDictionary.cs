using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> :
ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key]
    {
        get => dictionary[key];
        set => dictionary[key] = value;
    }

    public void Add(TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
            throw new System.ArgumentException("An item with the same key has already been added.");

        dictionary.Add(key, value);
        keys.Add(key);
        values.Add(value);
    }

    public bool Remove(TKey key)
    {
        if (!dictionary.ContainsKey(key))
            return false;

        var index = keys.IndexOf(key);
        keys.RemoveAt(index);
        values.RemoveAt(index);
        return dictionary.Remove(key);
    }

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
    public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
    public int Count => dictionary.Count;
    public void Clear()
    {
        dictionary.Clear();
        keys.Clear();
        values.Clear();
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        for (int i = 0; i < keys.Count; i++)
        {
            if (i < values.Count && !dictionary.ContainsKey(keys[i]))
            {
                dictionary.Add(keys[i], values[i]);
            }
        }
    }
}
