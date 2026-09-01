using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FanController : MonoBehaviour
{//im your biggest fan!!!~~~
    private MouseCollisionOnItems mcs;
    private Transform tiles;
    private List<string> objLayers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private Transform player;
    private void Start()
    {
        Transform so = RootObjectCache.GetRoot("ScriptObject").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        TurnAllFansOn();
    }
    private void Update()
    {
        if(mcs.isTouchingFanSwitch && Input.GetMouseButtonDown(0))
        {
            float distance = Vector2.Distance(player.position, mcs.touchedFanSwitch.transform.position);
            if(distance <= 2.4f)
            {
                FlipFanSwitch(mcs.touchedFanSwitch);
            }
        }
    }
    public void TurnAllFansOff() //for generator swtiching.
    {
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(objLayers[i]))
            {
                if(obj.name == "Wind")
                {
                    Destroy(obj.gameObject);
                }
            }
        }
    }
    public void TurnAllFansOn() //for generator switching (doesnt affect fans where isOn = false
    {
        List<GameObject> fansToSwitch = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(objLayers[i]))
            {
                if(obj.name == "Fan")
                {
                    FanHandler handler = obj.GetComponent<FanHandler>();
                    if(handler.isOn)
                    {
                        fansToSwitch.Add(obj.gameObject);
                    }
                }
            }
        }

        foreach(GameObject fan in fansToSwitch)
        {
            int windLength = fan.GetComponent<FanHandler>().windLength;
            for (int i = 0; i < windLength; i++)
            {
                GameObject windObj = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Wind"));
                windObj.name = "Wind";
                windObj.GetComponent<FanHandler>().assignNum = fan.GetComponent<FanHandler>().assignNum;
                string dir = fan.GetComponent<FanHandler>().direction;

                Vector3 offset = Vector3.zero;
                switch (dir)
                {
                    case "down":
                        offset = new Vector3(0, -1.6f);
                        break;
                    case "up":
                        offset = new Vector3(0, 1.6f);
                        break;
                    case "left":
                        offset = new Vector3(-1.6f, 0);
                        break;
                    case "right":
                        offset = new Vector3(1.6f, 0);
                        break;
                }

                windObj.transform.position = fan.transform.position + offset;
                Vector3 offsetMultiplied = new Vector3(offset.x * i, offset.y * i);
                windObj.transform.position += offsetMultiplied;
            }
        }
    }
    public void FlipFanSwitch(GameObject fanSwitch)
    {
        int assignNum = fanSwitch.GetComponent<FanHandler>().assignNum;
        List<GameObject> fansToSwitch = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(objLayers[i]))
            {
                if(obj.name == "Fan" && obj.GetComponent<FanHandler>() != null && obj.GetComponent<FanHandler>().assignNum == assignNum)
                {
                    fansToSwitch.Add(obj.gameObject);
                }
            }
        }

        bool isOn = fanSwitch.GetComponent<FanHandler>().isOn;
        bool shouldTurnOn = !isOn;
        foreach(GameObject fan in fansToSwitch)
        {
            fan.GetComponent<FanHandler>().isOn = shouldTurnOn;
            if (shouldTurnOn)
            {
                foreach (Transform obj in fan.transform.parent)
                {
                    if (obj.name == "Wind" && obj.GetComponent<FanHandler>() != null && obj.GetComponent<FanHandler>().assignNum == assignNum)
                    {
                        Destroy(obj.gameObject);
                    }
                }

                int windLength = fan.GetComponent<FanHandler>().windLength;
                for(int i = 0; i < windLength; i++)
                {
                    GameObject windObj = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Wind"));
                    windObj.name = "Wind";
                    windObj.GetComponent<FanHandler>().assignNum = assignNum;
                    string dir = fan.GetComponent<FanHandler>().direction;

                    Vector3 offset = Vector3.zero;
                    switch (dir)
                    {
                        case "down":
                            offset = new Vector3(0, -1.6f);
                            break;
                        case "up":
                            offset = new Vector3(0, 1.6f);
                            break;
                        case "left":
                            offset = new Vector3(-1.6f, 0);
                            break;
                        case "right":
                            offset = new Vector3(1.6f, 0);
                            break;
                    }

                    windObj.transform.position = fan.transform.position + offset;
                    Vector3 offsetMultiplied = new Vector3(offset.x * i, offset.y * i);
                    windObj.transform.position += offsetMultiplied;
                }
            }
            else
            {
                foreach(Transform obj in fan.transform.parent)
                {
                    if(obj.name == "Wind" && obj.GetComponent<FanHandler>() != null && obj.GetComponent<FanHandler>().assignNum == assignNum)
                    {
                        Destroy(obj.gameObject);
                    }
                }
            }
        }
    }
}
