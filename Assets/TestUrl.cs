﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        new TableArt(Init);
    }
    void Init()
    {
        Log.Error("TableMgr.inst._Cb");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
