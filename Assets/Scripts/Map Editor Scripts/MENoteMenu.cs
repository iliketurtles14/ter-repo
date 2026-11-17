using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MENoteMenu : MonoBehaviour
{
    private int currentNote = 0;
    public Transform uic;
    private Picker pickerScript;
    private List<Transform> notePanels;
    private void Start()
    {
        pickerScript = uic.Find("NotePanel").Find("OptionsPanel").Find("NotePicker").GetComponent<Picker>();
        notePanels = new List<Transform>
        {
            uic.Find("NotePanel").Find("NormalNote"), uic.Find("NotePanel").Find("NoSigNote"),
            uic.Find("NotePanel").Find("DTAFNote"), uic.Find("NotePanel").Find("ETNote")
        };
    }
    private void Update()
    {
        if(currentNote != pickerScript.currentIndex)
        {
            currentNote = pickerScript.currentIndex;
            DisableAllPanels();
            notePanels[currentNote].gameObject.SetActive(true);
        }
    }
    private void DisableAllPanels()
    {
        foreach(Transform panel in notePanels)
        {
            panel.gameObject.SetActive(false);
        }
    }
}
