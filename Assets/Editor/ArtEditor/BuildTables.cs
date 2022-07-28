using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using WUtils;

public class BuildTables : Editor
{
    [MenuItem("Tools/导出二进制表")]
    public static void BuildChinese()
    {
        TableProcessing.TableClear();
        AssetDatabase.Refresh();
        BuildTable(Language_.Chinese);
    }
    static void BuildTable(Language_ tmpLan)
    {
        string root = string.Format("{0}/{1}", EditorPathTools.PROJECT_TABLES, tmpLan.ToString());
        if (!Directory.Exists(root))
        {
            Directory.CreateDirectory(root);
        }
        Type baseType = typeof(TableBaseManager);
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (baseType.IsAssignableFrom(type) && baseType != type)
                {
                    ByteTable(type, root);
                }
            }
        }
        AssetDatabase.Refresh();
    }
    static void ByteTable(Type type, string root)
    {
        TableBaseManager tableInstance = Activator.CreateInstance(type) as TableBaseManager;
        if (tableInstance.Open == false )
        {
            return;
        }
        var tableInfo = type.GetMethod("GetData");
        object tableData = tableInfo.Invoke(tableInstance, null);
        string path = string.Format("{0}/{1}.bytes", root, tableInstance.GetTableName());
        ProtobufTools.SerializeToFile(path, tableData);
    }

}
