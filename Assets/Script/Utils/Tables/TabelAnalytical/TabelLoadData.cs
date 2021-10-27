using UnityEngine;

public class TabelLoadData
{
    /// <summary>
    /// 处理出数据
    /// </summary>
    /// <param name="TableStruct"></param>
    /// <param name="data"></param>
    public static object[] ProcessTable(System.Type[] TableStruct,string tableName)
    {
        object[] objs;
        if (Application.isPlaying)
        {
            Debug.LogError("读取protobuf的表");
            //读取protobuf的表
            TextAsset txt = TableMgr.inst.GetTable(tableName) as TextAsset;
            objs =(object[])ProtobufTools.Deserialize(TableStruct[0],txt.bytes);
        }
        else
        {
            //读取本地的表
            objs=LoadLocalTable.ProcessTable(TableStruct[1], tableName);        
        }
        return objs;
    }
}