
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


public class DataFloat : Data<float>
{
    public DataFloat(float value) : base(value)
    {
    }

    public DataFloat()
    {
    }



    public override bool Compare(float value)
    {
        return _value == value;
    }

    // 重载加减乘除
    public void AddSelf(float value)
    {
        Set(_value + value);
    }

    public void SubSelf(float value)
    {
        Set(_value - value);
    }

    public bool AddSelfWithMax(float value, float max)
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

    public bool SubSelfWithMin(float value, float min)
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
    public static DataFloat operator ++(DataFloat data)
    {
        data.Set(data._value + 1);
        return data;
    }

    public static DataFloat operator --(DataFloat data)
    {
        data.Set(data._value - 1);
        return data;
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataFloat, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, action);
    }
}