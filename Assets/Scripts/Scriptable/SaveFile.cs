using Bewildered;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Save", menuName = "SF/Save")]
public class SaveFile : ScriptableObject
{
    static string key = "SAVESAVESAVESAVESAVESAVESAVESAVE"; //set any string of 32 chars
    static string iv = "FILEFILEFILEFILE"; //set any string of 16 chars

    [SerializeField]
    private bool loadOnStartup = true;

    [SerializeField]
    private GameConstants constants;

    public Vector3? CameraPos { get; set; }
    public float? CameraScroll { get; set; }
    public string LevelSelect { get; set; }

    [SerializeField]
    SaveData m_save;
    public SaveData SaveData => m_save;

    public void CompleteLevel(string levelName)
    {
        if (constants.Levels.ContainsKey(levelName)) m_save.m_levelsCompleted.Add(levelName);
    }
    public bool IsLevelComplete(string levelName)
    {
        return m_save.m_levelsCompleted.Contains(levelName);
    }
    public void Save()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/game_save"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/save.txt");
        var json = JsonUtility.ToJson(m_save);
        var jsonEnc = Crypt(json);
        bf.Serialize(file, jsonEnc);

        PlayerPrefs.SetString("Save", json);

        file.Close();
    }



    public void Load()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/game_save"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }
        BinaryFormatter bf = new BinaryFormatter();
        string json = null;
        if (File.Exists(Application.persistentDataPath + "/game_save/save.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/save.txt", FileMode.Open);
            try
            {
                string jsonEnc = (string)bf.Deserialize(file);
                json = Decrypt(jsonEnc);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            file.Close();
        }
        else
        {
            json = PlayerPrefs.GetString("Save", null);
        }

        if(json != null) JsonUtility.FromJsonOverwrite(json, m_save);
    }

    void OnEnable()
    {
        if(loadOnStartup) Load();
        SaveData.OnEnable();
    }

    private static string Crypt(string text)
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
        AEScryptoProvider.BlockSize = 128;
        AEScryptoProvider.KeySize = 256;
        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        AEScryptoProvider.Mode = CipherMode.CBC;
        AEScryptoProvider.Padding = PaddingMode.PKCS7;

        byte[] txtByteData = ASCIIEncoding.ASCII.GetBytes(text);
        ICryptoTransform trnsfrm = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return Convert.ToBase64String(result);
    }

    private static string Decrypt(string text)
    {
        AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
        AEScryptoProvider.BlockSize = 128;
        AEScryptoProvider.KeySize = 256;
        AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
        AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
        AEScryptoProvider.Mode = CipherMode.CBC;
        AEScryptoProvider.Padding = PaddingMode.PKCS7;

        byte[] txtByteData = Convert.FromBase64String(text);
        ICryptoTransform trnsfrm = AEScryptoProvider.CreateDecryptor();

        byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
        return ASCIIEncoding.ASCII.GetString(result);
    }
}

[Serializable]
public class SaveData
{
    [SerializeField]
    private bool resetOnPlay;

    //Private Fields are Variables that you want to control access to, but also save between sessions, unless reset on play is true.
    [SerializeField]
    public UHashSet<string> m_levelsCompleted;
    public int LevelCompleted => m_levelsCompleted.Count;
    
    public void OnEnable()
    {
        if (resetOnPlay)
        {
            m_levelsCompleted.Clear();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SaveFile))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveFile myScript = (SaveFile)target;
        if (GUILayout.Button("Save"))
        {
            myScript.Save();
        }
    }

}
#endif