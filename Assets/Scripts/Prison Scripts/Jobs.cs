using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jobs : MonoBehaviour
{
    private bool hasJob;
    private Schedule scheduleScript;
    private Transform ic;
    private Transform player;
    private bool isShowing;
    private bool doneWithJob;
    private string currentJob;
    private Map currentMap;
    private Sprite quotaSprite1; //brightest
    private Sprite quotaSprite2; //2nd brightest
    private Sprite quotaSprite3; //3rd brightest
    private Sprite quotaSprite4; //original
    private ApplyPrisonData applyScript;
    private Dictionary<string, int> jobPayDict = new Dictionary<string, int>()
    {
        { "Janitor", 5 }, { "Gardening", 10 }, { "Tailor", 15 }, { "Laundry", 20 },
        { "Library", 25 }, { "Kitchen", 30 }, { "Mailman", 35 }, { "Woodshop", 40 },
        { "Deliveries", 45 }, { "Metalshop", 50 }
    };
    private void Start()
    {
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        scheduleScript = ic.Find("Period").GetComponent<Schedule>();
        player = RootObjectCache.GetRoot("Player").transform;
        applyScript = GetComponent<ApplyPrisonData>();
        StartCoroutine(StartWait());
        HideQuotaPanel();
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        quotaSprite1 = applyScript.UISprites[89];
        quotaSprite2 = applyScript.UISprites[88];
        quotaSprite3 = applyScript.UISprites[86];
        quotaSprite4 = applyScript.UISprites[193];
        currentMap = GetComponent<LoadPrison>().currentMap;
    }
    private void Update()
    {
        currentJob = player.GetComponent<PlayerCollectionData>().playerData.job;
        hasJob = !String.IsNullOrEmpty(currentJob);

        if(hasJob && scheduleScript.periodCode == "W" && !isShowing)
        {
            ShowQuotaPanel();
        }
        else if(isShowing && (!hasJob || scheduleScript.periodCode != "W"))
        {
            HideQuotaPanel();
        }
    }
    private void ShowQuotaPanel()
    {
        isShowing = true;
        ic.Find("QuotaPanel").gameObject.SetActive(true);
    }
    private void HideQuotaPanel()
    {
        isShowing = false;
        ic.Find("QuotaPanel").gameObject.SetActive(false);
        doneWithJob = false;

        //reset bar
        ic.Find("QuotaPanel").Find("BarLine").localPosition = new Vector2(50, 37.5f);
        ic.Find("QuotaPanel").Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 25);
    }
    public void AddToQuota(float num, float denom)
    {
        if(doneWithJob == true)
        {
            return;
        }
        
        //stretch bar to size
        //divide the size by 2 and make that its pos
        float maxWidth = 280;
        float size = (maxWidth * num) / denom;
        GameObject bar = ic.Find("QuotaPanel").Find("BarLine").gameObject;
        if (bar.GetComponent<RectTransform>().sizeDelta.x + size > maxWidth)
        {
            bar.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, 25);
        }
        else
        {
            bar.GetComponent<RectTransform>().sizeDelta += new Vector2(size, 0);
        }
        bar.transform.localPosition = new Vector2((bar.GetComponent<RectTransform>().sizeDelta.x / 2f) + 50f, 37.5f);

        if (bar.GetComponent<RectTransform>().sizeDelta.x == maxWidth && !doneWithJob)
        {
            doneWithJob = true;
            GiveJobMoney();
        }
    }
    private void GiveJobMoney()
    {
        StartCoroutine(FlashQuotaBar());

        //get payout
        int mult = 1;
        int basePay = jobPayDict[currentJob];
        switch (currentMap.npcLevel)
        {
            case 1:
                mult = 4;
                break;
            case 2:
                mult = 3;
                break;
            case 3:
                mult = 2;
                break;
        }

        player.GetComponent<PlayerCollectionData>().playerData.money += mult * basePay;
    }
    private IEnumerator FlashQuotaBar()
    {
        //.18
        ic.Find("QuotaPanel").GetComponent<Image>().sprite = quotaSprite1;
        yield return new WaitForSeconds(.18f);
        ic.Find("QuotaPanel").GetComponent<Image>().sprite = quotaSprite2;
        yield return new WaitForSeconds(.18f);
        ic.Find("QuotaPanel").GetComponent<Image>().sprite = quotaSprite3;
        yield return new WaitForSeconds(.18f);
        ic.Find("QuotaPanel").GetComponent<Image>().sprite = quotaSprite4;
    }
}
