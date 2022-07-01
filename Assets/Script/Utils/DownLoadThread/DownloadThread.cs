using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
public enum FileRoot : byte
{
    dll = 0,
    lua = 1,
    datainfo,
    sound,
    windowinfo,
    mapinfo,
    scenes,
    tables,
    firstinfo,

    minimap,
    unitinfo,
    monsterinfo,
    npc,
    otherinfo,
    effectinfo,
    plantinfo,
    trapinfo,
    mapsdata,
    none,
}
enum GameStage : byte
{
    Foreground,
    Background,
}
public class DownloadThread
{
    object downObj;
    Queue<ResData> _UIDownFiles = new Queue<ResData>();
    Queue<ResData> _SmallMapDownFiles = new Queue<ResData>();
    Queue<ResData> _MapDownFiles = new Queue<ResData>();
    Queue<ResData> _ModelDownFiles = new Queue<ResData>();
    Queue<ResData> _EffectDownFiles = new Queue<ResData>();
    Queue<ResData> _MapDatasDownFiles = new Queue<ResData>();

    Queue<ResData> _MapDatasLoadFiles = new Queue<ResData>();
    Queue<ResData> _UILoadFiles = new Queue<ResData>();
    Queue<ResData> _SmallMapLoadFiles = new Queue<ResData>();
    Queue<ResData> _MapLoadFiles = new Queue<ResData>();
    Queue<ResData> _ModelLoadFiles = new Queue<ResData>();
    Queue<ResData> _EffectLoadFiles = new Queue<ResData>();

    //public ArtRes LineRes;
    GameStage _Stage;
    public bool isStop=false;
    public Thread downloadThread;
    public void InitRes(bool IsBackground)
    {
        _Stage = IsBackground ? GameStage.Background : GameStage.Foreground;
        lock (downObj)
        {
            speed = IsBackground ? 1024 : 2048;
            nbytes = new byte[speed];
            //if (IsBackground && LineRes == null)
            //{
            //    LineRes = new ArtRes();
            //}
        }        
    }
    public DownloadThread()
    {
        ServicePointManager.DefaultConnectionLimit = 512;//设置下载的链接数
        ServicePointManager.Expect100Continue = false;
        downObj = new object();
        downloadThread = new Thread(new ThreadStart(DownFile));
        downloadThread.Priority = System.Threading.ThreadPriority.Lowest;
        downloadThread.Start();
    }    
    
    ResData GetDataDown()
    {
        if (_MapDatasDownFiles.Count > 0)
        {
            lock (downObj)
            {
                var res = _MapDatasDownFiles.Dequeue();
#if LHLOG
                Debug.LogError("_MapDatasDownFiles ======" + res.server_url);
#endif
                return res;
            }

        }
        if (_UIDownFiles.Count > 0)
        {
            lock (downObj)
            {
                var res = _UIDownFiles.Dequeue();
#if LHLOG
                Debug.LogError("_UIDownFiles ======" + res.server_url );
#endif
                return res;
            }            
        }

        if (_SmallMapDownFiles.Count>0)
        {
            lock (downObj)
            {
                var res = _SmallMapDownFiles.Dequeue();
#if LHLOG
                Debug.LogError("_SmallMapDownFiles ======" + res.server_url);
#endif
                return res;
            }
                
        }
        if (_MapDownFiles.Count>0)
        {
            lock (downObj)
            {
                var res = _MapDownFiles.Dequeue();
#if LHLOG
                Debug.LogError("_MapDownFiles ======" + res.server_url);
#endif
                return res;
            }
                
        }
        if (_ModelDownFiles.Count>0)
        {
            lock (downObj)
            {
                var res = _ModelDownFiles.Dequeue();
#if LHLOG
                Debug.LogError("_ModelDownFiles ======" + res.server_url);
#endif
                return res;
            }
                
        }
        if (_EffectDownFiles.Count>0)
        {
            lock (downObj)
            {
                var res = _EffectDownFiles.Dequeue();
#if LHLOG
                Debug.LogError("_EffectDownFiles ======" + res.server_url);
#endif
                return res;
            }
                
        }
//        if (_Stage == GameStage.Background&& LineRes!=null)
//        {
//            lock (downObj)
//            {
//                var res = LineRes.GetNeeds();
//#if LHLOG
//                Debug.LogError("LineRes ======" + res.server_url);
//#endif
//                return res;
//            }
               
//        }
        return null;
    }        
    public void Clear()
    {
        _SmallMapLoadFiles.Clear();
        _UILoadFiles.Clear();
        _MapLoadFiles.Clear();
        _MapDatasLoadFiles.Clear();
        _ModelLoadFiles.Clear();
        _EffectLoadFiles.Clear();
        _UIDownFiles.Clear();
        _SmallMapDownFiles.Clear();
        _MapDatasDownFiles.Clear();
        _MapDownFiles.Clear();
        _ModelDownFiles.Clear();
        _EffectDownFiles.Clear();
    }
    public ResData GetDataLoad()
    {
        if (_SmallMapLoadFiles.Count > 0)
        {           
            return _SmallMapLoadFiles.Dequeue();
        }
        if (_MapDatasLoadFiles.Count>0)
        {            
            return _MapDatasLoadFiles.Dequeue();
        }       
        if (_UILoadFiles.Count > 0)
        {
            return _UILoadFiles.Dequeue();
        }        
        if (_MapLoadFiles.Count > 0)
        {            
            return _MapLoadFiles.Dequeue();
        }
        if (_ModelLoadFiles.Count > 0)
        {
            return _ModelLoadFiles.Dequeue();
        }
        if (_EffectLoadFiles.Count > 0)
        {
            return _EffectLoadFiles.Dequeue();
        }
        return null;
    }
    public void DownQueue(ResData data)
    {
        data.status = ResStatus.Downing;
        //这样可以在主线程初始化_url,减少多线程问题       
        switch (data.root)
        {            
            case FileRoot.mapinfo:
                lock (downObj)
                {
                    //var w = data.server_url;
                    _MapDownFiles.Enqueue(data);
                }                    
                break;
            case FileRoot.minimap:
                lock (downObj)
                {
                    //var w = data.server_url;
                    _SmallMapDownFiles.Enqueue(data);
                }
                break;
            case FileRoot.mapsdata:
                _MapDatasDownFiles.Enqueue(data);
                break;
            case FileRoot.unitinfo:
            case FileRoot.monsterinfo:
            case FileRoot.npc:
            case FileRoot.otherinfo:
            case FileRoot.plantinfo:
            case FileRoot.trapinfo:
                lock (downObj)
                {
                    //var w = data.server_url;
                    _ModelDownFiles.Enqueue(data);
                }
                break;
            case FileRoot.effectinfo:                
            case FileRoot.sound://声音不需要优先下载，但需要优先加载
                lock (downObj)
                {
                    //var w = data.server_url;
                    _EffectDownFiles.Enqueue(data);
                }
                break;
            default:
                lock (downObj)
                {
                    //var w = data.server_url;
                    _UIDownFiles.Enqueue(data);
                }
                break;
        }
    }

    public void LoadQueue(ResData data)
    {
        data.status = ResStatus.Loading;
        switch (data.root)
        {
            case FileRoot.mapinfo:
#if LHLOG
                Debug.LogError("_MapLoadFiles   " + data.loadurl);
#endif
                _MapLoadFiles.Enqueue(data);
                break;
            case FileRoot.minimap:
#if LHLOG
                Debug.LogError("_SmallMapLoadFiles   " + data.loadurl);
#endif
                _SmallMapLoadFiles.Enqueue(data);
                break;            
            case FileRoot.mapsdata:
#if LHLOG
                Debug.LogError("_MapDatasLoadFiles   " + data.loadurl);
#endif
                _MapDatasLoadFiles.Enqueue(data);
                break;
            case FileRoot.unitinfo:
            case FileRoot.monsterinfo:
            case FileRoot.npc:
            case FileRoot.otherinfo:
            case FileRoot.plantinfo:
            case FileRoot.trapinfo:
#if LHLOG
                Debug.LogError("_ModelLoadFiles   " + data.loadurl);
#endif
                _ModelLoadFiles.Enqueue(data);
                break;
            case FileRoot.effectinfo:
#if LHLOG
                Debug.LogError("_EffectLoadFiles   " + data.loadurl);
#endif
                _EffectLoadFiles.Enqueue(data);
                break;
            default:
#if LHLOG
                Debug.LogError("_UILoadFiles   " + data.loadurl);
#endif
                _UILoadFiles.Enqueue(data);
                break;
        }
    }

    public Action<string> ForegroundErrorOver;
   
    void DownFile()
    {
        while (true)
        {
            try
            {
                if (!isStop)//是不是自动关闭下载线程
                {
                    if (StaticTools._NetStatus == 0)//当前网络如何,0表示没有网络
                    {
                        if (_Stage==GameStage.Foreground)//如果是在前端下载中断,后台没有网络不做处理
                        {
                            ForegroundErrorOver(null);
                        }
                    }else
                    {
                        
                        Downloading();
                    }
                }
            }            
            catch (Exception e)
            {
                var ee = e.ToString();                
                if (ee.IndexOf("Disk full") >= 0||ee.IndexOf("ERROR_GEN_FAILURE")>=0)
                {
                    isStop = true;
                    ForegroundErrorOver("磁盘空间不足，请清理一下磁盘");
                    Debug.LogWarning("下载线程BUG:" + ee);
                }
                else
                {
                    Debug.LogError("下载线程BUG:" + ee);
                }
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
    

    void Call(ResData resdata)
    {
        //IArt artbase = null;
        //if (resdata.objNode != null)
        //{
        //    artbase = resdata.objNode.Value as IArt;
        //    if (artbase != null && (artbase.IsWaitArt(resdata.key) == false)&&!(artbase is MovieData))
        //    {
        //        //一般资源需要过滤掉，除了特效资源例外，因为这个资源占用时间短容易不容易资源很多的情况不容易下载
        //        resdata.ClearNode();
        //        return;
        //    }            
        //}

        try
        {

            _FileTotalLen = GetLength(resdata.server_url);
            req = WebRequest.Create(resdata.server_url) as HttpWebRequest;
            req.Method = WebRequestMethods.Http.Get;
            req.Timeout = 15000;
            req.ReadWriteTimeout = 10000;
            req.KeepAlive = false;
            req.Proxy = null;            
            req.AllowWriteStreamBuffering = false;            

            string localUrl = resdata.persis_url;
#if UNITY_ANDROID || UNITY_EDITOR
            if (resdata.root == FileRoot.dll)
            {
                localUrl += ".bak";
            }
#endif
            string localdir = resdata.persis_dir;//本地目录

            if (!Directory.Exists(localdir))
            {
                Directory.CreateDirectory(localdir);
            }

            if (File.Exists(localUrl))
            {
                fs = File.OpenWrite(localUrl);
                _FileCurLen = fs.Length;
                //Debug.LogWarning(" fileExists  serverurl=" + resdata.server_url  + ",  _FileCurLen " + _FileCurLen + " _FileTotalLen  " + _FileTotalLen);
                if (_FileTotalLen == _FileCurLen)
                {
                    ResetHttp();
                    MarkFinish(resdata);                    
                    return;
                    //这种情况估计是版本没有保存，但本地资源是正确的，因为前面已经md5比对过了
                }else if(_FileTotalLen<_FileCurLen||resdata.IsLog==-1)
                {
                    if (fs != null)
                    {
                        fs.Flush();
                        fs.Close();
                        fs = null;
                    }
                    File.Delete(localUrl);
                    fs = new FileStream(localUrl, FileMode.CreateNew);
                    _FileCurLen = 0;
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
            resdata.IsLog = resdata.IsLog ==-1 ?(sbyte) 0 : resdata.IsLog;
            var _bytes = nbytes;
#if LHLOG
            Debug.LogWarning(" request  serverurl=" + resdata.server_url);
#endif
            //向服务器请求，获得服务器回应数据流
            using (stream = rsp.GetResponseStream())
            {
				//如果下载已经超出，就停止下载，减少无用下载
                while ((!isStop)&& _FileCurLen<_FileTotalLen && (nReadSize = stream.Read(_bytes, 0, speed)) > 0)//1.限速了、2,错误率
                {
                    fs.Write(_bytes, 0, nReadSize);                   
                    _FileCurLen += nReadSize;
                    //Debug.LogWarning("serverurl=" + resdata.server_url + "  _FileCurLen =" + _FileCurLen);
                    //ShowFPS.UpSpeed(nReadSize);
                    Process(nReadSize, 0);
                    if (_Stage == GameStage.Background)
                    {
                        //Thread.Sleep((int)(WifiNet.sleepTime));
                    }
                }
            }

            var temp_num = _FileCurLen - _FileTotalLen;

            ResetHttp();
            if (temp_num!=0)
            {                
                if (temp_num > 0)
                {
                    File.Delete(localUrl);
                    Debug.LogWarning(localUrl + "||||" + resdata.server_url + " 异常,重新下载这个文件:::" + _FileCurLen + "/" + _FileTotalLen);
                }else
                {
                    Debug.LogWarning("meiyou xia zai wang:" + resdata.server_url);
                }
                DownQueue(resdata);
                return;
            }
            else
            {
#if LHLOG
                Debug.LogWarning("request  serverurl=" + resdata.server_url  + "   download finish");
#endif
                MarkFinish(resdata);
            }
        }
        catch (WebException serverEx)
        {
            if (serverEx.Status == WebExceptionStatus.Timeout)
            {
                Debug.LogWarning("GetResponse Timeout！" + serverEx.Message + "  " + resdata.server_url);
            }
            else
            {
                Debug.LogWarning("GetResponse Exception: " + serverEx.Message + "  " + resdata.server_url);
            }
            Error(resdata);
        }
        catch (IOException ioEx)
        {
            Debug.LogWarning("IO Exception: " + ioEx.Message + "  " + resdata.server_url);
            Error(resdata);
        }
        finally
        {            
            ResetHttp();
        }
    }
    long GetLength(string url)
    {
        HttpWebRequest requet = WebRequest.Create(url) as HttpWebRequest;
        requet.Method = "HEAD";
        HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
        var len=response.ContentLength;
        requet.Abort();
        requet = null;
        response.Close();
        response = null;
        return len;
    }
    void Error(ResData resdata)
    {
        if (_Stage == GameStage.Foreground)//如果是在前端下载中断,后台没有网络不做处理
        {
            isStop = true;//停止下载
            ForegroundErrorOver(null);
        }
        else
        {
            ResetHttp();
            Debug.LogWarning("Error重置下载");
            DownQueue(resdata);
            //Call(resdata);//异常之后重新下载
        }
    }
    void Downloading()
    {
        var resdata = GetDataDown();
        if (resdata==null)
        {
            return;
        }
        if (resdata.isFinish)
        {
            MarkFinish(resdata);//下载完成
            return;
        }                
        Call(resdata);
    }
    public Action<float,float> cb;
    void Process(int x, int max)
    {
        if (cb != null)
        {
            cb(x, max);
        }

    }
    void MarkFinish(ResData data)
    {
        //该加载则加载，该返回则返回，标记filish
        if (data.Finish())
        {
            if (data.status != ResStatus.Loading)
            {
                LoadQueue(data);
            }
        }
        else if(_Stage==GameStage.Foreground)//只有在登录前需要未加载返回的流程
        {
            data.FeedBack();
        }else if(data.status != ResStatus.Loading)
        {
            //可能从下载的池中出来的单位是不需要加载的，导致ClearNode，如果已经有需要加载的资源在loadQueue中，就会出现问题，导致黑屏，添加这个判断data.status != ResStatus.Loading，防止黑屏
            data.ClearNode();
        }         
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