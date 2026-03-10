using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraNPCAnimation : MonoBehaviour
{
    public string xLookDir;
    public string yLookDir;
    public string lookDir;
    private int lookNum;
    private float xDif;
    private float yDif;
    private Vector2 oldPos;
    private Vector2 currentPos;
    private int whichCycle = 0;
    private ApplyPrisonData applyScript;
    private List<Sprite> medicSprites;
    private List<Sprite> jobOfficerSprites;
    private List<Sprite> wardenSprites;
    private List<Sprite> currentSprites;
    private void Start()
    {
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        medicSprites = applyScript.MedicSprites;
        jobOfficerSprites = applyScript.JobOfficerSprites;
        wardenSprites = applyScript.WardenSprites;

        switch (name)
        {
            case "Warden":
                currentSprites = wardenSprites;
                break;
            case "JobOfficer":
                currentSprites = jobOfficerSprites;
                break;
            case "Medic":
                currentSprites = medicSprites;
                break;
        }
    }
    private void OnEnable()
    {
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
                GetComponent<SpriteRenderer>().sprite = currentSprites[lookNum + whichCycle];
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
            DirGet();
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
