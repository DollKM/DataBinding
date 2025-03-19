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

public class DataList<TData> : Data<List<TData>>
{
    public delegate void DataListOnChanged(List<TData> list, int index);

    [NonSerialized]
    new public DataListOnChanged willChanged;
    public DataList()
    {
        _value = new List<TData>();
    }

    public DataList(List<TData> value)
    {
        _value = value;
    }

    public override void Read(JsonReader reader, JsonSerializer serializer)
    {
        JArray token = JArray.Load(reader);
        _value = token.ToObject<List<TData>>(serializer);
    }

    public void Add(TData data)
    {
        index = _value.Count;
        willChanged?.Invoke(_value, index);
        _value.Add(data);
        DataBind.I.Emit(this, DataBindAction.Add);
    }

    public void Insert(int idx, TData data)
    {
        index = idx;
        willChanged?.Invoke(_value, index);
        _value.Insert(index, data);
        DataBind.I.Emit(this, DataBindAction.Add);
    }
    public int index = -1;

    public void Remove(TData data)
    {
        int idx = _value.IndexOf(data);
        if (idx >= 0)
        {
            index = idx;
            willChanged?.Invoke(_value, index);
            _value.RemoveAt(idx);
            DataBind.I.Emit(this, DataBindAction.Remove);
        }
    }

    public void RemoveAt(int idx)
    {
        if (idx >= 0 && idx < _value.Count)
        {
            index = idx;
            willChanged?.Invoke(_value, index);
            _value.RemoveAt(idx);
            DataBind.I.Emit(this, DataBindAction.Remove);
        }
    }

    public void Clear()
    {
        index = -1;
        willChanged?.Invoke(_value, index);
        _value.Clear();
        DataBind.I.Emit(this, DataBindAction.Clear);
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataList<TData>, TData, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, (c, data, act) =>
        {
            action(c, data, (data.index >= 0 && data.index < data.Value.Count) ? data.Value[data.index] : default, act);
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