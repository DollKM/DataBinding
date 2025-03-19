using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataSet<TData> : Data<HashSet<TData>>
{
    public delegate void DataSetOnChanged(HashSet<TData> dict);

    [NonSerialized]
    new public DataSetOnChanged willChanged;
    public DataSet()
    {
        _value = new HashSet<TData>();
    }

    public DataSet(HashSet<TData> value)
    {
        _value = value;
    }

    public void Add(TData data)
    {
        if (!_value.Contains(data))
        {
            willChanged?.Invoke(_value);
            _value.Add(data);
            DataBind.I.Emit(this, DataBindAction.Add);
        }
    }

    public override void Read(JsonReader reader, JsonSerializer serializer)
    {
        JArray token = JArray.Load(reader);
        _value = token.ToObject<HashSet<TData>>(serializer);
    }

    public void Remove(TData data)
    {
        if (_value.Contains(data))
        {
            willChanged?.Invoke(_value);
            _value.Remove(data);
            DataBind.I.Emit(this, DataBindAction.Remove);
        }
    }

    public void Clear()
    {
        willChanged?.Invoke(_value);
        _value.Clear();
        DataBind.I.Emit(this, DataBindAction.Clear);
    }

    public bool Contains(TData data)
    {
        return _value.Contains(data);
    }
    public int Count
    {
        get
        {
            return _value.Count;
        }
    }
}
