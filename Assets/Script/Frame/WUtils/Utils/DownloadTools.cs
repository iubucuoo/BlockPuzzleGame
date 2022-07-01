using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WUtils.Utils
{
    public class DownloadTools : MonoBehaviour
    {
        private struct DownloadData
        {
            public string path;

            public GetData dataFunc;

            public ErrorBack error;

            public WWWForm _form;

            public float time;

            public DownloadData(string tmpPath, GetData dataFunc, ErrorBack error, WWWForm _form, float _time)
            {
                this = default(DownloadData);
                path = tmpPath;
                this.dataFunc = dataFunc;
                this.error = error;
                this._form = _form;
                time = _time;
            }
        }

        public delegate void GetData(WWW www);

        public delegate void ErrorBack(string name);

        private static DownloadTools _instance;

        private Queue<DownloadData> queue;

        private bool isDownloading;

        private static DownloadTools Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gameObject = GameObject.Find("MainScripts");
                    if (gameObject == null)
                    {
                        gameObject = new GameObject("MainScripts");
                    }
                    _instance = gameObject.GetOrCreatComponent<DownloadTools>();
                    _instance.queue = new Queue<DownloadData>();
                }
                return _instance;
            }
        }

        private IEnumerator DownloadFile()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    DownloadData _Data = queue.Dequeue();
                    WWW www = (_Data._form != null) ? new WWW(_Data.path, _Data._form) : new WWW(_Data.path);
                    float tmp = 0f;
                    while (tmp < _Data.time && !www.isDone)
                    {
                        tmp += 0.033f;
                        yield return (object)null;
                    }
                    string error = www.error;
                    if (www.isDone && string.IsNullOrEmpty(error))
                    {
                        _Data.dataFunc(www);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(error))
                        {
                            Debug.LogError(_Data.path + "请求超时：" + _Data.time);
                        }
                        else
                        {
                            Debug.LogError(_Data.path + "\t" + error);
                        }
                        ErrorBack error2 = _Data.error;
                        error2?.Invoke(_Data.path);
                    }
                }
                yield return (object)null;
            }
        }

        private void StartDownload(string tmpPath, GetData dataFunc, ErrorBack error = null, WWWForm _form = null, float time = 10f)
        {
            queue.Enqueue(new DownloadData(tmpPath, dataFunc, error, _form, time));
            if (!isDownloading)
            {
                base.StartCoroutine(DownloadFile());
                isDownloading = true;
            }
        }

        public static void Loading(string tmpPath, GetData dataFunc, ErrorBack error = null, WWWForm _from = null, float time = 10f)
        {
            Instance.StartDownload(tmpPath, dataFunc, error, _from, time);
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public void Dis()
        {
            base.StopCoroutine("DownloadFile");
            isDownloading = false;
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