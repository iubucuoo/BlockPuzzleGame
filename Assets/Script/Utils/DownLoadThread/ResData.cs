using UnityEngine;
using System.Collections.Generic;
using WUtils.Utils;
using ProtoBuf;
using System.IO;

public enum ResStatus:byte
{
    None,
    Downing,
    Loading,
}
/// <summary>
/// 1.比对文件
/// ID->MD5
/// URL->
/// </summary>
[ProtoContract]
public class ResData
{
    [ProtoMember(1)]
    public string md5;//MD5
    [ProtoMember(2)]
    public string url;//abName->sceneInfo/job3
    [ProtoMember(3)]
    public string name;//name->job3,
    [ProtoMember(4)]
    public bool isFinish;
    [ProtoMember(5)]
    public FileRoot root;   
    [ProtoMember(7)]
    public ushort mapID;
    [ProtoMember(8)]
    public byte type;//0在服务器，1在包内
    [ProtoMember(9)]
    public ushort kb;
    public ResStatus status;
    public const string postfix="";
    public static string[] dirs;
    string localpath = "本地缓存路径";
    public string persis_dir
    {
        get
        {
            if (dirs == null)
            {
                dirs = new string[]
                {
                   
                    "",
                    //localpath+FileRoot.scenes ,
                    //localpath+FileRoot.tables,
                    //localpath+FileRoot.firstinfo,
                    //localpath+FileRoot.minimap,
                    //localpath+FileRoot.unitinfo,
                    //localpath+FileRoot.monsterinfo,
                    //localpath+FileRoot.npc,
                    //localpath+FileRoot.otherinfo,
                    //localpath+FileRoot.effectinfo,
                    //localpath+FileRoot.plantinfo,
                    //localpath+FileRoot.trapinfo,
                    //localpath+FileRoot.mapsdata,
                };
            }
            if (root==FileRoot.mapinfo)
            {
                return persis_url.Remove(persis_url.LastIndexOf('/'));//本地目录
            }
            return dirs[(int)root];
        }
    }
    string _persis_url;
    public string persis_url { get {return _persis_url ?? (_persis_url = localpath +url) ;} }    
    public string loadurl
    {
        get
        {
            //            int len = url.Length;
            //            SpeedString sb = null;
            //            if (type == 0&&isFinish)//为了兼容旧版本的ios，在包外的资源 isfinish 针对first的资源
            //            {
            //                len += GlobalData.LocalPathLen;
            //                sb = StringMgr.inst.GetString(len);
            //                sb.Append(GlobalData.LocalPath);                
            //                sb.Append(url);
            //            }
            //            else
            //            {
            //                if (type==0)
            //                {
            //                    Debug.LogWarning("数据异常读了包内的资源"+url);
            //                }
            //                len += streamingPathLen + 1;
            //                #if UNITY_IOS
            //                len+=2*postfix.Length; //混淆
            //                #endif
            //                sb = StringMgr.inst.GetString(len);
            //                sb.Append(streamingPath);
            //                sb.Append("/");
            //#if UNITY_IOS
            //                sb.Append(postfix);
            //#endif
            //                sb.Append(url);
            //#if UNITY_IOS
            //                sb.Append(postfix);
            //#endif
            //            }
            //            return sb.string_base;
            return "";
        }
    }
    string _serverurl;
    public string server_url
    {
        get
        {
            //if (_serverurl == null)
            //{
            //    StaticTools.newSb.Length = 0;
            //    StaticTools.newSb.Append(GlobalData.ServerConfig.end_res_url);
            //    StaticTools.newSb.Append(url);
            //    StaticTools.newSb.Append(GlobalData.ServerConfig.cdnVersion);
            //    _serverurl = StaticTools.newSb.ToString();
            //}
            //return _serverurl;
            return "服务器地址";
        }
    }

    public sbyte IsLog = -1;
    
    public ushort key;//资源获取之后，将名字改成key，查询
    public Dictionary<string , Object> objs;//ab 中的objs
    public Dictionary<string, Object[]> objs_a;//ab 中的objs_a
    public UnityEngine.Object obj;
    public WUtils.Utils.Node objNode;
    public bool isLoad = true;
    public AssetBundle abres;

    public float time;
   
    internal bool IsNeedDown()
    {
        return isFinish == false && type == 0;
    }

    #region EDITOR
#if UNITY_EDITOR
    public ResData(string _id, string _url, string _name, FileRoot _root, int check_inpackage,bool _type,ushort _kb)
    {
        kb = _kb;
        md5 = _id;
        url = _url;
        name = _name;
        root = _root;
        
        if (root == FileRoot.mapinfo)
        {
            int star = url.IndexOf("/") + 1;
            int end = url.LastIndexOf("/");
            string str = url.Substring(star, end - star);
            mapID = ushort.Parse(str);
        }
        else if (root == FileRoot.minimap||root==FileRoot.mapsdata)
        {
            mapID = ushort.Parse(name);
        }
        if (_type)
        {
            type= 1;
            return;
        }
        switch (check_inpackage)
        {
            case 0:
                {
                    //在服务器
                } break;
            case 1:
                {
                    InPackage();     //在包内               
                }
                break;
            case 2:
                {
                    type = 1;
                }
                break;
            case 3:
                {
                    if (root == FileRoot.windowinfo||root==FileRoot.firstinfo)
                    {
                        type = 1;
                    }
                }
                break;
            default:
                break;
        }
    }
   public bool IsSame(ResData res)
    {
        return res.url == url && res.root == root;
    }
    void InPackage()
    {
        if (root == FileRoot.lua
            || root == FileRoot.windowinfo
            || root == FileRoot.firstinfo 
            || root ==FileRoot.scenes 
            || root ==FileRoot.tables 
            || root ==FileRoot.datainfo 
            || root ==FileRoot.sound 
            || root ==FileRoot.dll 
            || root ==FileRoot.npc
            || root ==FileRoot.minimap//现在小地图比较少，都放包内
            || root ==FileRoot.mapsdata//mapdata 地图配置都放包内
            )
        {
            type = 1;
            return;
        }      
    }
#endif
#endregion

    public bool NullRes(string artName)
    {
        if (root==FileRoot.minimap&&
            root==FileRoot.lua&&
            root==FileRoot.windowinfo&&
            root==FileRoot.scenes&&
            root==FileRoot.firstinfo&&
            obj==null)
        {
            return true;
        }
        else
        {
            UnityEngine.Object temp;
            if (objs == null || objs.TryGetValue(artName, out temp) == false)
            {
                return true;
            }
            else if (temp == null)
            {
                objs.Remove(artName);
                return true;
            }
        }        
        return false;        
    }
    public ResData()
    {

    }
    public bool Finish()
    {
        isFinish = true;
        bool isResult = isLoad;
        isLoad = true;        
        return isResult;
    }
    public void Repair()
    {
        if (File.Exists(loadurl))
        {
            System.IO.File.Delete(loadurl);
        }
        isFinish = false;
        //文件二进制有问题
    }
    public void Clear()
    {
        if (objs != null)
        {
            var e = objs.GetEnumerator();
            while (e.MoveNext())
            {
                Resources.UnloadAsset(e.Current.Value);
            }
            e.Dispose();
        }


        if (objs_a != null)
        {
            var em = objs_a.GetEnumerator();
            while (em.MoveNext())
            {
                var t = em.Current.Value;
                for (int i = 0; i < t.Length; i++)
                {
                    Resources.UnloadAsset(t[i]);
                }
            }
            em.Dispose();
        }
        
        OnlyCacel();
    }

    public void OnlyCacel()
    {
        obj = null;

        if (objs!=null)
        {
            objs.Clear();
        }
        objs = null;

        if (objs_a != null)
        {
            objs_a.Clear();
        }
        objs_a = null;
    }    

    public void DelayClearObjs(float t=0)
    {
        time = Time.time+t;
    }
   
    public bool isCacel()
    {
        var tmp = objNode;
        if (tmp != null)
        {
            IArt obj = (IArt)tmp.Value;

            if (obj.IsWaitArt(key))
            {
                return false;
            }

            while (tmp.next != null)
            {
                tmp = tmp.next;
                obj = (IArt)tmp.Value;
                if (obj.IsWaitArt(key))
                {
                    return false;
                }
            }
            return true;
        }
        return true;
    }
    public void FeedBack()
    {
        if (objNode != null)
        {
            IArt obj = (IArt)objNode.Value;
            Node tmp = objNode;
            FeedBackSingle(obj);
            while (objNode.next != null)
            {
                objNode = objNode.next;
                tmp.next = null;
                tmp = objNode;
                obj = (IArt)objNode.Value;
                FeedBackSingle(obj);
            }
            tmp = null;
            objNode = null;
        }
        status = ResStatus.None;
    }      
    public void ClearNode()
    {
        if (objNode != null)
        {
            //IArt obj = (IArt)objNode.Value;
            WUtils.Utils.Node tmp = objNode;            
            while (objNode.next != null)
            {
                objNode = objNode.next;
                tmp.next = null;
                tmp = objNode;
               // obj = (IArt)objNode.Value;               
            }
            tmp = null;
            objNode = null;
        }
        status = ResStatus.None;
    }
    public void FeedBackSingle(IArt obj)
    {
        if (obj.IsWaitArt(key))
        {
            obj.UseArt(this);
        }
    }

}