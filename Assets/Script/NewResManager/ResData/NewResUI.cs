using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChapterUnit
{
	public ChapterUnit() { _Abs = new List<NewResAb>(); }
	public int _ID;
	public int _MAX { get; private set; }
	public int _Current
	{
		get
		{
			int cur = 0;
			foreach (var item in _Abs)
			{
				cur += item._curSize;
			}
			return cur;
		}
	}
	public List<NewResAb> _Abs;
	public void Add(NewResAb ab)
	{
		_Abs.Add(ab);
		_MAX += ab._Size;
	}
}

public class Chapter
{
	public Chapter() { _Data = new List<ChapterUnit>(); }
	public int _ChapterID;//章节ID
	public List<ChapterUnit> _Data;//每一条数据
	public int _MAX { get; private set; }

	public int _Current
	{
		get
		{
			int cur = 0;
			foreach (var item in _Data)
			{
				cur += item._Current;
			}
			return cur;
		}
	}
	public bool IsDownOver => _Current >= _MAX;
	internal void Add(NewResAb ab)
	{
		ChapterUnit _temp = null;
		for (int i = 0; i < _Data.Count; i++)
		{
			_temp = _Data[i];
			if (ab._DownloadID == _temp._ID)
			{
				_temp.Add(ab);
				_MAX += ab._Size;
				return;
			}
		}
		_temp = new ChapterUnit()
		{
			_ID = ab._DownloadID,
		};

		_temp.Add(ab);
		_Data.Add(_temp);
		_MAX += ab._Size;
	}
}

public class NewResUI : IMgr
{
	public Dictionary<int, Chapter> _Chapters = new Dictionary<int, Chapter>();
	public static NewResUI _inst;
	static int max = 0;
	public static int MaxChapter
	{
		get
		{
			if (max == 0)
			{
				max = 0;
				return max;
			}
			else
				return max;
		}
	}
	public static Chapter GetChapter(int id)
	{
		if (!_inst._Chapters.TryGetValue(id, out Chapter v))
		{
			v = new Chapter()
			{
				_ChapterID = id,
			};
			
			_inst._Chapters.Add(id, v);
			return v;
		}
		else
		{
			return _inst._Chapters[id];
		}
	}

	public void InitMgr()
	{
		_inst = this;
	}

	public void ResetMgr()
	{
		_Chapters.Clear();
	}
	public static void DownloadChapter(int _ChapterID)
	{
		ResCenter.inst.DownloadChapter(_ChapterID);
	}
	public static void ClearChapterData(int _ChapterID)
	{
		if (_inst._Chapters.TryGetValue(_ChapterID, out Chapter v))
		{
			ResCenter.inst.ClearChapterData(v);
		}
	}
	/// <summary>
	/// 该地图资源名所在章节是否下载完所有的资源
	/// </summary>
	/// <param name="resname"></param>
	/// <returns></returns>
	public static bool IsDownload(int mapid)
	{

		//var res_down = ResDownloadManager.GetSingleData(map.detour);
		//if (res_down == null)
		//{
		//	return true;//res_download没有该资源名 不用下载
		//}
		int chapterid = 100;
		var chapters = GetChapter(chapterid);
		if (chapters.IsDownOver)
		{
			return true;//该章节下载完了
		}
		else
		{
			return false;//该章节还没有下载完成
		}
	}
}
