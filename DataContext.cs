using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
//已将文件转为UTF-8

public class DataContext : MonoBehaviour
{
    public string typeName;
    // Start is called before the first frame update
    void Start()
    {
        var type = Type.GetType(typeName);
        if (type == null)
        {
            Debug.LogError("Type not found: " + typeName);
            return;
        }

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