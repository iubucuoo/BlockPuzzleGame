using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;

public class LoadLocalTable
{
    /// <summary>
    /// 处理出数据
    /// </summary>
    /// <param name="TableStruct"></param>
    /// <param name="data"></param>
    public static object[] ProcessTable(System.Type TableStruct, string tableName)
    {        
        TextAsset data = LoadTable(tableName);
        if (data==null)
        {
            return null;
        }
        //Debug.Log("加载表："+tableName);
        string[] lines = data.text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        if (IsFilter(lines.Length, data.name)) return null;

        Dictionary<int, object> propertyInfos = GetPropertyInfos(lines[1], TableStruct);

        object[] genericList = (object[])CreateGeneric(TableStruct.MakeArrayType(), TableStruct, lines.Length - 2);
        
        for (int i = 0; i < lines.Length - 2; i++)
        {
            TableBase _base = ParseObject(TableStruct, lines, i + 2, propertyInfos);
            genericList[i] = _base;
        }
        return genericList;

    }
    public static object CreateGeneric(Type generic, Type innerType, params object[] args)
    {
        return Activator.CreateInstance(generic, args);
    }

    static TableBase ParseObject(System.Type TableStruct, string[] lines, int idx, Dictionary<int, object> propertyInfos)
    {
        TableBase obj = (TableBase)Activator.CreateInstance(TableStruct);
        string line = lines[idx];
        string[] values = line.Split('\t');
        foreach (KeyValuePair<int, object> pair in propertyInfos)
        {
            if (pair.Key >= values.Length)
                continue;

            string value = values[pair.Key];
            if (string.IsNullOrEmpty(value))
                continue;

            try
            {
                ParsePropertyValue(obj, pair.Value, value);
            }
            catch
            {
                DebugMgr.LogError(string.Format("表数据错误: 表名={0} 字段名={1} 当前类型={2} 获取值={3}", 
                    TableStruct.ToString(),
                    (pair.Value as FieldInfo).Name,
                     (pair.Value as FieldInfo).FieldType.Name,
                      value));
                //Debuger.LogError(string.Format("表数据错误: Row={0} Column={1} Name={2} Want={3} Get={4} ",
                //    //idx + 1,
                //    pair.Key + 1,
                //    (pair.Value as FieldInfo).Name,
                //    (pair.Value as FieldInfo).FieldType.Name,
                //    value));
            }
        }
        return obj;
    }
    static void ParsePropertyValue(object obj, object fieldInfo, string valueStr)
    {
        System.Object value=null;
        if (fieldInfo is FieldInfo)
        {
            value = ParseValue((fieldInfo as FieldInfo).FieldType, valueStr);
            if (value != null)
                (fieldInfo as FieldInfo).SetValue(obj, value);
        }
        else
        {
            value = ParseValue((fieldInfo as PropertyInfo).PropertyType, valueStr);
            if (value != null)
                (fieldInfo as PropertyInfo).SetValue(obj, value,null);//glz
        }                
    }

    static object ParseValue(Type type, string valueStr)
    {
        if (type.IsEnum)
            return Enum.Parse(type, valueStr);
        else
        {
            if (type == typeof(int)) return int.Parse(valueStr);
            if (type == typeof(ushort)) return ushort.Parse(valueStr);
            if (type == typeof(byte)) return byte.Parse(valueStr);
            if (type == typeof(float)) return float.Parse(valueStr);
            if (type == typeof(double)) return double.Parse(valueStr);
            if (type == typeof(long)) return long.Parse(valueStr);
            if (type == typeof(bool)) return bool.Parse(valueStr);
            if (type == typeof(sbyte)) return sbyte.Parse(valueStr);
            if (type == typeof(short)) return short.Parse(valueStr);
            if (valueStr.Contains("\"\""))
                valueStr = valueStr.Replace("\"\"", "\"");

            // process the excel string.
            if (valueStr.Length > 2 && valueStr[0] == '\"' && valueStr[valueStr.Length - 1] == '\"')
                valueStr = valueStr.Substring(1, valueStr.Length - 2);

            return valueStr;
        }
    }

    /// <summary>
    /// 获得表字段
    /// </summary>
    /// <param name="fieldLine"></param>
    /// <param name="objType"></param>
    static Dictionary<int, object> GetPropertyInfos(string fieldLine, System.Type objType)
    {
        string[] infos = fieldLine.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries); 
        Dictionary<int, object> propertyInfos = new Dictionary<int, object>();
        for (int i = 0; i < infos.Length; i++)
        {
            
            var fieldInfo = objType.GetProperty(infos[i]); 
            if (fieldInfo == null)
            {
                var fieldInfo1 = objType.GetField(infos[i]);
                if (fieldInfo1==null)
                {
                    continue;
                }else
                {
                    propertyInfos[i] = fieldInfo1;
                }
            }
            else
            {
                propertyInfos[i] = fieldInfo;
            }
            
        }
        return propertyInfos;
    }


    #region 过滤

    /// <summary>
    /// 表格过滤
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    static bool IsFilter(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
        {
            DebugMgr.LogError("表名有问题");
            return true;
        }
        return false;
    }
    /// <summary>
    /// 表格过滤，当行数小于3行 时
    /// </summary>
    /// <param name="lenght">行数</param>
    /// <returns></returns>
    static bool IsFilter(int lenght, string name)
    {
        if (lenght < 3)
        {
            DebugMgr.LogError("表格文件行数错误，【1】属性名称【2】变量名称【3-...】值：" + name);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 读本地的表数据
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    #endregion


    static TextAsset LoadTabelForLocal(string tableName)
    {
#if UNITY_EDITOR
            string lan = Language_.Chinese.ToString();
        string tableRoot = Application.dataPath + "/Script/Utils/Tables/Data/" + lan + "/"+ tableName+".txt";
            //Debuger.Log("读取"+tableRoot);
            return UnityEditor.AssetDatabase.LoadMainAssetAtPath(PathCutOff(tableRoot, Format.Change_Z_Cut)) as TextAsset;
        #else
            return null;
        #endif
    }
    public static TextAsset LoadTable(string tableName)
    {
        if (IsFilter(tableName))
        {
            return null;
        }
        return LoadTabelForLocal(tableName);

    }

    /// <summary>
    /// 路径，以及调整斜杠
    /// </summary>
    /// <param name="tmpPath">路径</param>
    /// <param name="isZFormat">是否换成正斜杠/</param>
    /// <returns></returns>
    public static string PathCutOff(string tmpPath, Format isZFormat, string cutStr = "Assets")
    {
        string tmp = "";
        switch (isZFormat)
        {
            case Format.Change_F_Cut:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, false), false);
                break;
            case Format.Change_Z_Cut:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, false), true);
                break;
            case Format.Change_F_Cut_HaveHead:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, true), false);
                break;
            case Format.Change_Z_Cut_HaveHead:
                tmp = FormatChange(PathCutOff(tmpPath, cutStr, true), true);
                break;
            case Format.Change_F_NotCut:
                tmp = FormatChange(tmpPath, false);
                break;
            case Format.Change_Z_NotCut:
                tmp = FormatChange(tmpPath, true);
                break;
            case Format.Change_Not_Cut:
                tmp = PathCutOff(tmpPath, cutStr, false);
                break;
            case Format.Change_Not_Cut_HaveHead:
                tmp = PathCutOff(tmpPath, cutStr, true);
                break;
        }
        return tmp;
    }
    /// <summary>
    /// 只切割路径
    /// </summary>
    /// <param name="tmpPath"></param>
    /// <returns></returns>
     static string PathCutOff(string tmpPath, string CutStr, bool isHead)
    {
        int index = tmpPath.IndexOf(CutStr);
        if (isHead)
        {
            return tmpPath.Substring(0, index);
        }
        return tmpPath.Substring(index);
    }
    static string FormatChange(string tmpPath, bool isZFormat)
    {
        string startFormat = "\\";
        string endFormat = "/";

        if (!isZFormat)
        {
            startFormat = "/";
            endFormat = "\\";
        }
        return tmpPath.Replace(startFormat, endFormat);
    }
}
public enum Language_ : byte
{
    Chinese,
    English
}
public enum Format
{
    Change_Z_Cut,
    Change_F_Cut,
    Change_Not_Cut,
    Change_Z_NotCut,
    Change_F_NotCut,
    Change_Z_Cut_HaveHead,
    Change_F_Cut_HaveHead,
    Change_Not_Cut_HaveHead,
}