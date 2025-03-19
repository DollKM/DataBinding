using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
//已将文件转为UTF-8

delegate void DataEmitHandler(Component comp, IBindData value, DataBindAction action);

public enum DataBindAction
{
    Init,
    Update,
    Add,
    Remove,
    Clear,
    Sort,
}

public class DataBind : SingletonMono<DataBind>
{

    DictionaryEX<IBindData, DictionaryEX<Component, DataEmitHandler>> cacheWithDatas = new((k) => new DictionaryEX<Component, DataEmitHandler>());
    DictionaryEX<Component, HashSet<IBindData>> cacheWithComps = new((k) => new HashSet<IBindData>());

    public void Bind<DataT, TComp>(DataT Ddata, TComp Dcomp, Action<TComp, DataT, DataBindAction> Dacion) where TComp : Component where DataT : IBindData
    {
        if (Dcomp == null)
        {
            return;
        }
        cacheWithDatas[Ddata][Dcomp] = (comp, data, action) =>
        {
            Dacion?.Invoke(comp as TComp, (DataT)data, action);
        };
        cacheWithComps[Dcomp].Add(Ddata);
        Dacion(Dcomp, (DataT)Ddata, DataBindAction.Init);
    }

    bool needRefresh = false;

    HashSet<IBindData> currentDatas = new HashSet<IBindData>();
    public void Emit(IBindData data, DataBindAction action)
    {
        bool isRemove = false;
        if (currentDatas.Contains(data))
        {
            return;
        }
        currentDatas.Add(data);
        if (cacheWithDatas.TryGetValue(data, out var comps))
        {
            foreach (var pair in comps)
            {
                if (pair.Key == null)
                {
                    isRemove = true;
                }
                else
                {
                    pair.Value(pair.Key, data, action);
                }
            }
        }
        currentDatas.Remove(data);
        if (isRemove)
        {
            if (!needRefresh)
            {
                needRefresh = true;
                StartCoroutine(EndFrame());
            }
        }
    }

    public void Clear(Component comp)
    {
        if (comp == null)
        {
            return;
        }
        if (cacheWithComps.TryGetValue(comp, out var datas))
        {
            foreach (var data in datas)
            {
                cacheWithDatas[data].Remove(comp);
                if (cacheWithDatas[data].Count == 0)
                {
                    cacheWithDatas.Remove(data);
                }
            }
            cacheWithComps.Remove(comp);
        }
    }

    public void Clear(IBindData data)
    {
        if (cacheWithDatas.TryGetValue(data, out var comps))
        {
            foreach (var comp in comps)
            {
                cacheWithComps[comp.Key].Remove(data);
                if (cacheWithComps[comp.Key].Count == 0)
                {
                    cacheWithComps.Remove(comp.Key);
                }
            }
            cacheWithDatas.Remove(data);
        }
    }

    List<Component> removedComps = new List<Component>();
    IEnumerator EndFrame()
    {
        yield return Yielders.EndOfFrame;
        //把无效的信号清空
        foreach (var pair in cacheWithComps)
        {
            if (pair.Key == null)
            {
                //组件已经被释放，相关的数据检查一下绑定组件的数量，没有的话就也清空
                foreach (var data in pair.Value)
                {
                    cacheWithDatas[data].Remove(pair.Key);
                    if (cacheWithDatas[data].Count == 0)
                    {
                        cacheWithDatas.Remove(data);
                    }
                }
                removedComps.Add(pair.Key);
            }
        }
        for (int i = 0; i < removedComps.Count; i++)
        {
            cacheWithComps.Remove(removedComps[i]);
        }
        removedComps.Clear();
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