using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IArt
{
    bool IsWaitArt(ushort key = 0);
    bool UseArt(ResData obj);

    string ArtName();
    string[] AbName();
    string AbSingleName();
    int AbSingleID();
    FileRoot RootName();

    IEnumerator<float> Loading(AssetBundle ab, ResData _data);
}