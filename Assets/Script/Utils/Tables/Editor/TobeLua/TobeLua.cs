using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System;
/// <summary>
/// 打lua表  char转成特殊值的类型
/// </summary>
public enum LuaSpecial_type
{
	None,//默认类型

	DanMH,//[DMH](目前就字符 类型用) 单个冒号分割

	Announcement,//特定的 公告表的text字段特殊处理

	Banned_word,// 特定的 banned_word表特殊

	ToBinaryTable,//[TBT] （字符与值类型都要用）  十进制转二进制的 位上值为1的加入表 gem skill 表 label字段用

	LastName// 特定的  lastName表特殊
}

public struct Cell_unit
{
	public string name;
	public byte type;
	public bool remove;
	public LuaSpecial_type special_type;
}

public class TobeLua : Editor
{
	static readonly string luatable_root = Application.dataPath + "/Lua/Tables/";
	public static void Creat_lua(string[] lines, string file_name)
	{
		StringBuilder sb = new StringBuilder();
		List<Cell_unit> list = new List<Cell_unit>();
		int index = 0;
		bool isContent = false;
		bool NeedLua = false;
		for (int i = 0; i < lines.Length; i++)
		{
			if (TableSign.Header.Equals(lines[i]))
			{
				sb.Append("local data = {}\n");
			}
			else if (TableSign.Data.Equals(lines[i]))
			{
				isContent = true;
			}
			else if (isContent != true)
			{
				list.Add(TobeLuaStrTool.Set_cell_unit(lines[i]));
			}
			else
			{
				string[] unit = lines[i].Split("\t".ToCharArray());
				index++;
				sb.AppendFormat("data[{0}] = ", index).Append("{");
				for (int z = 0; z < unit.Length; z++)
				{
					if (list.Count <= z || list[z].remove)
						continue;
					NeedLua = true;
					switch (list[z].type)
					{
						case 1:
						case 3:
							sb.AppendFormat("{0} = {1}",TobeLuaStrTool._Prefix(list[z].name), TobeLuaStrTool.Value_num(file_name, list[z], unit[z]));
							break;
						case 2:
							sb.AppendFormat("{0} = {1}", TobeLuaStrTool._Prefix(list[z].name), TobeLuaStrTool.Value_str(list[z], unit[z]));
							break;
						default:
							break;
					}
					if (z != unit.Length - 1)
					{
						sb.Append(",\t");
					}
				}
				sb.Append("}\n");
			}
		}
		sb.Append("return data");
		if (NeedLua)
			File.WriteAllText(luatable_root + file_name.Replace(".txt", ".lua"), sb.ToString());
		else
			File.Delete(luatable_root + file_name.Replace(".txt", ".lua"));   //没有使用的字段  删除该脚本
	}
}
