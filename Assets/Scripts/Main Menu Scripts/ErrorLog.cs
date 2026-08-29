using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class ErrorLog : MonoBehaviour
{
    public List<string> logs = new List<string>();
    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }
    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }
    private void Log(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Warning || string.IsNullOrEmpty(condition))
        {
            return;
        }
        string log = "[" + type + "]\n" + "Scene: " + SceneManager.GetActiveScene().name + "\n" + "At: " + DateTime.UtcNow + "\n" + condition + "\n" + stackTrace + "\n";
        logs.Add(log);
        string path = Path.Combine(Application.streamingAssetsPath, "log.txt");
        if (!File.Exists(path))
        {
            File.Create(path);
        }
        File.AppendAllText(path, log);
        float length = new FileInfo(path).Length;
        if (length >= 10000000) //1mb
        {
            File.WriteAllText(path, "");
            File.AppendAllText(path, log);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
