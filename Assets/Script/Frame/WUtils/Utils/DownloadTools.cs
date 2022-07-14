using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WUtils.Utils
{
    public enum ResType
    {
        COM = 0,
        AB = 1,
        Texture = 2,
        Form,
    }
    public class DownloadTools 
    {
        private struct DownloadData
        {
            public string url;
            public readonly ResType resType;
            public Action<DownloadHandler> callback;
            public Action<string> ErrCb;

            public WWWForm Form;
            public readonly int lastTime;
           
            public DownloadData(int time, string _url, Action<DownloadHandler> cb, Action<string> Err, ResType type, WWWForm form)
            {
                lastTime = time;
                url = _url;
                callback = cb;
                ErrCb = Err;
                resType = type;
                Form = form;
            }
          
        }

        private static DownloadTools _instance;

        private Queue<DownloadData> _list;

        private bool loadIng;

        private static DownloadTools Inst
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DownloadTools();
                    _instance._list = new Queue<DownloadData>();
                }
                return _instance;
            }
        }
        private IEnumerator<float> LoadAction()
        {
            while (_list.Count > 0)
            {
                var temp = _list.Dequeue();
                UnityWebRequest w;
                switch (temp.resType)
                {
                    case ResType.Form:
                        w = UnityWebRequest.Post(temp.url, temp.Form);
                        break;
                    case ResType.AB:
                        w = UnityWebRequestAssetBundle.GetAssetBundle(temp.url);
                        break;
                    case ResType.Texture:
                        w = UnityWebRequestTexture.GetTexture(temp.url);
                        break;
                    default:
                        w = UnityWebRequest.Get(temp.url);
                        break;
                }

                using (w)
                {
                    w.timeout = temp.lastTime;
                    yield return Timing.WaitUntilDone(w.SendWebRequest());
                    if (w.isNetworkError || w.isHttpError)
                    {
                        //if (DebugMgr.CanLogError()) DebugMgr.LogError("Error URL=" + temp.url + "\n" + w.error);
                        if (temp.ErrCb == null)
                        {
                            continue;
                        }
                        temp.ErrCb(w.error);
                    }
                    else
                    {
                        temp.callback(w.downloadHandler);
                    }
                }
            }

            loadIng = false;
        }
       
        public static void LoadUrl(string url, int lastTime, Action<DownloadHandler> cb, Action<string> err = null, ResType type = ResType.COM, WWWForm form = null)
        {
            //if(DebugMgr.CanLogError()) DebugMgr.LogError("xxxxxxxxxxxxxxxxxxxxxx");
            Inst._list.Enqueue(new DownloadData(lastTime, url, cb, err, type, form));
            if (Inst.loadIng || Inst._list.Count == 0)
            {
                return;
            }
            Inst.loadIng = true;
            Timing.RunCoroutine(Inst.LoadAction());
        }
       
  

        private void OnDestroy()
        {
            _instance = null;
        }

        public void Dis()
        {
            
            loadIng = false;
        }

        public static void Dispose()
        {
            _instance.Dis();
        }

        private DownloadTools()
        {
        }
    }
}