using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class MMMusicController : MonoBehaviour
{
    public bool canStartMusic;
    private bool hasStarted;
    private AudioSource audioSource;
    private IniFile iniFile;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        iniFile = new IniFile(System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath, "UserData.ini"));
    }
    private void Update()
    {
        if (canStartMusic && !hasStarted)
        {
            hasStarted = true;
            audioSource.clip = DataSender.instance.MusicList[40];
            audioSource.Play();
        }

        audioSource.volume = Convert.ToInt32(iniFile.Read("Music", "Settings")) / 100f;
    }
}
