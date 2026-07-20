using System.Collections;
using UnityEngine;

public class IDNPCName : MonoBehaviour
{
    public string aName;
    public Color color;
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
        if(name == "Player")
        {
            aName = RootObjectCache.GetRoot("Player").GetComponent<PlayerCollectionData>().playerData.displayName.Replace("\n", "").Replace("\r", "");
            //234 140
            color = new Color(234f / 255f, 140f / 255f, 0);
        }
    }
}
