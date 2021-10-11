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
        OnValueChanged(toggle.isOn);
        toggle.onValueChanged.AddListener(OnValueChanged);
    }
    void OnValueChanged(bool ison)
    {
        IsOnObj.SetActive(toggle.isOn);
        IsOffObj.SetActive(!toggle.isOn);
    }
}
