using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif
//已将文件转为UTF-8

public class DataDict<TKey, TData> : Data<Dictionary<TKey, TData>>
{
    public delegate void DataDictOnChanged(Dictionary<TKey, TData> dict, TKey key);

    [NonSerialized]
    new public DataDictOnChanged willChanged;
    public DataDict()
    {
        _value = new Dictionary<TKey, TData>();
    }

    public DataDict(Dictionary<TKey, TData> value)
    {
        _value = value;
    }
    public TKey key;
    public void Set(TKey key, TData data)
    {
        this.key = key;
        willChanged?.Invoke(_value, key);
        _value[key] = data;
        DataBind.I.Emit(this, DataBindAction.Add);
    }

    public override void Read(JsonReader reader, JsonSerializer serializer)
    {
        JObject token = JObject.Load(reader);
        _value = token.ToObject<Dictionary<TKey, TData>>(serializer);
    }

    public void Remove(TKey key)
    {
        if (_value.ContainsKey(key))
        {
            this.key = key;
            willChanged?.Invoke(_value, key);
            _value.Remove(key);
            DataBind.I.Emit(this, DataBindAction.Remove);
        }
    }

    public void Clear()
    {
        this.key = default(TKey);
        willChanged?.Invoke(_value, this.key);
        _value.Clear();
        DataBind.I.Emit(this, DataBindAction.Clear);
    }

    public bool ContainsKey(TKey key)
    {
        return _value.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TData value)
    {
        return _value.TryGetValue(key, out value);
    }

    public int Count
    {
        get
        {
            return _value.Count;
        }
    }

    //重载[]运算符
    public TData this[TKey key]
    {
        get
        {
            return _value[key];
        }
        set
        {
            Set(key, value);
        }
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataDict<TKey, TData>, TKey, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, (c, data, act) =>
        {
            action(c, data, key, act);
        });
    }
}

#if UNITY_EDITOR
/*
[CustomEditor(typeof(#ScriptName#))]
public class #ScriptName#Editor : Editor
{

}
*/
#endif