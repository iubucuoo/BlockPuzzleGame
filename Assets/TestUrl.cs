using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtils.Utils;

public class TestUrl : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<MEC.Timing>();
        var xxx = new ResCenter();
        xxx.InitMgr();
    }
    // Start is called before the first frame update
    void Start()
    {
        DebugMgr.LogError(WUtils.PathTools.GetParentPath("你/号/啊/2.kko", 1));
        DownloadTools.LoadUrl("https://www.iubucuoo.com/index.xml",5, (www) =>
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

        TableMgr.Inst._Cb = Init;
        MsgSend.GetRes(TableMgr.Inst);
    }
    void Init()
    {
        DebugMgr.LogError("TableMgr.inst._Cb");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
