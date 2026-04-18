using System.Collections;
using UnityEngine;

public class ItemSwell : MonoBehaviour
{
    public IEnumerator Swell(GameObject slot)
    {
        RectTransform rt = slot.GetComponent<RectTransform>();
        float baseScale = rt.localScale.x;
        float time = 0f;
        while(time <= .2f)
        {
            float scaleFactor = (-5f * time) + (2f * baseScale); // s(t) = -5t + 2a, where a is the initial scaling of the slot
            rt.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            yield return null;
            time += Time.deltaTime;
        }
        rt.localScale = new Vector3(baseScale, baseScale, 1);
    }
}
