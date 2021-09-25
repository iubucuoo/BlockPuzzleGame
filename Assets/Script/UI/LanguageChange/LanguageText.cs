using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    public string key;
    Text m_text;
    public void LChange()
    {
       var txt = LanguageManger.Inst.GetTextByKey(key);
        if (txt!=null)
        {
            m_text.text = txt;
        }
    }
    
    private void OnEnable()
    {
        LanguageManger.Inst.RegisterText(this);
        LChange();
    }
    private void OnDisable()
    {
        LanguageManger.Inst.UnregisterText(this);
    }
    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

}
