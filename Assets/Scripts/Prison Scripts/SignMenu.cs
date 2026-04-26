using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

public class SignMenu : MonoBehaviour //the parent of this object is ScriptObject because
                                      //this has to work with multiple sign varients
{
    private Transform tiles;
    private Map currentMap;
    private MouseCollisionOnItems mcs;
    private bool inMenu;
    private string currentSignType;
    private string[] mapData;
    private Transform mc;
    private PauseController pc;
    private bool ready = false;
    private Transform player;
    private List<string[]> objectProperties;             //so i dont have to copy code 4 times
    private List<string> layerNames = new List<string>() //so i dont have to copy code 4 times
    {
        "GroundObjects", "UndergroundObjects", "VentObjects", "RoofObjects"
    };
    private void Start()
    {
        pc = GetComponent<PauseController>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        player = RootObjectCache.GetRoot("Player").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        ready = true;

        currentMap = GetComponent<LoadPrison>().currentMap;

        objectProperties = new List<string[]>()
        {
            currentMap.groundObjectProperties,
            currentMap.undergroundObjectProperties,
            currentMap.ventObjectProperties,
            currentMap.roofObjectProperties
        };

        SetSignText();
        CloseMenu("white");
        CloseMenu("blue");
    }
    private void SetSignText()
    {
        for(int i = 0; i < 4; i++)
        {
            foreach (string line in objectProperties[i])
            {
                string objName = line.Split("=")[0];
                if (objName != "DTAFSign" && objName != "DTAFPlaque" && objName != "SSSign")
                {
                    continue;
                }

                string objDataStr = line.Split("=")[1];

                float objX = Convert.ToSingle(objDataStr.Split(";")[0].Split(",")[0]);
                float objY = Convert.ToSingle(objDataStr.Split(";")[0].Split(",")[1]);

                Vector2 objPos = new Vector2((objX * 1.6f) - 1.6f, (objY * 1.6f) - 1.6f);
                Debug.Log("pos: " + objPos);

                string header = Regex.Unescape(objDataStr.Split("{HEADER}:")[1].Split("{BODY}:")[0]);
                string body = Regex.Unescape(objDataStr.Split("{BODY}:")[1]);

                foreach (Transform sign in tiles.Find(layerNames[i]))
                {
                    if (sign.name == "DTAFSign" || sign.name == "DTAFPlaque" || sign.name == "SSSign")
                    {
                        float distance = Vector2.Distance(sign.position, objPos);
                        if (distance < .1f)
                        {
                            sign.GetComponent<SignData>().header = header;
                            sign.GetComponent<SignData>().body = body;
                            break;
                        }
                    }
                }

            }
        }
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if(mcs.isTouchingSign && Input.GetMouseButtonDown(0) && !inMenu)
        {
            float distance = Vector2.Distance(player.position, mcs.touchedSign.transform.position);
            if(distance <= 2.4f)
            {
                switch (mcs.touchedSign.name)
                {
                    case "DTAFSign":
                        currentSignType = "white";
                        break;
                    case "DTAFPlaque":
                    case "SSSign":
                        currentSignType = "blue";
                        break;
                }

                if (!String.IsNullOrEmpty(currentSignType))
                {
                    OpenMenu(currentSignType, mcs.touchedSign.GetComponent<SignData>().header, mcs.touchedSign.GetComponent<SignData>().body);
                }
            }
        }
        else if(!mcs.isTouchingInvSlot && !mcs.isTouchingIDPanel && !mcs.isTouchingExtra && !mcs.isTouchingSign && Input.GetMouseButtonDown(0) && inMenu)
        {
            CloseMenu(currentSignType);
        }
    }
    private void OpenMenu(string signType, string header, string body)
    {
        GameObject signMenu = null;
        switch (signType)
        {
            case "blue":
                signMenu = mc.Find("BlueSignMenuPanel").gameObject;
                break;
            case "white":
                signMenu = mc.Find("WhiteSignMenuPanel").gameObject;
                break;
        }
        if(signMenu == null)
        {
            return;
        }
        mc.Find("Black").GetComponent<Image>().enabled = true;
        signMenu.transform.Find("HeaderText").GetComponent<TextMeshProUGUI>().text = header;
        signMenu.transform.Find("BodyText").GetComponent<TextMeshProUGUI>().text = body;
        signMenu.SetActive(true);
        inMenu = true;
        pc.Pause(true);
    }
    private void CloseMenu(string signType)
    {
        GameObject signMenu = null;
        switch (signType)
        {
            case "blue":
                signMenu = mc.Find("BlueSignMenuPanel").gameObject;
                break;
            case "white":
                signMenu = mc.Find("WhiteSignMenuPanel").gameObject;
                break;
        }
        if (signMenu == null)
        {
            return;
        }
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        signMenu.SetActive(false);
        inMenu = false;
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
