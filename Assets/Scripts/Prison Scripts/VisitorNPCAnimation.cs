using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VisitorNPCAnimation : MonoBehaviour
{
    private ApplyPrisonData applyScript;
    public string xLookDir;
    public string yLookDir;
    public string lookDir;
    private int lookNum;
    private float xDif;
    private float yDif;
    private Vector2 oldPos;
    private Vector2 currentPos;
    private int whichCycle = 0;
    private List<Sprite> visitorSprites;
    private bool isSS;
    private bool isCustom;
    private Map currentMap;
    private int charNum;
    private bool ready;
    private VisitorNPCAI visitorNPCAIScript;
    private IniFile iniFile;
    List<Sprite> newVisitorSprites;
    private bool doSpecial;
    private void Start()
    {
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        visitorSprites = applyScript.VisitorSprites;
        visitorNPCAIScript = GetComponent<VisitorNPCAI>();
        iniFile = new IniFile(Path.Combine(Application.streamingAssetsPath, "UserData.ini"));

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
        isSS = currentMap.mapName == "Santa's Sweatshop";
        newVisitorSprites = new List<Sprite>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                newVisitorSprites.Add(Resources.Load<Sprite>("PrisonResources/FanmadeVisitorSprites/" + i.ToString() + "/" + j.ToString()));
            }
        }
        if (iniFile.Read("EnableNewVisitors", "Settings") == "True")
        {
            isCustom = true;
            foreach (Sprite sprite in newVisitorSprites)
            {
                visitorSprites.Add(sprite);
            }
        }
        ready = true;
    }
    private void OnEnable()
    {
        if (!ready)
        {
            return;
        }

        //get char
        doSpecial = false;
        if (isSS)
        {
            charNum = UnityEngine.Random.Range(8, 13);
        }
        else if (isCustom)
        {
            int rand = UnityEngine.Random.Range(0, 2);
            if(rand == 0)
            {
                charNum = UnityEngine.Random.Range(0, 8);
                if (isSS)
                {
                    charNum = UnityEngine.Random.Range(8, 13);
                }
            }
            else
            {
                charNum = UnityEngine.Random.Range(0, 6);
                doSpecial = true;
            }
        }
        else
        {
            charNum = UnityEngine.Random.Range(0, 8);
        }

        oldPos = currentPos = transform.position;
        if (string.IsNullOrEmpty(lookDir))
        {
            lookDir = "down";
        }
        StartCoroutine(DirWait());
        StartCoroutine(AnimCycle());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        if(lookDir != null)
        {
            switch (lookDir)
            {
                case "right": lookNum = 0; break;
                case "up": lookNum = 2; break;
                case "left": lookNum = 4; break;
                case "down": lookNum = 6; break;
            }

            try
            {
                if (doSpecial)
                {
                    GetComponent<SpriteRenderer>().sprite = newVisitorSprites[(lookNum + whichCycle) + (charNum * 8)];
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = visitorSprites[(lookNum + whichCycle) + (charNum * 8)];
                }
            }
            catch { }
        }
    }
    private IEnumerator AnimCycle()
    {
        while (true)
        {
            whichCycle = 0;
            yield return new WaitForSeconds(.266f);
            whichCycle = 1;
            yield return new WaitForSeconds(.266f);
        }
    }
    public IEnumerator DirWait()
    {
        while (true)
        {
            oldPos = transform.position;
            yield return new WaitForSeconds(.05f); //apparantly this is 20hz or something idk the ai said so
            currentPos = transform.position;
            if(visitorNPCAIScript.dirToLook == "any")
            {
                DirGet();
            }
            else
            {
                lookDir = visitorNPCAIScript.dirToLook;
            }
        }
    }
    public void DirGet()
    {
        xDif = currentPos.x - oldPos.x;
        yDif = currentPos.y - oldPos.y;

        float threshold = 0.02f;

        // If movement is below threshold, keep current lookDir (don't force "down")
        if (Mathf.Abs(xDif) < threshold && Mathf.Abs(yDif) < threshold)
        {
            return;
        }

        // Prefer horizontal movement (right/left). Use vertical only if horizontal movement is below threshold.
        if (Mathf.Abs(xDif) >= threshold)
        {
            lookDir = xDif > 0 ? "right" : "left";
            return;
        }

        if (Mathf.Abs(yDif) >= threshold)
        {
            lookDir = yDif > 0 ? "up" : "down";
        }
    }
}
