
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


public class DataDouble : Data<double>
{
    public DataDouble(double value) : base(value)
    {
    }

    public DataDouble()
    {
    }



    public override bool Compare(double value)
    {
        return _value == value;
    }

    // 重载加减乘除
    public void AddSelf(double value)
    {
        Set(_value + value);
    }

    public void SubSelf(double value)
    {
        Set(_value - value);
    }

    public bool AddSelfWithMax(double value, double max)
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

    public bool SubSelfWithMin(double value, double min)
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
    public static DataDouble operator ++(DataDouble data)
    {
        data.Set(data._value + 1);
        return data;
    }

    public static DataDouble operator --(DataDouble data)
    {
        data.Set(data._value - 1);
        return data;
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataDouble, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, action);
    }
}