using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Picker : MonoBehaviour
{
    public List<string> options = new List<string>();
    private TextMeshProUGUI tmp;
    public int currentIndex;
    public MMSoundController soundController;
    public MESoundController meSoundController;
    private void Start()
    {
        tmp = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        tmp.text = options[currentIndex];
    }
    public void PickerLeft()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Prison":
                PSoundController.PlaySound("plip");
                break;
            case "Main Menu":
                soundController.PlaySound("plip");
                break;
            case "Map Editor":
                meSoundController.PlaySound("plip");
                break;

        }
        if (currentIndex > 0 && options.Count > 0)
        {
            currentIndex--;
        }
        else if(options.Count > 0)
        {
            currentIndex = options.Count - 1;
        }
    }
    public void PickerRight()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Prison":
                PSoundController.PlaySound("plip");
                break;
            case "Main Menu":
                soundController.PlaySound("plip");
                break;
            case "Map Editor":
                meSoundController.PlaySound("plip");
                break;

        }
        if (currentIndex < options.Count - 1 && options.Count > 0)
        {
            currentIndex++;
        }
        else if(options.Count > 0)
        {
            currentIndex = 0;
        }
    }
}
