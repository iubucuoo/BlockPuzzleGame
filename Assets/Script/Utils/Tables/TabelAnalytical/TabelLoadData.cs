using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class TabelLoadData
{
    /// <summary>
    /// 处理出数据
    /// </summary>
    /// <param name="TableStruct"></param>
    /// <param name="data"></param>
    public static object[] ProcessTable(Type[] TableStruct,string tableName)
    {
        if (Application.isPlaying)
        {
            Debug.LogError("读取protobuf的表");
            //读取protobuf的表
            var bytes = TableMgr.Inst.GetTable(tableName);
            return (object[])ProtobufTools.Deserialize(TableStruct[0],bytes);
        }
        else
        {
            //读取本地的表
            return LoadLocalTable.ProcessTable(TableStruct[1], tableName);        
        }
    }
}