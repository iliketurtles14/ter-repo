using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Picker : MonoBehaviour
{
    public List<string> options = new List<string>();
    private TextMeshProUGUI tmp;
    public int currentIndex;
    public MMSoundController soundController;
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
        if(soundController == null)
        {
            PSoundController.PlaySound("plip");
        }
        else
        {
            soundController.PlaySound("plip");
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
        if(soundController == null)
        {
            PSoundController.PlaySound("plip");
        }
        else
        {
            soundController.PlaySound("plip");
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
