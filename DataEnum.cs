
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public interface IDataEnum { }

public class DataEnum<TData> : Data<TData>, IDataEnum where TData : Enum
{
    public DataEnum(TData value) : base(value)
    {
    }

    public DataEnum()
    {
    }

    public override bool Compare(TData value)
    {
        return _value.Equals(value);
    }

    public override void Read(JsonReader reader, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        int intValue = token.Value<int>();
        _value = (TData)(object)intValue;
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataEnum<TData>, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, action);
    }
}

