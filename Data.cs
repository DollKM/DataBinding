
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DataConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        IBindData d = value as IBindData;
        d.Write(writer, serializer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (existingValue == null)
        {
            existingValue = objectType.Assembly.CreateInstance(objectType.FullName);
        }
        IBindData d = existingValue as IBindData;
        d.Read(reader, serializer);
        return d;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsSubclassOf(typeof(IBindData));
    }
}

public class IBindData
{

    public virtual void Read(JsonReader reader, JsonSerializer serializer)
    {
    }

    public virtual void Write(JsonWriter writer, JsonSerializer serializer)
    {
    }
}

public class Data<TData> : IBindData
{
    protected TData _value;
    public TData Value
    {
        get => _value;
    }
    public delegate void DataOnChanged(TData old_value, TData new_value);

    [NonSerialized]
    public DataOnChanged willChanged;

    public override void Write(JsonWriter writer, JsonSerializer serializer)
    {
        serializer.Serialize(writer, _value);
    }

    public override void Read(JsonReader reader, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        _value = token.Value<TData>();
    }

    bool instack = false;
    protected void WillChange(TData value)
    {
        if (!instack)
        {
            instack = true;
            willChanged?.Invoke(_value, value);
            instack = false;
        }
    }
    public void Set(TData value)
    {
        if (!Compare(value))
        {
            WillChange(value);
            _value = value;
            DataBind.I.Emit(this, DataBindAction.Update);
        }
    }

    public void Init(TData value)
    {
        _value = value;
    }

    public virtual bool Compare(TData value)
    {
        return EqualityComparer<TData>.Default.Equals(_value, value);
    }

    //接下来的更新是否不检查值是否相等，总是触发
    public void SetAlways(TData value)
    {
        var old = _value;
        _value = value;
        willChanged?.Invoke(_value, value);
        DataBind.I.Emit(this, DataBindAction.Update);
    }

    public void Refresh()
    {
        willChanged?.Invoke(_value, _value);
        DataBind.I.Emit(this, DataBindAction.Update);
    }

    protected Data()
    {
    }
    protected Data(TData value)
    {
        _value = value;
    }

    public static bool operator ==(Data<TData> a, Data<TData> b)
    {
        if (a is null && b is null)
        {
            return true;
        }
        if (a is null || b is null)
        {
            return false;
        }
        return a._value.Equals(b._value);
    }

    public static bool operator !=(Data<TData> a, Data<TData> b)
    {
        return !(a == b);
    }
    // 重载运算符
    public static implicit operator TData(Data<TData> data)
    {
        return data._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }

}

