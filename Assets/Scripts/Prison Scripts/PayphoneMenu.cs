using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayphoneMenu : MonoBehaviour
{
    private Map currentMap;
    public string tip1;
    public string tip2;
    public string tip3;
    private int baseCost;
    private Transform mc;
    private PauseController pc;
    private bool inMenu;
    private Transform player;
    private MouseCollisionOnItems mcs;
    private bool ready;
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        player = RootObjectCache.GetRoot("Player").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        CloseMenu();
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
        GetTipsAndCost();
        ready = true;
    }
    private void GetTipsAndCost()
    {
        baseCost = (currentMap.npcLevel * 10) + 5;
        tip1 = Regex.Unescape(currentMap.hint1);
        tip2 = Regex.Unescape(currentMap.hint2);
        tip3 = Regex.Unescape(currentMap.hint3);

        Debug.Log(tip1);

        transform.Find("Tip1Button").Find("Text").GetComponent<TextMeshProUGUI>().text = "Locked ($" + baseCost + ")";
        transform.Find("Tip2Button").Find("Text").GetComponent<TextMeshProUGUI>().text = "Locked ($" + (baseCost + 10) + ")";
        transform.Find("Tip3Button").Find("Text").GetComponent<TextMeshProUGUI>().text = "Locked ($" + (baseCost + 20) + ")";
    }
    private void OpenMenu()
    {
        mc.Find("Black").GetComponent<Image>().enabled = true;
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("TitleText").gameObject.SetActive(true);
        transform.Find("ExplainText").gameObject.SetActive(true);
        transform.Find("Tip1Button").gameObject.SetActive(true);
        transform.Find("Tip2Button").gameObject.SetActive(true);
        transform.Find("Tip3Button").gameObject.SetActive(true);
        inMenu = true;
        pc.Pause(true);
    }
    private void CloseMenu()
    {
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("TitleText").gameObject.SetActive(false);
        transform.Find("ExplainText").gameObject.SetActive(false);
        transform.Find("Tip1Button").gameObject.SetActive(false);
        transform.Find("Tip2Button").gameObject.SetActive(false);
        transform.Find("Tip3Button").gameObject.SetActive(false);
        transform.Find("ReturnButton").gameObject.SetActive(false);
        transform.Find("TipText").gameObject.SetActive(false);
        inMenu = false;
    }
    public void BuyTip(int hintNum)
    {
        Debug.Log(hintNum);
        
        int price = ((hintNum - 1) * 10) + baseCost;
        if(player.GetComponent<PlayerCollectionData>().playerData.money < price)
        {
            return;
        }
        player.GetComponent<PlayerCollectionData>().playerData.money -= price;

        TextMeshProUGUI tmp = null;
        string hint = null;
        switch (hintNum)
        {
            case 1:
                hint = tip1;
                tmp = transform.Find("Tip1Button").Find("Text").GetComponent<TextMeshProUGUI>();
                break;
            case 2:
                hint = tip2;
                tmp = transform.Find("Tip2Button").Find("Text").GetComponent<TextMeshProUGUI>();
                break;
            case 3:
                hint = tip3;
                tmp = transform.Find("Tip3Button").Find("Text").GetComponent<TextMeshProUGUI>();
                break;

        }
        tmp.text = "Unlocked";
        tmp.color = new Color(18f / 255f, 128f / 255f, 0f);

        GoToTip(hint);
    }
    public void GoToTip(string hint)
    {
        transform.Find("TipText").GetComponent<TextMeshProUGUI>().text = hint;
        
        transform.Find("TitleText").gameObject.SetActive(true);
        transform.Find("ExplainText").gameObject.SetActive(false);
        transform.Find("Tip1Button").gameObject.SetActive(false);
        transform.Find("Tip2Button").gameObject.SetActive(false);
        transform.Find("Tip3Button").gameObject.SetActive(false);
        transform.Find("ReturnButton").gameObject.SetActive(true);
        transform.Find("TipText").gameObject.SetActive(true);
    }
    public void ReturnFromTip()
    {
        transform.Find("TitleText").gameObject.SetActive(true);
        transform.Find("ExplainText").gameObject.SetActive(true);
        transform.Find("Tip1Button").gameObject.SetActive(true);
        transform.Find("Tip2Button").gameObject.SetActive(true);
        transform.Find("Tip3Button").gameObject.SetActive(true);
        transform.Find("ReturnButton").gameObject.SetActive(false);
        transform.Find("TipText").gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if (!inMenu)
        {
            if(mcs.isTouchingPayphone && Input.GetMouseButtonDown(0))
            {
                float distance = Vector2.Distance(player.position, mcs.touchedPayphone.transform.position);
                if(distance <= 2.4f)
                {
                    OpenMenu();
                }
            }
        }
        else
        {
            if(!mcs.isTouchingInvSlot && !mcs.isTouchingIDPanel && !mcs.isTouchingExtra && !mcs.isTouchingPayphone && !mcs.isTouchingButton && Input.GetMouseButtonDown(0))
            {
                CloseMenu();
            }
        }
    }
}
