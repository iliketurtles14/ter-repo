using UnityEngine;
using UnityEngine.UI;

public class SleepMenu : MonoBehaviour
{
    private Schedule scheduleScript;
    private Routine routineScript;
    private bool canSleep;
    private MouseCollisionOnItems mcs;
    private Transform player;
    private bool menuIsOpen;
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Schedule>();
        routineScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Routine>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
    }
    private void Update()
    {
        if(scheduleScript.periodCode == "LO" && routineScript.min != routineScript.startingMin)
        {
            canSleep = true;
        }
        else
        {
            canSleep = false;
        }

        if (!canSleep)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0) && mcs.isTouchingSittable && mcs.touchedSittable.name.Contains("PlayerBed"))
        {
            float distance = Vector2.Distance(player.position, mcs.touchedSittable.transform.position);
            if(distance > 2.4f)
            {
                return;
            }
            Open();
        }
    }
    public void Open()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        menuIsOpen = true;
    }
    public void Close()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        menuIsOpen = false;
    }
    public void Sleep()
    {
        Close();

        //fade screen
        //save
        //set time
        //send inmates to beds
        //keep it niche
        //put sheets and pillows back on beds tsssss
        //advance day (i think do that at a time script or something)
        //unfade


    }
}
