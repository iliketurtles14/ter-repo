using System.Collections;
using UnityEngine;

public class SeatNumberSet : MonoBehaviour
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

        SetSeatNumbers();
    }
    private void SetSeatNumbers()
    {
        int i = 1;
        foreach(Transform seat in tiles.Find("GroundObjects"))
        {
            if(seat.name == "Seat")
            {
                seat.GetComponent<SeatNumber>().seatNumber = i;
                i++;
            }
        }
    }
}
