using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class TobeTxt //: Editor
{
	public static void CreatTxt(string file_path, string[] lines)
	{
		StringBuilder sb = ChangeToTxt(file_path, lines);
		File.WriteAllText(file_path, sb.ToString());
	}

	public static StringBuilder ChangeToTxt(string file_path, string[] lines)
	{
		StringBuilder sb = new StringBuilder();
		string tablename = file_path.Substring(file_path.LastIndexOf('\\') + 1);
		string f_line = "";
		string s_line = "";
		bool is_body = false;
		bool isMap = file_path.Contains("map.txt");
		bool ismissile = tablename.Equals("missile.txt");
		bool ismonster = tablename.Equals("monster.txt");
		int monster_attrpos = 0;
		int missile_tagpos = 0;
		for (var i = 1; i < lines.Length; i++)
		{
			if ("[data]".Equals(lines[i]))
			{
				is_body = true;
				if (ismonster)
				{
					s_line += "\t" + "speed";
					f_line += "\t" + "speed";
				}
				sb.AppendFormat("{0}\r{1}\r", f_line, s_line);
				continue;
			}
			if (is_body)
			{
				if (isMap)
				{
					string[] unit = lines[i].Split("\t".ToCharArray());
					if (unit[1].LastIndexOf("一一一") >= 0)
					{
						continue;
					}
					sb.Append(ChangeCs(lines[i]));
				}
				else
				{
					sb.Append(ChangeCs(lines[i]));
				}
				if (ismonster)
				{
					string _speed = "1";
					string[] unit = lines[i].Split("\t".ToCharArray());
					string attr = unit[monster_attrpos];
					string[] at = attr.Split("|".ToCharArray());
					for (int k = 0; k < at.Length; k++)
					{
						string[] at_m = at[k].Split(":".ToCharArray());
						if (at_m[0] == "5")
						{
							_speed = at_m[1];
						}
					}
					sb.Append("\t" + _speed);
				}
				sb.Append("\r");
			}
			else
			{
				string[] unit = lines[i].Split("\t".ToCharArray());
				var ccs0 = ChangeCs(unit[0]);
				if (ismonster && unit[0].Equals("attr"))
				{
					monster_attrpos = i - 1;
				}
				if (ismissile && unit[0].Equals("tag"))
				{
					missile_tagpos = i - 1;
				}
				if (unit.Length < 4)
				{
					f_line += i != 1 ? "\t" + ccs0 : "" + ccs0;
				}
				else
				{
					var ccs3 = ChangeCs(unit[3]);
					f_line += i != 1 ? "\t" + ccs3 : "" + ccs3;
				}
				s_line += i != 1 ? "\t" + ccs0 : "" + ccs0;
			}
		}
		return sb;
	}

	static string ChangeCs(string str)
	{
		if (TableStrTool.MP3(str))
		{
			return str.Replace(".mp3", "");
		}
		if (str.Contains("Selected_0"))
		{
			//处理怪物脚下的特效
			str = str.Replace("Selected_04", "2").Replace("Selected_03", "3");
		}
		if (str.Contains("Choose__"))
		{
			str = str.Replace("Choose__", "");
		}
		if (str.Contains("foot_01"))
		{
			str = str.Replace("foot_01", "1");
		}
		return str;
	}
}
