using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum LanguageList
    {
        En,
        Cn
    }
public class LanguageManger //:MonoBehaviour
{
    //private void Awake()
    //{
    //    _inst = this;
    //}
    private static LanguageManger _inst;
    public static LanguageManger Inst
    {
        get{
            if (_inst == null)
            {
                _inst = new LanguageManger();
            }
            return _inst;
        }
    }

    private LanguageList m_curLanguage;
    List<LanguageText> languages;
    public Dictionary<LanguageList, Dictionary<string, string>> languagedic;
    LanguageManger()
    {
        languages = new List<LanguageText>();
        m_curLanguage = LanguageList.Cn;
        languagedic = new Dictionary<LanguageList, Dictionary<string, string>>();
    }
    //private void Start()
    //{
    //    languages = new List<LanguageText>();
    //    m_curLanguage = LanguageList.Cn;
    //    languagedic = new Dictionary<LanguageList, Dictionary<string, string>>();
    //    LoadLanguage();
    //}
    //注册语言文本
    public void RegisterText(LanguageText txt)
    {
        if(!languages.Contains(txt))
        languages.Add(txt);
    }
    //注销语言文本
    public void UnregisterText(LanguageText txt)
    {
        if (languages.Contains(txt))
            languages.Remove(txt);
    }
    public void ChangeLanguage(LanguageList lanl)
    {
        m_curLanguage = lanl;
        OnLanguageChange();
    }

    void OnLanguageChange()
    {
        foreach (var v in languages)
        {
            v.LChange();
        }
    }
    public string GetTextByKey(string key)
    {
        if (languagedic.ContainsKey(m_curLanguage) && languagedic[m_curLanguage].ContainsKey(key))
        {
            return languagedic[m_curLanguage][key];
        }
        else
        {
            DebugMgr.LogError("当前语言不存在");
            return null;
        }
    }

    //void LoadLanguageData()
    //{
    //    var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/language.ly");
    //    StartCoroutine(Loading(ab));
    //}
    //IEnumerator<float> Loading(AssetBundle ab)
    //{
    //    var objs = ab.LoadAllAssets();
    //    UseArt(objs);
    //    yield return 0;
    //    ab.Unload(false);
    //}
    //void UseArt(object[] objs)
    //{
    //    foreach (var str in objs)
    //    {
    //        var languagedata = JsonUtility.FromJson<LanguageData>((str as TextAsset).text);
    //        foreach (var v in languagedata.datas)
    //        {
    //            if (!LanguageManger.Inst.languagedic.ContainsKey(v.LanguageList))
    //            {
    //                Dictionary<string, string> ky = new Dictionary<string, string>();
    //                foreach (var vv in v.ky)
    //                {
    //                    ky.Add(vv.key, vv.value);
    //                }
    //                LanguageManger.Inst.languagedic.Add(v.LanguageList, ky);
    //            }
    //        }
    //    }
    //}
}
