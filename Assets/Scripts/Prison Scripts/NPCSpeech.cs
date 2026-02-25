using Ookii.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpeech : MonoBehaviour
{
    private Schedule scheduleScript;
    private string period;
    public bool isTalking;
    private string[] speechFile;
    private MouseCollisionOnItems mcs;
    private bool isDestroying;
    private bool isWaiting;
    private bool textBoxIsActive;
    private bool madeTextBox;
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        speechFile = Resources.Load<TextAsset>("Speech").text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); ;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();

        DestroyTextBox(transform);
        StartCoroutine(SpeechLoop());
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isTalking && !textBoxIsActive && mcs.isTouchingNPC && mcs.touchedNPC == gameObject && (mcs.touchedNPC.name.StartsWith("Inmate") || mcs.touchedNPC.name.StartsWith("Guard")) && !mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead)
        {
            isTalking = true;
            Talk();
        }

        //check if the text box is actually active
        if (transform.Find("SpeechCanvas").Find("SpeechBackground").gameObject.activeInHierarchy)
        {
            textBoxIsActive = true;
        }
        else
        {
            textBoxIsActive = false;
        }

        //destroy textbox every two seconds
        if (textBoxIsActive && !isDestroying)
        {
            StartCoroutine(DestroyWait());
        }
    }
    private void Talk()
    {
        string messageType = null;
        if (name.StartsWith("Guard"))
        {
            messageType = "Guard_Talk";
        }
        else if (name.StartsWith("Inmate"))
        {
            int opn = GetComponent<NPCCollectionData>().npcData.opinion;
            if(opn < 33)
            {
                messageType = "Rep_1";
            }
            else if(opn > 33 && opn < 66)
            {
                messageType = "Rep_2";
            }
            else if(opn > 66)
            {
                messageType = "Rep_3";
            }
        }
        StartCoroutine(MakeTextBox(GetMessage(messageType), transform, false));
    }
    private IEnumerator SpeechLoop()
    {
        while (true)
        {
            if (!isTalking && !isWaiting && !textBoxIsActive)
            {
                isWaiting = true;
                float rand = UnityEngine.Random.Range(3f, 5f);
                yield return new WaitForSeconds(rand);
                isWaiting = false;
            }
            else
            {
                yield return null;
                continue;
            }

            period = scheduleScript.periodCode;

            string messageType = null;

            if (name.StartsWith("Inmate"))
            {
                switch (period)
                {
                    case "B":
                    case "L":
                    case "D":
                        messageType = "Canteen";
                        break;
                    case "E":
                        messageType = "Gym";
                        break;
                    case "FT":
                    case "W":
                        messageType = "Banter";
                        break;
                    case "S":
                        messageType = "Shower";
                        break;
                }
            }
            else if(name == "JobOfficer")
            {
                messageType = "JobStaff";
            }
            else if(name == "Medic")
            {
                messageType = "MedStaff";
            }
            else if(name == "Warden")
            {
                messageType = "Warden";
            }

            if (!isTalking && messageType != null && !textBoxIsActive)
            {
                Debug.Log("Making Text Box");
                StartCoroutine(MakeTextBox(GetMessage(messageType), transform, false));
            }
            yield return null;
        }
    }
    public IEnumerator MakeTextBox(string msg, Transform npc, bool isMad)
    {
        madeTextBox = true;
        isTalking = true;

        List<int> breakPoints = new List<int>();
        int loopBreakNum = 0;
        for(int i = 0; i < msg.Length; i++)
        {
            if (msg[i] == ' ')
            {
                loopBreakNum++;
                if(loopBreakNum % 8 == 0)
                {
                    breakPoints.Add(i);
                }
            }
        }
        string newMsg = "";
        for(int i = 0; i < breakPoints.Count; i++)
        {
            breakPoints[i] += i;
        }
        for(int i = 0; i < msg.Length; i++)
        {
            if (breakPoints.Contains(i))
            {
                newMsg += "\n";
            }
            newMsg += msg[i];
        }

        npc.Find("SpeechCanvas").Find("Text").GetComponent<TextMeshProUGUI>().text = newMsg;
        npc.Find("SpeechCanvas").Find("Text").GetComponent<TextMeshProUGUI>().color = Color.clear;
        npc.Find("SpeechCanvas").Find("Text").gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(npc.Find("SpeechCanvas").Find("Text").GetComponent<RectTransform>());
        yield return null;

        float borderWidth = npc.Find("SpeechCanvas").Find("Text").GetComponent<RectTransform>().sizeDelta.x;
        float borderHeight = npc.Find("SpeechCanvas").Find("Text").GetComponent<RectTransform>().sizeDelta.y;

        RectTransform bottomRT = npc.Find("SpeechCanvas").Find("BottomSpeechBorder").GetComponent<RectTransform>();
        bottomRT.sizeDelta = new Vector2(borderWidth, .1f);
        bottomRT.anchoredPosition = new Vector2(0, .05f);
        RectTransform topRT = npc.Find("SpeechCanvas").Find("TopSpeechBorder").GetComponent<RectTransform>();
        topRT.sizeDelta = new Vector2(borderWidth, .1f);
        topRT.anchoredPosition = new Vector2(0, (borderHeight) + .05f);
        RectTransform leftRT = npc.Find("SpeechCanvas").Find("LeftSpeechBorder").GetComponent<RectTransform>();
        leftRT.sizeDelta = new Vector2(.1f, (borderHeight) + .1f);
        leftRT.anchoredPosition = new Vector2(((-borderWidth) / 2f) - .05f, ((borderHeight) / 2f) + .05f);
        RectTransform rightRT = npc.Find("SpeechCanvas").Find("RightSpeechBorder").GetComponent<RectTransform>();
        rightRT.sizeDelta = new Vector2(.1f, (borderHeight) + .1f);
        rightRT.anchoredPosition = new Vector2(((borderWidth) / 2f) + .05f, ((borderHeight) / 2f) + .05f);

        RectTransform backgroundRT = npc.Find("SpeechCanvas").Find("SpeechBackground").GetComponent<RectTransform>();
        backgroundRT.sizeDelta = new Vector2(borderWidth, borderHeight);
        backgroundRT.anchoredPosition = new Vector2(0, (borderHeight    ) / 2f);

        RectTransform textRT = npc.Find("SpeechCanvas").Find("Text").GetComponent<RectTransform>();
        textRT.anchoredPosition = backgroundRT.anchoredPosition;

        if (npc.name.StartsWith("Guard"))
        {
            npc.Find("SpeechCanvas").Find("SpeechBackground").GetComponent<Image>().sprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/GuardSpeechBackground");
        }

        if (isMad && npc.name.StartsWith("Guard"))
        {
            npc.Find("SpeechCanvas").Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(157f / 255f, 0f, 0f);
        }
        else
        {
            npc.Find("SpeechCanvas").Find("Text").GetComponent<TextMeshProUGUI>().color = Color.black;
        }

        foreach (Transform obj in npc.Find("SpeechCanvas"))
        {
            obj.gameObject.SetActive(true);
        }
        npc.Find("SpeechCanvas").Find("Text").GetComponent<TextMeshProUGUI>().color = Color.black;
    }
    private IEnumerator DestroyWait()
    {
        isDestroying = true;
        float timer = 0f;
        while (timer < 2f)
        {
            timer += Time.deltaTime;

            if(madeTextBox == true)
            {
                timer = 0f;
                madeTextBox = false;
            }
            yield return null;
        }
        DestroyTextBox(transform);
        isTalking = false;
        isDestroying = false;
    }
    public void DestroyTextBox(Transform npc)
    {
        foreach(Transform obj in npc.Find("SpeechCanvas"))
        {
            obj.gameObject.SetActive(false);
        }
    }
    private int GetLength(string msg)
    {
        char[] charArr = msg.ToCharArray();
        int length = 0;
        Dictionary<char, int> charWidths = new Dictionary<char, int>
        {
            {'A', 5}, {'B', 5}, {'C', 5}, {'D', 5}, {'E', 5}, {'F', 5}, {'G', 5}, {'H', 5}, {'I', 5}, {'J', 5}, {'K', 5}, {'L', 5}, {'M', 7}, {'N', 5}, {'O', 5}, {'P', 5}, {'Q', 5}, {'R', 5}, {'S', 5}, {'T', 5}, {'U', 5}, {'V', 5}, {'W', 7}, {'X', 5}, {'Y', 5}, {'Z', 5},
            {'a', 5}, {'b', 5}, {'c', 5}, {'d', 5}, {'e', 5}, {'f', 4}, {'g', 5}, {'h', 5}, {'i', 1}, {'j', 3}, {'k', 5}, {'l', 2}, {'m', 5}, {'n', 5}, {'o', 5}, {'p', 5}, {'q', 5}, {'r', 5}, {'s', 5}, {'t', 3}, {'u', 5}, {'v', 5}, {'w', 5}, {'x', 5}, {'y', 5}, {'z', 5},
            {'0', 5}, {'1', 3}, {'2', 5}, {'3', 5}, {'4', 5}, {'5', 5}, {'6', 5}, {'7', 5}, {'8', 5}, {'9', 5},
            {',', 2}, {':', 1}, {';', 2}, {'\'', 1}, {'"', 3}, {'!', 1}, {'?', 5}, {'(', 3}, {')', 3}, {'+', 5}, {'-', 5}, {'_', 7}, {'*', 5}, {'/', 3}, {'=', 5}, {'@', 7}, {'#', 5}, {'$', 5}, {'%', 7}, {'^', 5}, {'&', 5}, {'`', 2}, {'~', 5},
            {'[', 3}, {']', 3}, {'{', 4}, {'}', 4}, {'\\', 3}, {'|', 1}, {'<', 5}, {'>', 5}, {' ', 1}, {'.', 1}
        };
        foreach(char c in charArr)
        {
            length += charWidths[c];
            length += 1;
        }

        return length;
    }
    public string GetMessage(string messageType)
    {
        int count = Convert.ToInt32(GetINIVar(messageType, "Count", speechFile));
        int rand = UnityEngine.Random.Range(1, count + 1);
        return GetINIVar(messageType, rand.ToString(), speechFile);
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
