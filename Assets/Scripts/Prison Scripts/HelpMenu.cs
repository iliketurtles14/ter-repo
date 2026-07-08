using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
    public bool atButtons;
    public bool isOpen;
    private Transform mc;
    private List<string> helpText = new List<string>();
    private Map currentMap;
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
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
        SetText();
        Close(true);
    }
    private void SetText()
    {
        for(int i = 0; i < 20; i++)
        {
            helpText.Add(GetINIVar("Help_Me_Please", i.ToString(), currentMap.speech));
            transform.Find("ButtonGrid").Find(i.ToString()).Find("Text").GetComponent<TextMeshProUGUI>().text = GetINIVar("Help_Buttons", i.ToString(), currentMap.speech);
        }
    }
    public void Open()
    {
        foreach(Transform child in transform)
        {
            if(child.name != "HelpBodyText")
            {
                child.gameObject.SetActive(true);
            }
        }
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        mc.Find("PauseMenuPanel").GetComponent<Pause>().ClosePauseMenu(true);
        isOpen = true;
        atButtons = true;
    }
    public void Close(bool atStart)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        if (!atStart)
        {
            mc.Find("PauseMenuPanel").GetComponent<Pause>().OpenPauseMenu();
        }
        isOpen = false;
    }
    public void Switch(int num)//num corresponds to the 20 help buttons. if num = -1, its going back to the buttons screen
    {
        if(num == -1)
        {
            transform.Find("HelpBodyText").gameObject.SetActive(false);
            transform.Find("ButtonGrid").gameObject.SetActive(true);
            atButtons = true;
        }
        else
        {
            transform.Find("HelpBodyText").GetComponent<TextMeshProUGUI>().text = helpText[num].Replace("#", "\n");
            transform.Find("HelpBodyText").gameObject.SetActive(true);
            transform.Find("ButtonGrid").gameObject.SetActive(false);
            atButtons = false;
        }
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }

        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }
}
