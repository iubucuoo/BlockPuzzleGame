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
            Debug.LogError("当前语言不存在");
            return null;
        }
    }


}
