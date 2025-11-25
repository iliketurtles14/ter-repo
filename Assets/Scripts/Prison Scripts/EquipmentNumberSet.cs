using System.Collections;
using UnityEngine;

public class EquipmentNumberSet : MonoBehaviour
{
    private Transform tiles;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        SetEquipmentNumbers();
    }
    private void SetEquipmentNumbers()
    {
        int i = 1;
        foreach(Transform equipment in tiles.Find("GroundObjects"))
        {
            if (equipment.gameObject.CompareTag("Equipment"))
            {
                equipment.GetComponent<EquipmentNumber>().equipmentNumber = i;
                i++;
            }
        }
    }
}
