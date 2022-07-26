/*
               #########                       
              ############                     
              #############                    
             ##  ###########                   
            ###  ###### #####                  
            ### #######   ####                 
           ###  ########## ####                
          ####  ########### ####               
         ####   ###########  #####             
        #####   ### ########   #####           
       #####   ###   ########   ######         
      ######   ###  ###########   ######       
     ######   #### ##############  ######      
    #######  #####################  ######     
    #######  ######################  ######    
   #######  ###### #################  ######   
   #######  ###### ###### #########   ######   
   #######    ##  ######   ######     ######   
   #######        ######    #####     #####    
    ######        #####     #####     ####     
     #####        ####      #####     ###      
      #####       ###        ###      #        
        ###       ###        ###               
         ##       ###        ###               
__________#_______####_______####______________

                我们的未来没有BUG              
* ==============================================================================
* Filename: TestProtoBuf
* Created:  2017/12/17 21:05:01
* Author:   HaYaShi ToShiTaKa
* Purpose:  
* ==============================================================================
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

#region class
public class ProBufAPIFiled
{
	public readonly string type;
	public readonly string name;
	private readonly static HashSet<string> m_numStrHash = new HashSet<string>{
		"double",
		"float",
		"int32",
		"uin32",
		"uint32",
		"int64",
		"uint64",
		"sint32",
		"sing64",
		"fixed32",
		"fixed64",
		"sfixed32",
		"sfixed64",
	};
	public ProBufAPIFiled(string lineStr, string pkgName)
	{
		name = lineStr;

		List<string> strList = SplitLine(lineStr);
		if (strList.Count < 2 || strList.Count > 3)
		{
			DebugMgr.LogError("格式错误");
			DebugMgr.Log(lineStr);
			return;
		}

		bool isArray = false;
		if (strList.Count == 3)
			isArray = strList[0].Trim() == "repeated";

		string firstType;
		if (strList.Count == 2)
			firstType = strList[0].Trim();
		else
			firstType = strList[1].Trim();

		if (strList.Count == 2)
			name = strList[1];
		else
			name = strList[2];
		if (firstType == "string")
		{
			type = "string";
		}
		else if (firstType == "bool")
		{
			type = "boolean";
		}
		else if (m_numStrHash.Contains(firstType))
		{
			type = "number";
		}
		else
		{
			var types = firstType.Split('.');
			if (types.Length == 2)
			{
				type = firstType;
			}
			else
			{
				type = string.Format("{0}.{1}", pkgName, firstType);
			}
		}

		if (isArray)
		{
			type += "[]";
		}
	}

	private static List<string> SplitLine(string lineStr)
	{
		lineStr = lineStr.Trim();
		List<string> result = new List<string>();

		int lastIndex = 0;
		for (int i = 0, imax = lineStr.Length; i < imax; i++)
		{
			char c = lineStr[i];
			if (c == ' ')
			{
				string str = lineStr.Substring(lastIndex, i - lastIndex + 1);
				if (lastIndex != i)
				{
					result.Add(str);
				}
				lastIndex = i + 1;
			}
			else if (i == imax - 1)
			{
				string str = lineStr.Substring(lastIndex, i - lastIndex + 1);
				result.Add(str);
			}
		}

		return result;
	}
	public override string ToString()
	{
		return string.Format("---@field public {0} {1}", name, type);
	}
}

public class ProBufAPIEnum
{

	public readonly string className;
	public readonly string packageName;
	public readonly Dictionary<string, int> fieldList = new Dictionary<string, int>();
	public ProBufAPIEnum(string messageStr, string parent)
	{
		string[] fieldArray;
		EmmyProtoBufExport.GetClassInfo("enum", messageStr, parent, out packageName, out className, out fieldArray);
		for (int i = 0; i < fieldArray.Length - 1; i++)
		{
			var lineStr = fieldArray[i].Split('=');
			var key = lineStr[0].Trim();
			try
			{
				var number = System.Convert.ToInt32(lineStr[1].Trim());
				fieldList[key] = number;
			}
			catch (System.Exception ex)
			{
				DebugMgr.Log(ex.ToString());
			}
		}
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		string realM = packageName.Split('_')[0];
		sb.AppendLine(string.Format("---@class {0}.{1}", packageName, className));
		foreach (var item in fieldList)
		{
			sb.AppendLine(string.Format("{0}.{1} = {2}.{1}", packageName, item.Key, realM));
		}

		return sb.ToString();
	}
}
public class ProBufAPI
{
	public readonly string className;
	public readonly string packageName;
	public readonly List<ProBufAPIFiled> fieldList = new List<ProBufAPIFiled>();

	public ProBufAPI(string messageStr, string parent)
	{
		string[] fieldArray;
		EmmyProtoBufExport.GetClassInfo("message", messageStr, parent, out packageName, out className, out fieldArray);
		for (int i = 0; i < fieldArray.Length - 1; i++)
		{
			string lineStr = (fieldArray[i].Split('=')[0]).Trim();
			if (string.IsNullOrEmpty(lineStr))
			{
				continue;
			}
			ProBufAPIFiled pf = new ProBufAPIFiled(lineStr, packageName);
			fieldList.Add(pf);
		}
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine(string.Format("---@class {0}.{1}", packageName, className));
		foreach (var item in fieldList)
		{
			sb.AppendLine(item.ToString());
		}
		sb.AppendLine(string.Format("---@return {1}.{0}\nfunction {1}.{0}(data)", className, packageName));
		sb.AppendLine("   if data == nil then return {} else return data end");
		sb.AppendLine("end");
		return sb.ToString();
	}
}
#endregion

public class EmmyProtoBufExport
{
	public static void ExportApi(string inPath, string outPath, string name)
	{
		string proBufStr = File.ReadAllText(inPath, Encoding.UTF8);
		name += "_Msg";
		EmmyProtoBufExport export = new EmmyProtoBufExport(proBufStr, name);
		name += ".lua";
		var path = Path.Combine(outPath, name);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		FileStream fs = new FileStream(path, FileMode.Create);
		var utf8WithoutBom = new System.Text.UTF8Encoding(false);
		StreamWriter sw = new StreamWriter(fs, utf8WithoutBom);
		sw.Write(export.ToString());

		//清空缓冲区
		sw.Flush();
		sw.Close();
		fs.Close();
	}
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine(strFileName + " = {}");
		//string realM = strFileName.Split('_')[0];
		//if(realM == "pb")
		//    sb.AppendLine(realM + " = require 'Protol.ClientMsg_pb';");
		//else if(realM == "spb")
		//    sb.AppendLine(realM + " = require 'Protol.ServerMsg_pb';");
		for (int i = 0, imax = strList.Count; i < imax; i++)
		{
			var item = strList[i];
			sb.AppendLine(item.ToString());
		}
		return sb.ToString();
	}

	public static void GetClassInfo(string classType, string messageStr, string parent, out string packageName, out string className, out string[] fieldArray)
	{
		var value = TrimUseless(messageStr);
		className = Regex.Match(value, classType + @"([\s\S]*?)\{").Value.Replace(classType, "");
		className = className.Replace("{", "");
		className = className.Trim();
		//if (!string.IsNullOrEmpty(parent))
		//{
		//    className = parent + "." + className;
		//}


		packageName = parent;
		string fields = Regex.Replace(value, classType + @"([\s\S]*?)\{", "");
		fields = Regex.Replace(fields, @"([\s\S])\}", "").Trim();
		fieldArray = fields.Split(';');
	}

	#region private
	private readonly List<object> strList = new List<object>();
	private readonly string strPackageName;
	private readonly string strFileName;
	private readonly string strModuleName;
	private EmmyProtoBufExport(string pbStr, string fileName)
	{
		strFileName = fileName;
		pbStr = TrimUseless(pbStr, out strPackageName);
		//pbStr = Regex.Match(pbStr, @"(?:message)([\s\S])*\}").Value;
		strList.Clear();
		SplitMessage(pbStr, ref strList, strFileName);
	}
	private static string TrimUseless(string value, out string strPackageName)
	{
		var matches = Regex.Matches(value, @"package(?<pkg>.+);");
		strPackageName = matches[0].Groups["pkg"].Value.Trim();
		//文件声明
		value = Regex.Replace(value, "syntax.+", "");
		value = Regex.Replace(value, "package.+", "");
		value = Regex.Replace(value, "import.+", "");
		value = TrimUseless(value);
		return value;
	}

	public static string TrimUseless(string value)
	{
		//去注释
		value = Regex.Replace(value, "//.+", "");
		//替换制表符
		value = value.Replace("\t", " ");
		//去空白行
		value = Regex.Replace(value, @"\n[\s| ]*\r", "");
		return value;
	}
	private static void SplitMessage(string pbStr, ref List<object> result, string parent)
	{
		int leftTime = 0;
		int rightTime = 0;
		int lastIndex = 0;
		for (int i = 0, imax = pbStr.Length; i < imax; i++)
		{
			char c = pbStr[i];
			if (c == '{')
			{
				leftTime++;
			}
			if (c == '}')
			{
				rightTime++;
			}
			if (leftTime == 0) continue;
			if (rightTime == 0) continue;
			if (leftTime == rightTime)
			{
				string msgStr = pbStr.Substring(lastIndex, i - lastIndex + 1);
				if (msgStr.IndexOf("enum") >= 0)
				{
					//ProBufAPIEnum api = new ProBufAPIEnum(msgStr, parent);
					//result.Add(api);
				}
				else if (msgStr.IndexOf("message") >= 0)
				{

					if (leftTime > 1)
					{
						string p;
						string selfContent = null;
						msgStr = TrimMessage(msgStr, out p, out selfContent);
						if (!string.IsNullOrEmpty(parent))
						{
							p = parent + "." + p;
						}
						SplitMessage(msgStr, ref result, p);
						if (!string.IsNullOrEmpty(selfContent))
						{
							result.Add(new ProBufAPI(selfContent, parent));
						}
					}
					else
					{
						ProBufAPI api = new ProBufAPI(msgStr, parent);
						result.Add(api);
					}
				}

				lastIndex = i + 1;
				leftTime = 0;
				rightTime = 0;
			}
		}
	}
	private static string TrimMessage(string pbStr, out string parent, out string selfContent)
	{
		string result = "";
		parent = "";
		selfContent = null;
		string[] strList = pbStr.Split("{".ToCharArray(), 2);
		if (strList.Length == 2)
		{
			result = strList[1].TrimEnd('}').Trim();
			parent = (strList[0].Replace("message", "")).Trim();
		}
		else
		{
			result = pbStr;
		}
		string trimResult = Regex.Match(result, @"message([\s\S]*)\}").Value;
		if (trimResult != result)
		{
			selfContent = pbStr.Replace(trimResult, "");
			result = trimResult;
		}

		return result;
	}
	#endregion
}
