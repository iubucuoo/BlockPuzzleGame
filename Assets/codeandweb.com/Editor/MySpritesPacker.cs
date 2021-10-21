
#if UNITY_EDITOR
using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
public class MySpritesPacker : Editor
{
    [MenuItem("Tools/SpritesPacker/TexturePacker")]
    public static void BuildTexturePacker()
    {
        string inputPath = CommandBuild.path_outAltasData;
        string[] imagePath = Directory.GetFiles(inputPath);
        foreach (string path in imagePath)
        {
            if (Path.GetExtension(path) == ".png" || Path.GetExtension(path) == ".PNG")
            {
                string sheetPath = GetAssetPath(path);
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(sheetPath);
                //Debug.Log(texture.name);
                string rootPath = CommandBuild.path_outAltas + texture.name;
                string pngPath = rootPath + "/" + texture.name + ".png";
                //TextureImporter asetImp = null;
                Dictionary<string, Vector4> tIpterMap = new Dictionary<string, Vector4>();
                if (Directory.Exists(rootPath))
                {
                    if (File.Exists(pngPath))
                    {
                        //Debug.LogError("exite: " + pngPath);
                        //asetImp = GetTextureIpter(pngPath);
                        //SaveBoreder(tIpterMap, asetImp);
                        File.Delete(pngPath);
                    }
                    File.Copy(inputPath + texture.name + ".png", pngPath);
                }
                else
                {
                    //Debug.LogError("Create: " + pngPath);
                    Directory.CreateDirectory(rootPath);
                    File.Copy(inputPath + texture.name + ".png", pngPath);
                }
                AssetDatabase.Refresh();
                FileStream fs = new FileStream(inputPath + texture.name + ".xml", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string jText = sr.ReadToEnd();
                fs.Close();
                sr.Close();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(jText);
                XmlNodeList elemList = xml.GetElementsByTagName("SubTexture");
                WriteMeta(elemList, texture.name, tIpterMap);
            }
        }
        AssetDatabase.Refresh();
    }
    //如果这张图集已经拉好了9宫格，需要先保存起来
    static void SaveBoreder(Dictionary<string, Vector4> tIpterMap, TextureImporter tIpter)
    {
        for (int i = 0, size = tIpter.spritesheet.Length; i < size; i++)
        {
            tIpterMap.Add(tIpter.spritesheet[i].name, tIpter.spritesheet[i].border);
        }
    }

    static TextureImporter GetTextureIpter(Texture2D texture)
    {
        TextureImporter textureIpter = null;
        string impPath = AssetDatabase.GetAssetPath(texture);
        textureIpter = TextureImporter.GetAtPath(impPath) as TextureImporter;
        return textureIpter;
    }

    static TextureImporter GetTextureIpter(string path)
    {
        TextureImporter textureIpter = null;
        Texture2D textureOrg = AssetDatabase.LoadAssetAtPath<Texture2D>(GetAssetPath(path));
        string impPath = AssetDatabase.GetAssetPath(textureOrg);
        textureIpter = TextureImporter.GetAtPath(impPath) as TextureImporter;
        return textureIpter;
    }
    //写信息到SpritesSheet里
    static void WriteMeta(XmlNodeList elemList, string sheetName, Dictionary<string, Vector4> borders)
    {
        string path = CommandBuild.outAtlaspath + string.Format("{0}/{1}.png", sheetName, sheetName);
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        string impPath = AssetDatabase.GetAssetPath(texture);
        TextureImporter asetImp = TextureImporter.GetAtPath(impPath) as TextureImporter;
        SpriteMetaData[] metaData = new SpriteMetaData[elemList.Count];
        for (int i = 0, size = elemList.Count; i < size; i++)
        {
            XmlElement node = (XmlElement)elemList.Item(i);
            Rect rect = new Rect();
            rect.x = int.Parse(node.GetAttribute("x"));
            rect.y = texture.height - int.Parse(node.GetAttribute("y")) - int.Parse(node.GetAttribute("height"));
            rect.width = int.Parse(node.GetAttribute("width"));
            rect.height = int.Parse(node.GetAttribute("height"));
            metaData[i].rect = rect;
            metaData[i].pivot = new Vector2(0.5f, 0.5f);
            metaData[i].name = node.GetAttribute("name");
            //读取源文件的meta文件，获取spriteBorder九宫格信息，写进图集中
            string sourcePath = string.Format(CommandBuild.path_Images + "/{1}/{2}.png", Application.dataPath, sheetName, node.GetAttribute("name"));
            //Debug.LogError(sourcePath);
            if (!File.Exists(sourcePath))
            {
                sourcePath = string.Format(CommandBuild.path_Images + "/{1}/{2}.jpg", Application.dataPath, sheetName, node.GetAttribute("name"));
                if (!File.Exists(sourcePath))
                {
                    //Debug.LogError("不存在的地址: " + sourcePath);
                    continue;
                }
            }
            Vector4 sourceBorder = GetTextureIpter(sourcePath).spriteBorder;
            //Debug.Log("图片的路径" + sourcePath + "图片的border" + sourceBorder.ToString());
            metaData[i].border = sourceBorder;
        }
        asetImp.spritesheet = metaData;
        asetImp.textureType = TextureImporterType.Sprite;
        asetImp.spriteImportMode = SpriteImportMode.Multiple;
        asetImp.mipmapEnabled = false;
        asetImp.SaveAndReimport();
    }

    static string GetAssetPath(string path)
    {
        string[] seperator = { "Assets" };
        string p = "Assets" + path.Split(seperator, StringSplitOptions.RemoveEmptyEntries)[1];
        return p;
    }

}

#endif
