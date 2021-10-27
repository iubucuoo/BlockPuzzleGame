using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BuildTables : Editor
{
    static string GetRoot(string root = "")
    {
        return root == "" ? string.Format("{0}/Art/Tables/", Application.dataPath) : root;
    }
    [MenuItem("Tools/导出二进制表")]
    public static void BuildChinese()
    {
        BuildTable(Language_.Chinese, "");
    }
    static void BuildTable(Language_ tmpLan, string root)
    {
        Type baseType = typeof(TableBaseManager);
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (baseType.IsAssignableFrom(type) && baseType != type)
                {
                    ByteTable(type,  tmpLan, root);
                }
            }
        }
        AssetDatabase.Refresh();
    }
    static void ByteTable(Type type,  Language_ lan, string _root)
    {
        string root = GetRoot(_root);
        TableBaseManager tableInstance = Activator.CreateInstance(type) as TableBaseManager;
        if (tableInstance.Open == false )
        {
            return;
        }
        var tableInfo = type.GetMethod("GetData");
        object tableData = tableInfo.Invoke(tableInstance, null);
        root = root + lan.ToString();
        if (!Directory.Exists(root))
        {
            Directory.CreateDirectory(root);
        }
        string path = string.Format("{0}/{1}.bytes", root, tableInstance.GetTableName());
        ProtobufTools.SerializeToFile(path, tableData);
    }

}
