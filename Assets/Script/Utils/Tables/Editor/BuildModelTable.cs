using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Text;

public class BuildModelTable : Editor
{

    static void Inter(string key,ref int index,Dictionary<string,ushort> kv,ref ushort id,int teshu_num=-1)
    {
        if (string.IsNullOrEmpty(key)==false&&key!="0")
        {
            key = key.ToLower();
            
            if (!kv.ContainsKey(key))
            {
                if (teshu_num != -1)
                {
                    int k = index % 10;
                    if (k > teshu_num)//5,1
                    {
                        //11-5
                        index += (10 + teshu_num - k);
                    }
                    else if (k < teshu_num)//0,1
                    {
                        index += (teshu_num - k);
                    }
                }
                kv.Add(key, (ushort)index);
                id = (ushort)index;
                index++;
            }
            else
            {
                id = kv[key];
            }
            
        }
    }
   
    static void Build(TableBase[] t,string path)
    {
        var item=t[0];
        Type ty = item.GetType();
        System.Reflection.FieldInfo[] PropertyInfo = ty.GetFields();
        StringBuilder sb = new StringBuilder();
        int max = PropertyInfo.Length;
        int max_1 = max - 1;
        for (int j = 0; j < 2; j++)
        {            
            for (int i = 0; i < max; i++)
            {                
                sb.Append(PropertyInfo[i].Name);
                if (i< max_1)
                {
                    sb.Append("\t");
                }                
            }
            sb.Append("\n");
        }

        for (int k = 0; k < t.Length; k++)
        {
            for (int i = 0; i < max; i++)
            {
                if (PropertyInfo[i].Name!="Have")
                {
                    /*Debug.Log(PropertyInfo[i].Name + "," + PropertyInfo[i].GetValue(t[0]));*/
                    sb.Append(PropertyInfo[i].GetValue(t[k]));
                    if (i < max_1)
                    {
                        sb.Append("\t");
                    }
                }                
            }
            sb.Append("\n");
        }        
        //Debug.LogError(sb.ToString());

        var s = System.IO.File.CreateText(path);
        s.WriteLine(sb.ToString());
        s.Dispose();
    }
}
