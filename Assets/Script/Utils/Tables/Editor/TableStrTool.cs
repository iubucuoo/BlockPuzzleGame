using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TableStrTool//:Editor
{	
	public static bool MP3(string str)
	{
		return str.Contains(".mp3");
	}

	/// <summary>
	/// 敏感词替换
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static string Shielding(string str)
	{
		return str.Replace("show", "sw").Replace("Show", "Sw").Replace("hide", "hd").Replace("pay", "gmai").Replace("money", "qian").Replace("weixin", "x").Replace("order", "sequ").Replace("web", "server");
	}
	/// <summary>
	/// 编码格式
	/// </summary>
	/// <param name="buffer"></param>
	/// <returns></returns>
	public static Encoding GetEncode(byte[] buffer)
	{
		if (buffer.Length <= 0 || buffer[0] < 239)
			return Encoding.Default;
		if (buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
			return Encoding.UTF8;
		if (buffer[0] == 254 && buffer[1] == byte.MaxValue)
			return Encoding.BigEndianUnicode;
		if (buffer[0] == byte.MaxValue && buffer[1] == 254)
			return Encoding.Unicode;
		return Encoding.Default;
	}




	#region 针对某个表的特殊处理
	/// <summary>
	/// missile表的第24个字段处理
	/// </summary>
	/// <param name="tag"></param>
	/// <returns></returns>
	static int Tagchartoint(string tag)
	{
		if (tag.Equals("") || tag.Equals("0"))
			return 0;
		string[] sp = tag.Split(':');
		int[] intzu = new int[sp.Length];
		for (int i = 0; i < sp.Length; i++)
		{
			intzu[i] = int.Parse(sp[i]);
		}
		string two = "";
		for (int i = 1; i <= intzu[intzu.Length - 1]; i++)
		{
			bool isone = false;
			for (int j = 0; j < intzu.Length; j++)
			{
				if (i == intzu[j])
				{
					isone = true;
					two += "1";
				}
			}
			if (!isone)
			{
				two += "0";
			}
		}
		string fantwo = "";
		for (int k = two.Length - 1; k >= 0; k--)
		{
			fantwo += two[k];
		}
		int value = (int)System.Convert.ToInt64(fantwo, 2);
		return value;
	}

	public static string str_act(string t, bool isAct, int index)
	{
		if (isAct && index == 13)
		{//week的字段的星期
			string _str = "{";
			int len = t.Length;
			for (int i = 0; i < len; i++)
			{
				_str += t[i] + ",";
			}
			_str += "}";
			return _str;
		}
		return t;
	}
	#region ------公告表特殊处理
	public static bool Colorexist(string str)
	{
		return str.Contains("#C_R_") || str.Contains("#C_Y_") || str.Contains("#C_G_") || str.Contains("#C_P_") || str.Contains("#C_O_") || str.Contains("#C_B_");
	}
	public static string ColorChange(string str)
	{
		return str.Replace("#C_R_", "<font color=#EA1C1C>").Replace("#C_Y_", "<font color=#FFC000>").Replace("#C_G_", "<font color=#00FF1E>").Replace("#C_P_", "<font color=#ff49f4>").Replace("#C_O_", "<font color=#FFC000>").Replace("#C_B_", "<font color=#2CA0F1>");
	}
	public static string CommomText(string str)
	{
		string[] col = { "", "", "#C_R_", "#C_Y_", "#C_G_", "#C_P_", "#C_O_", "#C_B_" };
		string[] strs = str.Split("|".ToCharArray());
		string s = "{";
		bool ispanel = false;
		string paneladd = "";
		for (int i = 0; i < strs.Length; i++)
		{
			if (Colorexist(strs[i]))
			{
				strs[i] = ColorChange(strs[i]) + "</font>";
			}

			if (strs[i].Contains("#L_PANEL"))
			{
				strs[i] = strs[i].Replace("#L_PANEL", "");
				string[] panel = strs[i].Split("_".ToCharArray());
				if (panel.Length == 5)
				{
					// strs[i] = panel[0] + "<font color=#00FF1E>" + "<a href='10:" + panel[1] + ":" + panel[2] + ":" + panel[3] + "'> " + panel[4] + " </a>" + "</font>";
					strs[i] = panel[0] + " <font color=#00FF1E>" + panel[4] + "</font> ";
					paneladd = "<font color=#00FF1E><a href='10:" + panel[1] + ":" + panel[2] + ":" + panel[3] + "'>" + " [ 我也要 ]" + "</a></font>";
				}
				else if (panel.Length == 6)
				{
					//strs[i] = panel[0] + ColorChange(col[int.Parse(panel[4])]) + "<a href='10:" + panel[1] + ":" + panel[2] + ":" + panel[3] + "'> " + panel[5] + " </a>" + "</font>";
					strs[i] = panel[0] + " " + ColorChange(col[int.Parse(panel[4])]) + panel[5] + "</font> ";
					paneladd = "<font color=#00FF1E><a href='10:" + panel[1] + ":" + panel[2] + ":" + panel[3] + "'>" + " [ 我也要 ]" + "</a></font>";
				}
				ispanel = true;
			}
			s = s + "\"" + strs[i] + "\",";
		}
		if (strs.Length > 1)
			if (ispanel)
				return s + "\"" + paneladd + "\"," + "}";
			else
				return s + "}";
		else
			return "{\"" + str + "\"}";
	}
	#endregion
	public static string BannedName(string t)
	{
		string _str = "\"" + t[0] + "\",\"";
		int len = t.Length;
		for (int i = 0; i < len; i++)
		{
			_str += t[i];
		}
		_str += "\"";
		return _str;
	}
	#endregion

	#region [header]部分的处理
	//-----------------------------cfg转其他的
	/// <summary>
	/// 类型装换
	/// </summary>
	/// <param name="str"></param>
	/// <param name="lenght"></param>
	/// <returns></returns>
	public static string str_type(string str, string lenght)
	{
		switch (str)
		{
			case "int":
				int len = int.Parse(lenght);
				if (len == 2)
				{
					return "sbyte";
				}
				else if (len == 4)
				{
					return "short";
				}
				else
				{
					return "int";
				}
			case "float":
			case "Float":
				return "float";
			case "char":
			case "String":
				return "string";
			default:
				return "";
		}
	}

	/// <summary>
	/// 拿到cfg表 的类型 来区分
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static byte get_type(string str)
	{
		switch (str)
		{
			case "int": return 1;
			case "float": return 3;
			case "char": return 2;
			case "String": return 2;
			default:
				 Log.Error("没有" + str + "格式");
				return 0;
		}
	}
	//-----------------------------生成cfg的

	public static string str_typeToCfg(string str)
	{
		switch (str)
		{
			case "Boolean":
			case "Int32":
				return "int";
			case "Single":
				return "float";
			case "String":
			case "V3":
			case "Int32[]":
				return "char";
			default:
				return "";
		}
	}

	public static string str_lengthToCfg(string str)
	{
		switch (str)
		{
			case "float":
			case "int":
				return "11";
			case "char":
				return "32";
			default:
				return "";
		}
	}
	#endregion
	/// <summary>
	/// cfg表的备注
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static void CreatFile(string path, StringBuilder sb)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		 Log.Info(path);
		File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
#endif
	}	
}
