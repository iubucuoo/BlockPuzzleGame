using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguageData
{
    public LanguageItem[] datas;
}
[System.Serializable]
public class LanguageItem
{
    public LanguageList LanguageList;
    public LanguageKY [] ky;
}
[System.Serializable]
public class LanguageKY
{
    public string key;
    public string value;
}
