using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Package
{
    static Dictionary<string, UI_Package> _packageInstById = new Dictionary<string, UI_Package>();
    static Dictionary<string, UI_Package> _packageInstByName = new Dictionary<string, UI_Package>();
    public static UI_Package GetByName(string name)
    {
        UI_Package pkg;
        if (_packageInstByName.TryGetValue(name, out pkg))
            return pkg;
        else
            return null;
    }
}
