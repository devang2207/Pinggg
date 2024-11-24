using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//using System.IO;
using TMPro;

//using System.IO;
using UnityEngine;

public static class FileHandler
{
    //save json in file
    public static void SaveToJSON<T>(List<T> toSave,string filename)
    {
        Debug.Log(GetPath(filename));
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(GetPath(filename),content);
    } 
    //read from json file
    public static List<T> ReadListFromJSON<T>(string fileName)
    {
        string content = ReadFile(GetPath(fileName));
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            Debug.Log("content is empty!");
            return new List<T>();
        }
        //Debug.Log("Got the file");
        List<T> list = JsonHelper.FromJson<T>(content).ToList();
        return list;
    }   
    //get the path of json file 
    public static string GetPath(string fileName)
    {
        return Application.dataPath + "/Resources/" + fileName + ".json";
    }

    public static string ReadFile(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            using (StreamReader streamReader = new System.IO.StreamReader(filePath))
            {
                string content = streamReader.ReadToEnd();
                return content;
            }
        }
        else
        {
            Debug.Log("file does not exists!");
            return "";
        }
    }
    public static void WriteFile(string filePath,string contentToWrite)
    {
        //create file if it does not exist or ovverrite if it exists
        FileStream fileStream = new FileStream(filePath,FileMode.Create);

        using(StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(contentToWrite);
        }
        
    }
}
public static class JsonHelper
{
    public static T[] FromJson<T> (string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}