using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
//已将文件转为UTF-8

public class DataString : Data<string>
{

    public DataString(string value) : base(value)
    {
    }

    public DataString()
    {
    }
    public override bool Compare(string value)
    {
        return _value == value;
    }


    public void Bind<TComp>(TComp comp, Action<TComp, DataString, DataBindAction> action) where TComp : UnityEngine.Component
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