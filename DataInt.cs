
using System;
using Invokes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DataInt : Data<int>
{
    public DataInt(int value) : base(value)
    {
    }

    public DataInt()
    {
    }

    public override bool Compare(int value)
    {
        return _value == value;
    }

    public void AddSelf(int value)
    {
        Set(_value + value);
    }

    public void SubSelf(int value)
    {
        Set(_value - value);
    }
    public bool AddSelfWithMax(int value, int max)
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

    public bool SubSelfWithMin(int value, int min)
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

    public static DataInt operator ++(DataInt data)
    {
        data.AddSelf(1);
        return data;
    }

    public static DataInt operator --(DataInt data)
    {
        data.SubSelf(1);
        return data;
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataInt, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, action);
    }
}