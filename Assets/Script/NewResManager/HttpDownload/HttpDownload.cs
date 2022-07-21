using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
public enum GameStage : byte
{
	Foreground,
	Background,
}
public class DownloadThread
{
	object downObj;
	List<NewResAb> _UIDownFiles = new List<NewResAb>();
	GameStage _Stage;
	public bool isStop = false;
	public Thread downloadThread;
	public bool _Idle
	{
		get
		{
			lock (downObj)
				return _ThreadIdle && _UIDownFiles.Count == 0;
		}
	}
	public bool _ThreadIdle { get; private set; }
	public DownloadThread()
	{
		ServicePointManager.DefaultConnectionLimit = 512;//设置下载的链接数
		ServicePointManager.Expect100Continue = false;
		downObj = new object();
		InitRes(false);
		downloadThread = new Thread(new ThreadStart(DownFile))
		{
			Priority = System.Threading.ThreadPriority.Lowest
		};
		downloadThread.Start();
	}
	public void InitRes(bool IsBackground)
	{
		_Stage = IsBackground ? GameStage.Background : GameStage.Foreground;
		lock (downObj)
		{
			speed = IsBackground ? 1024 : 4096;
			nbytes = new byte[speed];
		}
	}

	NewResAb GetDataDown()
	{
		if (_UIDownFiles.Count > 0)
		{
			lock (downObj)
			{
				var v = _UIDownFiles[0];
				_UIDownFiles.Remove(v);
				_ThreadIdle = false;//需要在lock里面 不然会有问题
				return v;
			}
		}
		_ThreadIdle = true;
		return null;
	}
	public void Clear()
	{
		_UIDownFiles.Clear();
	}

	public void DownQueue(NewResAb data)
	{
		//data._LoadStatus = LoadStatus.Downloading;
		//这样可以在主线程初始化_url,减少多线程问题      
		if (data._IsFinish)
			return;
		_UIDownFiles.Add(data);
		isStop = false;
	}
	public void ClearChapterData(List<ChapterUnit> _list)
	{
		isStop = true;
		_UIDownFiles.Clear();
		//for (int i = 0; i < _list.Count; i++)
		//{
		//	var id = _list[i]._ID;
		//	for (int j = _UIDownFiles.Count - 1; j >= 0; j--)
		//	{
		//		var downUnit = _UIDownFiles[j];
		//		if (downUnit._DownloadID == id)
		//		{
		//			_UIDownFiles.Remove(downUnit);
		//		}
		//	}
		//}		
	}


	void DownFile()
	{
		while (true)
		{
			try
			{
				if (!isStop)//是不是自动关闭下载线程
				{
					Downloading();
				}
			}
			catch (Exception e)
			{
				var ee = e.ToString();
				if (ee.IndexOf("Disk full") >= 0 || ee.IndexOf("ERROR_GEN_FAILURE") >= 0)
				{
					isStop = true;
				}
				DebugMgr.LogWarning("下载线程BUG:" + ee);
			}

			Thread.Sleep(20);
		}
	}


	HttpWebRequest req;
	HttpWebResponse rsp;
	long _FileTotalLen;
	long _FileCurLen;
	FileStream fs = null;
	Stream stream;
	int nReadSize = 0;
	byte[] nbytes = new byte[2048];
	public int speed = 1024;


	void Call(NewResAb resdata)
	{
		try
		{
			var _NetUrl = resdata._NetUrl;
			_FileTotalLen = GetLength(_NetUrl);
			req = WebRequest.Create(_NetUrl) as HttpWebRequest;
			req.Method = WebRequestMethods.Http.Get;
			req.Timeout = 15000;
			req.ReadWriteTimeout = 10000;
			req.KeepAlive = false;
			req.Proxy = null;
			req.AllowWriteStreamBuffering = false;

			string localUrl = resdata.GetPathBak();

			string localdir = resdata.GetDir();//本地目录

			if (!Directory.Exists(localdir))
			{
				Directory.CreateDirectory(localdir);
			}

			if (File.Exists(localUrl))
			{
				fs = File.OpenWrite(localUrl);
				_FileCurLen = fs.Length;
				if (_FileTotalLen == _FileCurLen)
				{
					ResetHttp();
					MarkFinish(resdata);
					return;
					//这种情况估计是版本没有保存，但本地资源是正确的，因为前面已经md5比对过了
				}
				else if (_FileTotalLen < _FileCurLen)
				{
					if (fs != null)
					{
						fs.Flush();
						fs.Close();
						fs = null;
					}
					File.Delete(localUrl);
					DownQueue(resdata);
					return;
				}
				else
				{
					fs.Seek(_FileCurLen, SeekOrigin.Current);//移动文件流中的当前指针
					req.AddRange((int)_FileCurLen);
				}
			}
			else
			{
				fs = new FileStream(localUrl, FileMode.CreateNew);
				_FileCurLen = 0;
			}
			rsp = req.GetResponse() as HttpWebResponse;
			var _bytes = nbytes;
			//向服务器请求，获得服务器回应数据流
			using (stream = rsp.GetResponseStream())
			{
				//如果下载已经超出，就停止下载，减少无用下载
				while ((!isStop) && _FileCurLen < _FileTotalLen && (nReadSize = stream.Read(_bytes, 0, speed)) > 0)//1.限速了、2,错误率
				{
					fs.Write(_bytes, 0, nReadSize);
					_FileCurLen += nReadSize;
					resdata._CurSize = (int)_FileCurLen;
					Process(nReadSize, 0, resdata);
					if (_Stage == GameStage.Background)
					{
						Thread.Sleep(10);
					}
				}
			}

			var temp_num = _FileCurLen - _FileTotalLen;
			resdata._CurSize = (int)_FileCurLen;
			ResetHttp();
			if (temp_num != 0)
			{
				if (temp_num > 0)
				{
					File.Delete(localUrl);
					DebugMgr.LogWarning(localUrl + "||||" + resdata._NetUrl + " 异常,重新下载这个文件:::" + _FileCurLen + "/" + _FileTotalLen);
				}
				else
				{
					DebugMgr.LogWarning("meiyou xia zai wang:" + resdata._NetUrl);
				}
				DownQueue(resdata);
				return;
			}
			else
			{
				MarkFinish(resdata);
			}
		}
		catch (WebException serverEx)
		{
			if (serverEx.Status == WebExceptionStatus.Timeout)
			{
				DebugMgr.LogError("GetResponse Timeout！" + serverEx.Message + "  " + resdata._NetUrl);
			}
			else
			{
				DebugMgr.LogError("GetResponse Exception: " + serverEx.Message + "  " + resdata._NetUrl);
			}
			Error(resdata);
		}
		catch (IOException ioEx)
		{
			DebugMgr.LogError("IO Exception: " + ioEx.Message + "  " + resdata._NetUrl);
			Error(resdata);
		}
		finally
		{
			ResetHttp();
		}
	}
	long GetLength(string url)
	{
		HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
		requet.Method = "HEAD";
		HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
		var len = response.ContentLength;
		requet.Abort();
		requet = null;
		response.Close();
		response = null;
		return len;
	}
	void Error(NewResAb resdata)
	{
		if (_Stage == GameStage.Foreground)//如果是在前端下载中断,后台没有网络不做处理
		{
			isStop = true;//停止下载
						  //ForegroundErrorOver(null);
		}
		else
		{
			ResetHttp();
			DebugMgr.LogWarning("Error重置下载");
			DownQueue(resdata);
			//Call(resdata);//异常之后重新下载
		}
	}
	void Downloading()
	{
		var resdata = GetDataDown();
		if (resdata == null)
		{
			return;
		}
		if (resdata._IsFinish)
		{
			MarkFinish(resdata);//下载完成
			return;
		}
		Call(resdata);
	}
	public Action<int> _Process;
	public Action<NewResAb> ResDownCB;//后续可能会加条件
	void Process(int x, int max, NewResAb resdata)
	{
		_Process?.Invoke(x);
	}
	void MarkFinish(NewResAb data)
	{
		data._IsFinish = true;
		data.Move();
		ResDownCB?.Invoke(data);
	}
	public void ResetHttp()
	{
		if (fs != null)
		{
			fs.Flush();
			fs.Close();
			fs = null;
		}
		if (stream != null)
		{
			stream.Close();
			stream = null;
		}
		if (rsp != null)
		{
			rsp.Close();
			rsp = null;
		}
		if (req != null)
		{
			req.Abort();
			req.KeepAlive = false;
			req = null;
		}

	}
}
