using System.Collections;
using UnityEngine;

public class SetDeskNum : MonoBehaviour
{
    private Transform tiles;
    public bool isReadyForRNG = false;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        
        StartCoroutine(Loop());
    }
    private IEnumerator Loop() //wait until objects load
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            
            int i = 0;
            foreach(Transform obj in tiles.Find("GroundObjects"))
            {
                i++;
            }

            if(i == 0)
            {
                continue;
            }
            else
            {
                SetNums();
                break;
            }
        }
    }
    private void SetNums()
    {
        StopCoroutine(Loop());

        int i = 0;
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.CompareTag("Desk"))
            {
                if(obj.name == "PlayerDesk")
                {
                    obj.GetComponent<DeskData>().inmateCorrelationNumber = -1;
                }
                else
                {
                    obj.GetComponent<DeskData>().inmateCorrelationNumber = i;
                    i++;
                }
            }
        }
        isReadyForRNG = true;
    }
}
