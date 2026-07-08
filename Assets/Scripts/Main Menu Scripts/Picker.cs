using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Picker : MonoBehaviour
{
    public List<string> options = new List<string>();
    private TextMeshProUGUI tmp;
    public int currentIndex;
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
        if(currentIndex > 0 && options.Count > 0)
        {
            currentIndex--;
        }
    }
    public void PickerRight()
    {
        if(currentIndex < options.Count - 1 && options.Count > 0)
        {
            currentIndex++;
        }
    }
}
