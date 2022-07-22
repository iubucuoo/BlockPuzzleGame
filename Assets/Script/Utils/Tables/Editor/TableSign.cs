using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TableSign//:Editor
{
	//CFG表的标识符
	public const string USE = "[USE]";//加了这个的字段 数据 会被 技能编辑器 覆盖
	public const string Header = "[header]";
	public const string Data = "[data]";
	public const string CRLF = "\r\n";


	//Sign
	public const string L = "[l]";//要导出lua添加的标记
	public const string CS = "[#]";//要导出c#结构添加的标记

	//打表时 对导到 LUA 的字段进行标识
	public const string DMH = "[DMH]"; //单个冒号分割 到LUA会改成一个表
	public const string TBT = "[TBT]"; //十进制转二进制的 位置为1 按位所在的第几个数 加入表 现有gem skill 表 label字段用


}
