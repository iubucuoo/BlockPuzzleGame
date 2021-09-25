using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainC : MonoBehaviour
{
    private void Awake()
    {
        var canver = GameObject.Find("Canvas");
        GameGloab.root_bg = canver.transform.Find("gamebg/BGROOT");
        GameGloab.root_prep = canver.transform.Find("gamebg/ADDROOT");
        DragingGridMgr.Inst.DragRoot = canver.transform.Find("gamebg/DragRoot");
    }
    public List<Sprite> sprites = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        //先载入数据文件
        LoadLanguageData();
        //

        gameObject.AddComponent<GridGroupMgr>();

        foreach (var v in sprites)
        {
            GameGloab.Sprites[v.name] = v;
        }
    }

    void LoadLanguageData()
    {
        var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/language.ly");
        StartCoroutine(Loading(ab));
    }
    IEnumerator<float> Loading(AssetBundle ab)
    {
        var objs = ab.LoadAllAssets();
        UseArt(objs);
        yield return 0;
        ab.Unload(false);
    }
    void UseArt(object[] objs)
    {
        foreach (var str in objs)
        {
            var languagedata = JsonUtility.FromJson<LanguageData>((str as TextAsset).text);
            foreach (var v in languagedata.datas)
            {
                if (!LanguageManger.Inst.languagedic.ContainsKey(v.LanguageList))
                {
                    Dictionary<string, string> ky = new Dictionary<string, string>();
                    foreach (var vv in v.ky)
                    {
                        ky.Add(vv.key, vv.value);
                    }
                    LanguageManger.Inst.languagedic.Add(v.LanguageList, ky);
                }
            }
        }
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
