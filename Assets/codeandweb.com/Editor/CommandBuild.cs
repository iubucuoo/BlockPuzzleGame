#if UNITY_EDITOR
using System;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;
using System.Diagnostics;

public class CommandBuild : Editor
{
    public static string outAtlaspath = string.Format("Assets/Art/Useart/TexturePacker/OutAtlas/");
    public static string TexturePackerpath = string.Format("{0}/Art/Useart/TexturePacker/", Application.dataPath);

    public static string path_Images = TexturePackerpath + "Images";
    public static string path_outAltasData = TexturePackerpath + "AltasData/";
    public static string path_outAltas = TexturePackerpath + "OutAtlas/";

    [MenuItem("Tools/SpritesPacker/CommandBuild")]
    public static void BuildTexturePacker()
    {
        //选择并设置TP命令行的参数和参数值(包括强制生成2048*2048的图集 --width 2048 --height 2048)
        string commandText = " --sheet {0}.png --data {1}.xml --format sparrow --trim-mode None --pack-mode Best  --algorithm MaxRects --width 2048 --height 2048 --max-size 2048 --size-constraints POT  --disable-rotation --scale 1 {2}" ;
        string inputPath = path_Images;//小图目录
        string[] imagePath = Directory.GetDirectories (inputPath); 
        
        for (int i = 0; i < imagePath.Length; i++) 
        {
            //UnityEngine.Debug.Log (imagePath [i]);
            StringBuilder sb = new StringBuilder("");
            string _path = imagePath[i];
            string[] fileName = Directory.GetFiles(imagePath[i]);
            for (int j = 0; j < fileName.Length; j++)
            {
                string extenstion = Path.GetExtension(fileName[j]);
                if (extenstion == ".png"|| extenstion == ".jpg")
                {
                    sb.Append(fileName[j]);
                    sb.Append("  ");
                    
                    
                }
                //UnityEngine.Debug.Log("fileName [j]:" + fileName[j]);
            }
            string name = Path.GetFileName(imagePath [i]);
            //string outputName = string.Format ("{0}/Art/TexturePacker/{1}/{2}", Application.dataPath,name,name);
            string sheetName = path_outAltasData+ name;
            //执行命令行
            processCommand("H:\\Program Files\\TexturePacker\\bin\\TexturePacker.exe", string.Format(commandText, sheetName, sheetName, sb.ToString()),_path);
        }
        AssetDatabase.Refresh();
    }
    private static void processCommand(string command, string argument,string _path)
    {
        ProcessStartInfo start = new ProcessStartInfo(command);
        start.Arguments = argument;
        start.CreateNoWindow = false;
        start.ErrorDialog = true;
        start.UseShellExecute = false;

        if(start.UseShellExecute){
            start.RedirectStandardOutput = false;
            start.RedirectStandardError = false;
            start.RedirectStandardInput = false;
        } else{
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        }

        Process p = Process.Start(start);
        if(!start.UseShellExecute)
        {
            //UnityEngine.Debug.Log(p.StandardOutput.ReadToEnd());
            //UnityEngine.Debug.Log(p.StandardError.ReadToEnd());
            
            var str = p.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(str))
            {
                UnityEngine.Debug.LogError(_path + "--->" + str);
            }
        }

        p.WaitForExit();
        p.Close();
    }
}
#endif
