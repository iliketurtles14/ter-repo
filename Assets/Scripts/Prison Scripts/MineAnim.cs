using System.Collections;
using UnityEngine;

public class MineAnim : MonoBehaviour
{
    private Sprite s1;
    private Sprite s2;
    private void OnEnable()
    {
        s1 = DataSender.instance.PrisonObjectImages[243];
        s2 = DataSender.instance.PrisonObjectImages[244];

        StartCoroutine(AnimCycle());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator AnimCycle()
    {
        while (true)
        {
            GetComponent<SpriteRenderer>().sprite = s1;
            yield return new WaitForSeconds(.55f);
            GetComponent<SpriteRenderer>().sprite = s2;
            yield return new WaitForSeconds(.55f);
        }
    }
}
