using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TobeCS : Editor
{
	static readonly string struct_root = Application.dataPath + "/Scripts/Utils/Tables/Structs/";
	public static void CreatCS(string[] lines, string fileName)
	{
		string cspath = struct_root + fileName.Replace(".txt", ".cs");
		if (!File.Exists(cspath))//不存在CS脚本创建
		{
			Creat_cs(lines, fileName);
		}
	}
	static void Creat_cs(string[] lines, string file_name)
	{
		bool usering = false;
		file_name = file_name.Replace(".txt", "");
		StringBuilder sb = new StringBuilder();
		string tupfilename = file_name.Substring(0, 1).ToUpper() + file_name.Substring(1);//首字母大写
		sb.Append("using System;\nusing ProtoBuf;\n[ProtoContract]\npublic class ").Append(tupfilename).Append(":TableBase\n{\n");
		string key = "0";
		int index = 0;
		for (int i = 1; i < lines.Length; i++)
		{
			if (lines[i] != "[data]")
			{
				string[] unit = lines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (unit.Length > 3 && unit[3].Contains(TableSign.CS))
				{
					usering = true;
					sb.AppendFormat("\t[ProtoMember({0})]\n", ++index);
					sb.AppendFormat("\tpublic {0} {1};\n", TableStrTool.str_type(unit[1], unit[2]), unit[0]);
					if (i == 1)
					{
						if (tupfilename == "Number" || tupfilename == "Sys_server_config")
							key = "1";
						else
							key = unit[0];
					}
				}
			}
			else
			{
				break;
			}
		}
		sb.Append("\t public override int Key()\n\t{\n\treturn ").Append(key).Append(";\n\t}\n}\n");
		sb.Append("public class ")
			.Append(tupfilename).Append("Manager: TableBaseManager\n{\n\tpublic override string GetTableName()\n\t{\n\t\treturn \"")
			.Append(tupfilename).Append("\";\n\t}\n\tpublic override bool Open { get { return true; } }\n#region ")
			.Append(tupfilename).Append("\nstatic ")
			.Append(tupfilename).Append("Manager mInstance;\nstatic ")
			.Append(tupfilename).Append("Manager instance\n{\nget\n{\nif (mInstance == null)\n{\nmInstance=new ")
			.Append(tupfilename).Append("Manager();\n}\n\treturn (")
			.Append(tupfilename).Append("Manager)mInstance;\n}\n}\nprotected override Type[] GetTableType()\n{\n\treturn new Type[] {typeof(")
			.Append(tupfilename).Append("[]),typeof(")
			.Append(tupfilename).Append(")};\n}\npublic static ")
			.Append(tupfilename).Append("[] GetData()\n{\n\treturn(")
			.Append(tupfilename).Append("[])instance._GetData();\n}\npublic static ")
			.Append(tupfilename).Append(" GetSingleData(int key)\n{\n\treturn (")
			.Append(tupfilename).Append(")instance._GetSingleData(key);\n}\n\n#endregion\n}");
		if (usering)
		{
			TableStrTool.CreatFile(string.Concat(struct_root, tupfilename, ".cs"), sb);
		}
		else
		{
			//Debug.Log("删除掉的文件路径：" + struct_root + tupfilename + ".cs");
			File.Delete(struct_root + tupfilename + ".cs");
		}
	}
}
