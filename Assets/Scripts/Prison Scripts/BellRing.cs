using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BellRing : MonoBehaviour
{
    private Image bellImage;
    private Sprite s1;
    private Sprite s2;
    private TextMeshProUGUI tmp;
    private void Start()
    {
        bellImage = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Bell").GetComponent<Image>();
        s1 = Resources.Load<Sprite>("PrisonResources/UI Stuff/bell1");
        s2 = Resources.Load<Sprite>("PrisonResources/UI Stuff/bell2");
        tmp = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<TextMeshProUGUI>();
    }
    public void RingBell()
    {
        StartCoroutine(BellAnim());
        PSoundController.PlaySound("bell");
    }
    private IEnumerator BellAnim()
    {
        tmp.color = new Color(1, 1, 0);
        bellImage.gameObject.SetActive(true);
        for (int i = 0; i < 22; i++)
        {
            bellImage.sprite = s1;
            yield return new WaitForSeconds(1f / 45f);
            bellImage.sprite = s2;
            yield return new WaitForSeconds(1f / 45f);
        }
        bellImage.gameObject.SetActive(false);
        tmp.color = new Color(191f / 255f, 191f / 255f, 191f / 255f);
    }
}
