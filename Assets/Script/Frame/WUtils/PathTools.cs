using System;
using UnityEngine;
namespace WUtils
{
    public static class PathTools
    {
        public static string GetPlatformFolderName(RuntimePlatform platform)
        {
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
            string platformFolderName = GetPlatformFolderName(Application.platform);
            return GetAppFilePath(isStreamingPath) + "/" + platformFolderName;
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
    }
}