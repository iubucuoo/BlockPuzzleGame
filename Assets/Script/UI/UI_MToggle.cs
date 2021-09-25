using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MToggle : MonoBehaviour
{
    public GameObject IsOnObj;
    public GameObject IsOffObj;
    Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
        OnValueChanged(toggle.isOn);
    }
    void OnValueChanged(bool ison)
    {
        IsOnObj.SetActive(ison);
        IsOffObj.SetActive(!ison);
    }
}
