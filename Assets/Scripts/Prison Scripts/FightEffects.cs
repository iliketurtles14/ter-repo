using System.Collections;
using UnityEngine;

public class FightEffects : MonoBehaviour
{
    private Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
    }
    public IEnumerator MakeScreenShake()
    {
        Vector3 rand1 = new Vector3(UnityEngine.Random.Range(.1f, .5f), UnityEngine.Random.Range(.1f, .5f));
        Vector3 rand2 = new Vector3(UnityEngine.Random.Range(.1f, .5f), UnityEngine.Random.Range(.1f, .5f));
        int rand = UnityEngine.Random.Range(0, 2);
        if(rand == 0)
        {
            rand1 *= -1;
        }
        else if(rand == 1)
        {
            rand2 *= -1;
        }

        mainCam.transform.position += rand1;
        yield return new WaitForSeconds(.05f);
        mainCam.transform.position += rand2;
    }
    public IEnumerator MakeStar(Vector2 pos)
    {
        //.083
        DataSender ds = DataSender.instance;
        GameObject starObj = new GameObject("Star");
        starObj.AddComponent<SpriteRenderer>().sprite = ds.UIImages[245];
        SpriteRenderer sr = starObj.GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = new Vector2(1.6f, 1.6f);
        sr.color += new Color(0, 0, -.5f);
        sr.sortingOrder = 100;
        starObj.transform.position = pos;
        yield return new WaitForSeconds(.083f);
        sr.sprite = ds.UIImages[246];
        yield return new WaitForSeconds(.083f);
        sr.sprite = ds.UIImages[247];
        yield return new WaitForSeconds(.083f);
        Destroy(starObj);
    }
}
