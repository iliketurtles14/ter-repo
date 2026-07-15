using System;
using System.Collections;
using UnityEngine;

public class WorkDoorContainer : MonoBehaviour
{
    public string job;
    private Map currentMap;
    private void Start()
    {
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
        if (gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            foreach (string line in currentMap.groundObjectProperties)
            {
                string objName = line.Split("=")[0];
                if (objName != "WorkDoor")
                {
                    continue;
                }

                string rawPos = line.Split("=")[1].Split(";")[0];
                string aJob = line.Split(";")[1];
                float posX = Convert.ToInt32(rawPos.Split(",")[0]) * 1.6f;
                float posY = Convert.ToInt32(rawPos.Split(",")[1]) * 1.6f;
                posX -= 1.6f;
                posY -= 1.6f;

                Debug.Log(posX.ToString() + ", " + posY.ToString());

                if (transform.position == new Vector3(posX, posY, 0))
                {
                    job = aJob;
                    break;
                }
            }
        }
        if (gameObject.layer == LayerMask.NameToLayer("Underground"))
        {
            foreach (string line in currentMap.undergroundObjectProperties)
            {
                string objName = line.Split("=")[0];
                if (objName != "WorkDoor")
                {
                    continue;
                }

                string rawPos = line.Split("=")[1].Split(";")[0];
                string aJob = line.Split(";")[1];
                float posX = Convert.ToInt32(rawPos.Split(",")[0]) * 1.6f;
                float posY = Convert.ToInt32(rawPos.Split(",")[1]) * 1.6f;

                if (transform.position == new Vector3(posX, posY, 0))
                {
                    job = aJob;
                    break;
                }
            }
        }
        if (gameObject.layer == LayerMask.NameToLayer("Vents"))
        {
            foreach (string line in currentMap.ventObjectProperties)
            {
                string objName = line.Split("=")[0];
                if (objName != "WorkDoor")
                {
                    continue;
                }

                string rawPos = line.Split("=")[1].Split(";")[0];
                string aJob = line.Split(";")[1];
                float posX = Convert.ToInt32(rawPos.Split(",")[0]) * 1.6f;
                float posY = Convert.ToInt32(rawPos.Split(",")[1]) * 1.6f;

                if (transform.position == new Vector3(posX, posY, 0))
                {
                    job = aJob;
                    break;
                }
            }
        }
        if (gameObject.layer == LayerMask.NameToLayer("Roof"))
        {
            foreach (string line in currentMap.roofObjectProperties)
            {
                string objName = line.Split("=")[0];
                if (objName != "WorkDoor")
                {
                    continue;
                }

                string rawPos = line.Split("=")[1].Split(";")[0];
                string aJob = line.Split(";")[1];
                float posX = Convert.ToInt32(rawPos.Split(",")[0]) * 1.6f;
                float posY = Convert.ToInt32(rawPos.Split(",")[1]) * 1.6f;

                if (transform.position == new Vector3(posX, posY, 0))
                {
                    job = aJob;
                    break;
                }
            }
        }
    }
}
