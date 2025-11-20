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
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        itemBehavioursScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemBehaviours>();
        HPAScript = RootObjectCache.GetRoot("Player").GetComponent<HPAChecker>();
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
                StartCoroutine(Read());
            }
        }
        if (isReading && oldPos != transform.position)
        {
            stopReading = true;
        }
    }
    public IEnumerator Read()
    {
        isReading = true;
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, true));
        itemBehavioursScript.CreateActionText("asdf");
        for (int i = 0; i < 49; i++)
        {
            if (stopReading)
            {
                yield break;
            }
            yield return new WaitForSeconds(.045f);
        }
        GetComponent<PlayerCollectionData>().playerData.intellect++;
        GetComponent<PlayerCollectionData>().playerData.energy += 5;
        isReading = false;
    }
}
