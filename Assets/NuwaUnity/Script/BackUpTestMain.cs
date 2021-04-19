using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BackUpTestMain : MonoBehaviour
{
    public Transform contents;
    Text[] text;
    string dataKey = "SaveTime";

    void Start()
    {
        /**
        //D:/2 -Git/UnityPluginsNew/MiboUnity/Assets
        Debug.Log(Application.dataPath);

        //C:/Users/nuwa/AppData/LocalLow/DefaultCompany/MiboUnity
        Debug.Log(Application.persistentDataPath);

        //D:/2 -Git/UnityPluginsNew/MiboUnity/Assets/StreamingAssets
        Debug.Log(Application.streamingAssetsPath);

        //C:/Users/nuwa/AppData/Local/Temp/DefaultCompany/MiboUnity
        Debug.Log(Application.temporaryCachePath);
        **/

        text = contents.GetComponentsInChildren<Text>();
    }

    public void BtnEvent_SaveData()
    {
        Debug.Log("寫入檔案");

        string nowTime = DateTime.Now.ToString();
        PlayerPrefs.SetString(dataKey, nowTime);

        string fileName = "BackUpTestFile";
        string path;

        path = Application.dataPath + "/" + fileName;
        SaveFile(path, nowTime);

        path = Application.persistentDataPath + "/" + fileName;
        SaveFile(path, nowTime);

        path = Application.streamingAssetsPath + "/" + fileName;
        SaveFile(path, nowTime);

        path = Application.temporaryCachePath + "/" + fileName;
        SaveFile(path, nowTime);
    }

    public void BtnEvent_LoadData()
    {
        Debug.Log("讀取檔案");

        string fileName = "BackUpTestFile";
        string path;
       
        text[0].text = PlayerPrefs.GetString(dataKey);

        path = Application.dataPath + "/" + fileName;
        text[1].text = "dataPath : " + LoadFile(path);

        path = Application.persistentDataPath + "/" + fileName;
        text[2].text = "persistentDataPath : " + LoadFile(path);

        path = Application.streamingAssetsPath + "/" + fileName;
        text[3].text = "streamingAssetsPath : " + LoadFile(path);

        path = Application.temporaryCachePath + "/" + fileName;
        text[4].text = "temporaryCachePath : " + LoadFile(path);
    }


    private void SaveFile(string path, string contents)
    {
        try
        {
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(contents);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    private string LoadFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("This file does not exist：" + path);
            return "";
        }

        try
        {
            // Open the stream and read it back.
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Debug.Log(path);
                    Debug.Log(s);
                    return s;
                }
            }
        }
        catch (Exception ex)
        {
            string s = "Failed to read file";
            Debug.LogError(ex.ToString());
            return s;
        }
        return "Failed to read file";
    }

    public void RetuenToTitle()
    {
        this.enabled = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }
}

[Serializable]
public class GameData
{
    public string SaveTimes;
}
