
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DataObject<TData> : Data<TData> where TData : class
{
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Compare(TData value)
    {
        return _value == value;
    }

    public override void Read(JsonReader reader, JsonSerializer serializer)
    {
        JObject token = JObject.Load(reader);
        _value = token.ToObject<TData>(serializer);
    }

    // 重载==判断
    public static bool operator ==(DataObject<TData> data, TData value)
    {
        return data._value == value;
    }

    public static bool operator !=(DataObject<TData> data, TData value)
    {
        return data._value != value;
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataObject<TData>, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, action);
    }
}

