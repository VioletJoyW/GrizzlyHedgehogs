using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{


     static string[] scenes = null;

    static List<string> scenePaths = new List<string>();
    static bool isInit = false;
    
    public static int Size { get => scenePaths.Count; }
    public static void Init() 
    {
        if (isInit) return;
        foreach (var path in scenes)
            scenePaths.Add(path);

        isInit = true;
    }

    public static void SetScenes(string[] _scenes) 
    {
        scenes = _scenes;
        Init();
    }

    public static void PlayScene(int id) 
    {
        if (!isInit) return;
        
        SceneManager.LoadScene(scenePaths[id], LoadSceneMode.Single);
    }


}
