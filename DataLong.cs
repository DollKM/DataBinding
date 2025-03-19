
using System;
using Invokes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DataLong : Data<long>
{
    public DataLong(long value) : base(value)
    {
    }

    public DataLong()
    {
    }

    public override bool Compare(long value)
    {
        return _value == value;
    }

    public void AddSelf(long value)
    {
        Set(_value + value);
    }

    public void SubSelf(long value)
    {
        Set(_value - value);
    }
    public bool AddSelfWithMax(long value, long max)
    {
        if (_value + value >= max)
        {
            Set(max);
            return true;
        }
        else
        {
            Set(_value + value);
            return false;
        }
    }

    public bool SubSelfWithMin(long value, long min)
    {
        if (_value - value <= min)
        {
            Set(min);
            return true;
        }
        else
        {
            Set(_value - value);
            return false;
        }
    }
    // 重载加减乘除

    public static DataLong operator ++(DataLong data)
    {
        data.AddSelf(1);
        return data;
    }

    public static DataLong operator --(DataLong data)
    {
        data.SubSelf(1);
        return data;
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataLong, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, action);
    }
}