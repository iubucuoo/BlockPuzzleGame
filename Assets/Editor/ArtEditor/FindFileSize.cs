using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;

class FindFileSize
{
    static List<string> sizeList = new List<string>();
    static string root;
    static string dir;

    //获取文件信息info
    public static List<string> GetDic()
    {
        sizeList.Clear();
        string path = EditorPathTools.SVN_RES_ROOT;
        string[] pas = path.Split('/');
        //截取最后一个字符串
        dir = pas[pas.Length - 1];
        //本地路径
        root = path.Substring(0,path.IndexOf(dir));
        listDirectory(path);
        //var _ResPaths = AssetDatabase.GetAllAssetPaths();
        //for (int i = 0; i < _ResPaths.Length; i++)
        //{
        //    var respath = _ResPaths[i];
        //    if (respath.Contains(root))
        //    {
        //        if (!Ignore(respath))
        //        {
        //            var key = respath.Substring(respath.IndexOf(dir));
        //            //版本号
        //            string fullPath = root + key;
        //            string fileSize = string.Empty;
        //            if (File.Exists(fullPath))
        //            {
        //                long size = new FileInfo(fullPath).Length;
        //                fileSize = size.ToString();
        //                string str = key + "\t" + 0 + "\t" + fileSize;
        //                DebugMgr.Log("FindfileSize     "+str);
        //                sizeList.Add(str);
        //            }
        //        }
        //    }
        //}


        return sizeList;
    }
    private static void listDirectory(string path)
    {
        DirectoryInfo theFolder = new DirectoryInfo(path);
         
        //遍历文件
        foreach (FileInfo NextFile in theFolder.GetFiles())
        {

            DebugMgr.Log(root);
            var respath =  NextFile.FullName.Replace("\\","/");
            DebugMgr.Log(respath);
            if (respath.Contains(root))
            {
                if (!Ignore(respath))
                {
                    var key = respath.Substring(respath.IndexOf(dir));
                    //版本号
                    string fullPath = root + key;
                    string fileSize = string.Empty;
                    if (File.Exists(fullPath))
                    {
                        long size = new FileInfo(fullPath).Length;
                        fileSize = size.ToString();
                        string str = key + "\t" + 0 + "\t" + fileSize;
                        //DebugMgr.Log("FindfileSize     " + str);
                        sizeList.Add(str);
                    }
                }
            }
        }

        //遍历文件夹
        foreach (DirectoryInfo NextFolder in theFolder.GetDirectories())
        {
            listDirectory(NextFolder.FullName);
        }
    }
    //忽略指定后缀名文件
    static string[] words = new string[] { "ignore", ".meta", ".swatch", ".sln", ".user", ".bat" };
    static bool Ignore(string path)
    {
        if (path.IndexOf(".") < 0)
        {
            return true;
        }
        foreach (var item in words)
        {
            if (path.IndexOf(item) >= 0)
            {
                return true;
            }
        }
        return false;
    }
}
