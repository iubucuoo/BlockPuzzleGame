using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtils.Utils;

public class TestUrl : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<MEC.Timing>();
    }
    // Start is called before the first frame update
    void Start()
    {
        new ProtobufTools();
        DownloadTools.Loading("https://www.iubucuoo.com/index.xml", (www) =>
        //DownloadTools.Loading("https://gitee.com/wangshijun12/GameInfo/raw/master/test.txt", (www) =>
        {
            Debug.Log(www.text);
            string _str = www.text;
            //_str = StaticTools.Entray(_str);
            Debug.Log(_str);
        }, (error) =>
        {
            Debug.LogError("无法加载网络资源");
        });

        var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/chinese.ly");
        TableMgr.inst.Load(ab, () =>
        {
            var xxx = ItemManager.GetSingleData(101);
            Debug.LogError(xxx == null);
            Debug.LogError(xxx.name);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
