using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class TobeCfg : Editor
{
	/// <summary>
	/// 获取星球编辑器的原表数据
	/// </summary>
	/// <returns></returns>
	static void GetOriginalData(string _path, out Dictionary<int, Dictionary<string, string>> _NotUSEData, out Dictionary<string, string> _Header, out Dictionary<int, Dictionary<string, string>> _USEData)
	{
		_NotUSEData = new Dictionary<int, Dictionary<string, string>>();//使用原表数据的内容(没有use标记)
		_Header = new Dictionary<string, string>();//原表[header]下的字段内容
		_USEData = new Dictionary<int, Dictionary<string, string>>();
		string[] lines = File.ReadAllLines(_path);
		bool isContent = false;
		List<(int, string)> NotUSE = new List<(int, string)>();//需要使用原表数据 {没有用到[USE]标记的}
		List<(int, string)> USE = new List<(int, string)>();

		for (int i = 0; i < lines.Length; i++)
		{
			if (TableSign.Header.Equals(lines[i]))
				continue;
			if (TableSign.Data.Equals(lines[i]))
			{
				isContent = true;
				continue;
			}
			if (isContent != true)
			{
				string[] unit = lines[i].Split("\t".ToCharArray());
				if (unit.Length >= 3)
				{
					_Header.Add(unit[0], lines[i] + TableSign.CRLF);
					if (!unit[3].Contains(TableSign.USE))
						NotUSE.Add((i, unit[0]));
					else
						USE.Add((i, unit[0]));
				}
			}
			else
			{
				string[] unit = lines[i].Split("\t".ToCharArray());
				//这里新加字段没写数据的话会越界  这里保证不越界 处理默认部分放到后面写数据的时候
				Dictionary<string, string> lineData = new Dictionary<string, string>();
				for (int j = 0; j < NotUSE.Count; j++)
				{
					var _index = NotUSE[j].Item1 - 1;//第一行是[header] 所以-1
					if (_index < unit.Length)
					{
						lineData.Add(NotUSE[j].Item2, unit[_index]);
					}
				}
				Dictionary<string, string> Uselinedata = new Dictionary<string, string>();
				for (int j = 0; j < USE.Count; j++)
				{
					var _index = USE[j].Item1 - 1;//第一行是[header] 所以-1
					if (_index < unit.Length)
					{
						Uselinedata.Add(USE[j].Item2, unit[_index]);
					}
				}
				if (int.TryParse(unit[0], out int _id))
				{
					if (!_NotUSEData.ContainsKey(_id))
						_NotUSEData.Add(_id, lineData);
					if (!_USEData.ContainsKey(_id))
						_USEData.Add(_id, Uselinedata);
				}
				else
					Log.Error(string.Format("获取星球编辑器出现问题  {0} 无法转成int", unit[0]));
			}
		}
	}
	public static void SetHeader(string _path, StringBuilder sb, out Dictionary<int, Dictionary<string, string>> NotUseData, out Dictionary<string, string> _Header, out Dictionary<int, Dictionary<string, string>> UseData)
	{
		GetOriginalData(_path,
			out NotUseData,
			out _Header,
			out UseData);
		sb.Append(TableSign.Header).Append(TableSign.CRLF);
		foreach (var item in _Header)
		{
			sb.Append(item.Value);
		}
		sb.Append(TableSign.CRLF).Append(TableSign.Data).Append(TableSign.CRLF);
	}

	//无法确定当前字段的类型  通过header获取类型
	public static string GetDefault(KeyValuePair<string, string> tatle)
	{
		string[] unit = tatle.Value.Split("\t".ToCharArray());
		if (unit[1] == "float" || unit[1] == "int")
			return "0";
		else
			return "";
	}
}
