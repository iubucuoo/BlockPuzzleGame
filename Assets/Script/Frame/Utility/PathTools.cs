using System;
using System.IO;
using UnityEngine;
namespace WUtils
{
    public static class PathTools
    {
        public const char SIGN = '|';
        public const string MD5BODY = "{0}|{1}|{2}|{3}|{4}|{5}";
        public const string ALL_VERSION = "Version|{0}\nCVersion|{1}";
        public static string DOT_AbSUFFIX = ".bundle";//点后缀
        public static string AbSUFFIX = "bundle";
        public static string DllABPath = "dll/main";
        public static string Dot_Logic = ".logic";

        public static string Log = Application.dataPath + "/../Log.txt";

        public static string PLATFORM = GetPlatformFolderName(Application.platform);
        public static string GetNetUrlPath { get { return "http://192.168.5.15/2d/DouLDL/" + PLATFORM + "/"; } }
        public static string GetNativeResDir = Application.dataPath + "/Art/";
        public static string PROJECT_TABLES = GetNativeResDir + "Tables";

        public static string NET_VERSION { get { return GetNetUrlPath + "Version.bytes"; } }
        public static string CACHE_RES_ROOT = GetAppFilePath() + "/" + PLATFORM + "/";
        public static string CACHE_VERSION = CACHE_RES_ROOT + "Version.bytes";
        public static string STREAM_RES_PATH = GetAppFilePath(true) + "/" + PLATFORM + "/";
        public static string STREAM_VERSION = STREAM_RES_PATH + "Version.bytes";
        public static string WWW_CACHE_RES_ROOT =
#if !UNITY_EDITOR
		"file://" + CACHE_RES_ROOT;
#else
        CACHE_RES_ROOT;
#endif
        public static string WWW_CACHE_VERSION =
#if !UNITY_EDITOR
		"file://" + CACHE_VERSION;
#else
        CACHE_VERSION;
#endif

        public static string GetPlatformFolderName(RuntimePlatform platform)
        {
#if UNITY_EDITOR
            return "Android";
#else
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                default:
                    return null;
            }
#endif
        }

        public static bool IsEditorPlatform()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != 0)
            {
                return false;
            }
            return true;
        }

        public static string GetAppFilePath(bool isStreamingPath = false)
        {
            if (isStreamingPath)
            {
                return Application.streamingAssetsPath;
            }
            return Application.persistentDataPath;
        }

        public static string GetAssetBundleRootPath(bool isStreamingPath = false)
        {
            return GetAppFilePath(isStreamingPath) + "/" + PLATFORM;
        }

        public static string GetWWWAssetBundlePath(bool isStreamingPath = false, bool JustPath = false)
        {
            string text = JustPath ? GetAppFilePath(isStreamingPath) : GetAssetBundleRootPath(isStreamingPath);
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                text = "file:///" + text;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (text.IndexOf(Application.persistentDataPath) >= 0)
                {
                    text = "file:///" + text;
                }
            }
            else
            {
                text = ((Application.platform != RuntimePlatform.WindowsPlayer) ? ("file://" + text) : ("file:///" + text));
            }
            return text;
        }
        public static string GetParentName(string path, int count = 0)
        {
            int i = 0;
            var parent = path;
            var len = parent.Length;
            while (i < count)
            {
                len = parent.LastIndexOf('/', --len);
                i++;
            }
            var stalen = parent.LastIndexOf('/', len - 1) + 1;
            parent = parent.Substring(stalen, len - stalen);
            return parent;
        }
        public static string GetParentPath(string path, int count)
        {
            int i = 0;
            var parent = path;
            var len = parent.Length;
            while (i < count)
            {
                len = parent.LastIndexOf('/', --len);
                i++;
            }
            parent = parent.Substring(0, len);
            return parent;
        }
        public static string GetSubPath(string path, int count)
        {
            int i = 0;
            var sub = path;
            var len = 0;
            while (i < count)
            {
                len = sub.IndexOf('/', ++len);
                i++;
            }
            sub = sub.Substring(len + 1);
            return sub;
        }
        ///获得配置版本
        public static string GetLineVersion(string path, int lineNum)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string str = null;
            for (int i = 0; i < lineNum; i++)
            {
                str = sr.ReadLine();
            }
            sr.Close();
            fs.Close();
            return str;
        }
    }
}