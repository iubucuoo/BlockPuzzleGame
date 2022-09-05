using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using ProtoBuf;
using WUtils;

public class BuildTable : Editor
{
	/// <summary>
	/// 所有的表table导出byte文件到table文件下
	/// </summary>
	public static void BuildChinese()
	{
		TableProcessing.TableClear();
		AssetDatabase.Refresh();
		BuildTables();
		Log.Info("BuildTable  bytes  OverOver");
	}

	static void BuildTables()
	{
		string root = string.Format("{0}/Chinese", PathTools.PROJECT_TABLES);
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
				if (baseType.IsAssignableFrom(type) && baseType != type)//&& typeof(CreateTableStyleManager) != type
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
		if (tableInstance.Open == false)
		{
			return;
		}
		var tableInfo = type.GetMethod("GetData");
		object tableData = tableInfo.Invoke(tableInstance, null);
		string path = string.Format("{0}/{1}.bytes", root, tableInstance.GetTableName());
		using (FileStream stream = new FileStream(path, FileMode.Create))
		{
			Serializer.Serialize(stream, tableData);
		}
	}
	//[MenuItem("ITools/XXXXXXXXXXXDeserializeTable",false,0)]
	static void DeserializeTable()
	{
		string tableRoot = @"Assets\Art\Tables\Chinese\Item.bytes";// Item 
		var txta = ObjectMgr.LoadMainAssetAtPath(tableRoot) as TextAsset;
		object result = ProtobufTools.Deserialize(typeof(Item[]), txta.bytes);
		//Log.Error(result);
	}
	 
	static void TestSetData(object tableData, string name)
	{
		string root = string.Format("{0}/Testttttttt", Application.dataPath);
		if (!Directory.Exists(root))
		{
			Directory.CreateDirectory(root);
		}
		string path = string.Format("{0}/{1}.bytes", root, name);
		using (FileStream stream = new FileStream(path, FileMode.Create))
		{
			Serializer.Serialize(stream, tableData);
		}
	}
}
