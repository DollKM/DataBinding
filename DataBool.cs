using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
//已将文件转为UTF-8

public class DataBool : Data<bool>
{
    public DataBool(bool value) : base(value)
    {
    }

    public DataBool()
    {
    }



    public override bool Compare(bool value)
    {
        return _value == value;
    }

    public void Bind<TComp>(TComp comp, Action<TComp, DataBool, DataBindAction> action) where TComp : UnityEngine.Component
    {
        DataBind.I.Bind(this, comp, action);
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