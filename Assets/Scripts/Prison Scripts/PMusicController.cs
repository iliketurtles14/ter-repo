using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMusicController : MonoBehaviour
{
    private static Map currentMap;
    private static Dictionary<string, int> musicDict = new Dictionary<string, int>
    {
        { "chow", 0 }, { "escaped", 1 }, { "lightsout", 2 }, { "freetime", 3 }, { "rollcall", 4 },
        { "shower", 5 }, { "work", 6 }, { "workout", 7 }, { "lockdown", 8 }
    };
    private static AudioSource audioSource;
    private IniFile iniFile;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        iniFile = new IniFile(System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath, "UserData.ini"));
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
    }
    private void Update()
    {
        audioSource.volume = Convert.ToInt32(iniFile.Read("Music", "Settings")) / 100f;
    }
    public static void PlayMusic(string musicClip)
    {
        if(musicClip == "escaped")
        {
            RootObjectCache.GetRoot("EscapeMusicSource").GetComponent<AudioSource>().Stop();
            RootObjectCache.GetRoot("EscapeMusicSource").GetComponent<AudioSource>().clip = currentMap.music[musicDict[musicClip]];
            RootObjectCache.GetRoot("EscapeMusicSource").GetComponent<AudioSource>().Play();
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = currentMap.music[musicDict[musicClip]];
            audioSource.Play();
        }
    }
}
