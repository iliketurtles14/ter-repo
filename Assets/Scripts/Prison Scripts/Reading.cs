using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Reading : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private ItemBehaviours itemBehavioursScript;
    private HPAChecker HPAScript;
    private Vector3 oldPos;
    private bool isReading = false;
    private bool stopReading = false;
    private bool isBusy;
    private StatEffects statEffectsScript;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        itemBehavioursScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemBehaviours>();
        HPAScript = RootObjectCache.GetRoot("Player").GetComponent<HPAChecker>();
        statEffectsScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<StatEffects>();
    }
    public void Update()
    {
        HPAScript.isReading = isReading;
        isBusy = HPAScript.isBusy;

        if(!isBusy && Input.GetMouseButtonDown(0) && mcs.isTouchingReader && GetComponent<PlayerCollectionData>().playerData.energy < 100 && !isReading)
        {
            float distance = Vector2.Distance(transform.position, mcs.touchedReader.transform.position);
            if (distance <= 2.4f)
            {
                oldPos = transform.position;
                PSoundController.PlaySound("open");
                StartCoroutine(Read(mcs.touchedReader.name));
            }
        }
        if (isReading && Vector2.Distance(oldPos, transform.position) > .1f)
        {
            stopReading = true;
            isReading = false;
        }
    }
    public IEnumerator Read(string readerName)
    {
        isReading = true;
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, true));
        int rand = UnityEngine.Random.Range(0, 100);
        if(rand == 0 && readerName == "ComputerTable")
        {
            itemBehavioursScript.CreateActionText("LOLing at cats");
        }
        else if(readerName == "ComputerTable")
        {
            itemBehavioursScript.CreateActionText("Browsing");
        }
        else
        {
            itemBehavioursScript.CreateActionText("Reading");
        }
        for (int i = 0; i < 49; i++)
        {
            if (stopReading)
            {
                yield break;
            }
            yield return new WaitForSeconds(.045f);
        }
        GetComponent<PlayerCollectionData>().playerData.intellect++;
        StartCoroutine(statEffectsScript.MakeEffect(transform, "intellect", GetComponent<SpriteRenderer>().sortingLayerName));
        GetComponent<PlayerCollectionData>().playerData.energy += 5;
        PSoundController.PlaySound("open");
        isReading = false;
    }
}
