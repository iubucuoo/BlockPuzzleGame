using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TobeLuaStrTool : Editor
{

	/// <summary>
	/// 设置结构cell_unit
	/// </summary>
	/// <param name="lines_i"></param>
	/// <returns></returns>
	public static Cell_unit Set_cell_unit(string lines_i)
	{
		string[] unit = lines_i.Split("\t".ToCharArray());
		Cell_unit _unit = new Cell_unit
		{
			name = unit[0],
			type = TableStrTool.get_type(unit[1]),
			remove = !(unit.Length > 3 && unit[3].Contains(TableSign.L))
		};
		if (unit.Length > 3 && unit[3].Contains(TableSign.TBT))
			_unit.special_type = LuaSpecial_type.ToBinaryTable;
		else if (unit.Length > 3 && unit[3].Contains(TableSign.DMH))
			_unit.special_type = LuaSpecial_type.DanMH;
		else
			_unit.special_type = LuaSpecial_type.None;
		return _unit;
	}

	/// <summary>
	/// lua表的值 字符串类型的处理 (char or string类型才可)
	/// </summary>
	/// <param name="list_z"></param>
	/// <param name="unit_z"></param>
	/// <returns></returns>
	public static string Value_str(Cell_unit list_z, string unit_z)
	{
		if (list_z.special_type == LuaSpecial_type.ToBinaryTable)
			return TantoTwoTable(unit_z);
		else if (list_z.special_type == LuaSpecial_type.DanMH)
			return str_danmaohao(unit_z);
		else
			return ChangeUBB(unit_z);
	}

	/// <summary>
	/// lua表的值 数字类型的处理 (int or float类型才可)
	/// </summary>
	/// <param name="file_name"></param>
	/// <param name="list_z"></param>
	/// <param name="unit_z"></param>
	/// <returns></returns>
	public static string Value_num(string file_name, Cell_unit list_z, string unit_z)
	{
		string _tmp = "";
		if (!string.IsNullOrEmpty(unit_z))
		{
			try
			{
				if (list_z.special_type == LuaSpecial_type.ToBinaryTable)
					_tmp = TantoTwoTable(unit_z);
				else
				{
					if (list_z.type == 1)
					{
						int outint;
						_tmp += int.TryParse(unit_z, out outint) ? int.Parse(unit_z) : long.Parse(unit_z);
					}
					else if (list_z.type == 3)
						_tmp = unit_z;
				}
			}
			catch
			{
				Log.Error("______表结构错误  表名: " + file_name + "    字段名: " + list_z.name + "    字段内容: " + unit_z);
			}
		}
		return _tmp;
	}

	static string ChangeUBB(string str)
	{
		if (TableStrTool.MP3(str))
		{
			return str.Replace(".mp3", "");
		}
		string[] strs = str.Split("|".ToCharArray());
		if (strs.Length == 1)
		{
			return Attr(str);
		}
		string s = "";
		bool f = true;
		int index = 0;
		for (int i = 0; i < strs.Length; i++)
		{
			if ("n".Equals(strs[i]))
			{
				s = s + "\\n";
			}
			else if (strs[i].Contains("#"))
			{
				if (strs[i].Length == 7 && !strs[i].Contains("_"))
				{
					if (f)
					{
						s = s + "<font color=" + strs[i] + ">";
						index++;
						f = false;
					}
					else
					{
						if (strs[i - 1] == "n")
						{
							s = s.Substring(0, s.Length - 2);
							s = s + "</font>\\n<font color=" + strs[i] + ">";
						}
						else
						{
							s = s + "</font><font color=" + strs[i] + ">";
						}
						index++;
					}
				}
				else
				{
					s = s + strs[i];
				}

			}
			else
			{
				s = s + strs[i];
			}
		}
		if (index > 0)
		{
			s = s + "</font>";
			return "\"" + s + "\"";
		}
		else
		{
			return Attr(str);
		}

	}
	static string Attr(string str)
	{
		if ((str.Contains("|") && str.Contains(":")) || (str.Split(":".ToCharArray()).Length >= 2) && !str.Contains("::") && !str.Contains("}:{"))
		{
			if (str == ":") { return "\"" + str + "\""; }
			string result = "{";
			string[] strs = str.Split("|".ToCharArray());
			for (int i = 0; i < strs.Length; i++)
			{
				string[] substrs = strs[i].Split(":".ToCharArray());
				result += "{";
				for (int j = 0; j < substrs.Length; j++)
				{
					var s = substrs[j];
					int b;
					float c;
					if (!int.TryParse(s, out b) && !float.TryParse(s, out c))
						s = "\"" + s + "\"";
					if (j == substrs.Length - 1)
						result += s;
					else
						result += s + ",";
				}
				if (i == strs.Length - 1)
					result += "}";
				else
					result += "},";
			}
			result += "}";
			return result;
		}
		else
		{
			return SingleColor(str);
		}
	}
	static string SingleColor(string str)
	{
		if (str.Length == 7 && '#'.Equals(str[0]))
		{
			str = "\"<font color=" + str + ">\"";
			return str;
		}
		else
		{
			return "\"" + str + "\"";
		}
	}


	/// <summary>
	/// 针对有些没有冒号 有些多个冒号进行的特殊处理   1  or   1:2:3 
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	static string str_danmaohao(string str)
	{
		var k = "{";
		string[] substrs = str.Split(":".ToCharArray());
		for (int s = 0; s < substrs.Length; s++)
		{
			k += substrs[s];
			if (s != substrs.Length - 1)
			{
				k += ",";
			}
		}
		k += "}";
		return k;
	}



	/// <summary>
	/// lua里关键词加 _  前缀处理
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static string _Prefix(string str)
	{
		if ("repeat".Equals(str))
		{
			str = "_repeat";
		}
		else if ("end".Equals(str))
		{
			str = "_end";
		}
		return str;
	}



	static string TantoTwoTable(string _value)
	{
		string valuestr = "";
		if (int.TryParse(_value, out int value))
		{
			string Binary = Convert.ToString(value, 2);
			string reverseBinary = "";
			for (int k = Binary.Length - 1; k >= 0; k--)
			{
				reverseBinary += Binary[k];
			}
			for (int k = 0; k < reverseBinary.Length; k++)
			{
				if (int.Parse(reverseBinary[k].ToString()).Equals(1))
				{
					if (k != 0 && valuestr != "")
					{
						valuestr += "," + (k + 1);
					}
					else
					{ valuestr += (k + 1); }
				}
			}
			valuestr = "{" + valuestr + "}";
		}
		else
			Log.Error("二进制转表  表的值不是int类型__" + _value);
		return valuestr;
	}
}
