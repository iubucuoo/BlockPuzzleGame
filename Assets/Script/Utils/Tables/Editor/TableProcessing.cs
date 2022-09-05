using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;


public class TableProcessing : Editor
{
	static readonly string doc_root = Application.dataPath + "/Scripts/Utils/Tables/Data/";// Application.dataPath + "/Scripts/Utils/Tables/Data/";

    /// <summary>
    /// 修改表 某个key的某个字段
    /// </summary>
    /// <param name="cfgname">表名</param>
    /// <param name="changekey">字段名</param>
    /// <param name="tableid"></param>
    /// <param name="changevalue"></param>
    public static void ChangeTable(string cfgname,string changekey, int tableid,float changevalue)
    {
        string fmpathstr = string.Format(@"{0}\StarEdit\StarEdit\bin\Debug\data\{1}.cfg", PlayerPrefs.GetString("星球编辑器url", ""), cfgname);
        
        if (!File.Exists(fmpathstr))
        {
            Log.Error("文件不存在:" + fmpathstr);
            return ;
        }
        StringBuilder sb = new StringBuilder();
        string[] lines = File.ReadAllLines(fmpathstr);
        bool isContent = false;
        int radiusid = 0;
        bool ischange = false;
        for (int i = 0; i < lines.Length; i++)
        {
            if (TableSign.Header.Equals(lines[i]))
            {
                sb.AppendFormat("{0}\n", lines[i]);
            }
            else if (TableSign.Data.Equals(lines[i]))
            {
                isContent = true;
                sb.AppendFormat("{0}\n", lines[i]);
            }
            else if (isContent != true)
            {
                if (lines[i].Split("\t".ToCharArray())[0] == changekey)
                {
                    radiusid = i;
                }
                sb.AppendFormat("{0}\n", lines[i]);
            }
            else
            {
                string[] unit = lines[i].Split("\t".ToCharArray());
                if (unit[0] == tableid.ToString()&& radiusid!=0)
                {
                    unit[radiusid-1] = changevalue.ToString();
                    ischange = true;
                }
                for (int j = 0; j < unit.Length; j++)
                {
                   if( j != unit.Length - 1)
                    sb.AppendFormat("{0}\t", unit[j]);
                   else
                    sb.AppendFormat("{0}\n", unit[j]);
                }
            }
        }
        if (ischange)
        {
            Log.Info(string.Format( "修改成功，{0}表的ID: {1} 的{2}字段值改为 {3} ", cfgname,tableid, changekey, changevalue));
            TableStrTool.CreatFile(fmpathstr, sb);
        }
        else
            Log.Error(string.Format("修改失败，是否{0}表的ID:{1} 或者{2}字段 不存在", cfgname, tableid, changekey));
    }

    public static void CfgCreateTLC(string pathstr)
    {
        string fmpathstr = string.Format(@"{0}\StarEdit\StarEdit\bin\Debug\data\", pathstr);
        if (!Directory.Exists(fmpathstr))
        {
            Log.Error("路径不存在" + fmpathstr);
            return;
        }
        TableClear();
        Check_Cfg(fmpathstr);
        Auto_table(doc_root);
    }
    public static void WindowSelectCfgtoTLC( string[] FileNames)
    {
		TableClear();
        Check_CfgWin(FileNames);
        Auto_table(doc_root);
    }
    /// <summary>
    /// 打表开始的时候清理数据
    /// </summary>
    public static void TableClear()
    {
        Type baseType = typeof(TableBaseManager);
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (baseType.IsAssignableFrom(type) && baseType != type)
                {
                    TableBaseManager tableInstance = Activator.CreateInstance(type) as TableBaseManager;
                    if (tableInstance.Open)
                    {
						tableInstance.ThisClear();
                        //var tableInfo = type.GetMethod("Clear");
                        //tableInfo.Invoke(tableInstance, null);
                    }
                }
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// txt->lua&cs
    /// </summary>
    /// <param name="path">doc_root</param>
    /// <param name="luaremove"></param>
    /// <param name="csremove"></param>
    static void Auto_table(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (dir.Exists)
        {
			string[] files = Directory.GetFiles(dir.FullName);
			foreach (var info in files)
			{
				if (info.EndsWith(".txt"))
				{
					TextAsset data = ObjectMgr.LoadMainAssetAtPath(info.Substring(info.IndexOf("Assets"))) as TextAsset;
					string[] lines = data.text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					if (TableSign.Header.Equals(lines[0]))
					{
						string fileName = info.Substring(info.LastIndexOf("\\") + 1);
						Log.Info("表处理__" + fileName.Replace(".txt", ""));
						TobeCS.CreatCS(lines, fileName);
						//cs脚本处理
						TobeLua.Creat_lua(lines, fileName);
						TobeTxt.CreatTxt(info, lines);
					}
				}
			}
		}
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// cfg->txt
    /// </summary>
    /// <param name="path"></param>
    static void Check_Cfg(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
		if (!dir.Exists)
		{
			Log.Error("路径不存在" + path);
			return;
		}
		string[] FileNames = Directory.GetFiles(dir.FullName);
		Check_CfgWin(FileNames);
    }

    static void Check_CfgWin(string[] FileNames)
    {
		foreach (var file in FileNames)
		{
			if (file.EndsWith(".cfg"))
			{
				StreamWriter(file);
			}
		}
        AssetDatabase.Refresh();
    }

	/// <summary>
	/// 提出关键字  转txt
	/// </summary>
	/// <param name="filename"></param>
    static void StreamWriter(string filename)
    {
        string path_txt = doc_root + filename.Substring(filename.LastIndexOf("\\") + 1).Replace(".cfg", ".txt");
        if (File.Exists(path_txt))
        {
            File.Delete(path_txt);
        }
		using (StreamReader sr = new StreamReader(filename, Encoding.UTF8, false))
        {
            var tmp = TableStrTool.Shielding(sr.ReadToEnd());
            if (tmp.Contains(TableSign.CS) || tmp.Contains(TableSign.L))
            {
                using (StreamWriter sw = new StreamWriter(path_txt, false, Encoding.UTF8))
                {
                    sw.Write(tmp);
                }
            }
        }
    }
	//生成cfg后打txt
	public static void CreatCfgtoTCL(string path, StringBuilder strb)
	{
		TableStrTool.CreatFile(path, strb);
		WindowSelectCfgtoTLC(new string[]{ path });
	}

}
