using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PButtonController : MonoBehaviour
{
    private MissionAsk missionAskScript;
    private FavorMenu favorMenuScript;
    private Giving givingScript;
    private Transform mc;
    private NPCIDInv npcIDInvScript;
    private ShopMenu shopMenuScript;
    private JobMenu jobMenuScript;
    private CraftMenu craftMenuScript;
    private PlayerIDInv playerIDInvScript;
    private ToiletMenu toiletMenuScript;
    private PayphoneMenu payphoneMenuScript;
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        favorMenuScript = mc.Find("FavorMenuPanel").GetComponent<FavorMenu>();
        missionAskScript = mc.Find("MissionPanel").GetComponent<MissionAsk>();
        givingScript = mc.Find("NPCGiveMenuPanel").GetComponent<Giving>();
        npcIDInvScript = mc.Find("NPCMenuPanel").GetComponent<NPCIDInv>();
        shopMenuScript = mc.Find("NPCShopMenuPanel").GetComponent<ShopMenu>();
        jobMenuScript = mc.Find("JobMenuPanel").GetComponent<JobMenu>();
        craftMenuScript = mc.Find("CraftMenuPanel").GetComponent<CraftMenu>();
        playerIDInvScript = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
        toiletMenuScript = mc.Find("ToiletMenuPanel").GetComponent<ToiletMenu>();
        payphoneMenuScript = mc.Find("PayphoneMenuPanel").GetComponent<PayphoneMenu>();
    }
    public void MissionYes()
    {
        missionAskScript.SaveMission(missionAskScript.currentNPC);
    }
    public void MissionNo()
    {
        missionAskScript.CancelMission(missionAskScript.currentNPC);
    }
    public void MissionMaybe()
    {
        missionAskScript.MaybeMission();
    }
    public void PlayerIDFavor()
    {
        Debug.Log("pressed!");
        favorMenuScript.OpenMenu();
    }
    public void FavorCancel(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if(pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        GameObject clickedMission = clicked.transform.parent.gameObject;
        Transform contentTransform = clickedMission.transform.parent;

        int index = 0;
        int i = 0;
        foreach(Transform mission in contentTransform)
        {
            if(mission.name == "Mission" && mission.gameObject == clickedMission)
            {
                index = i;
            }
            if(mission.name != "PlaceholderMission")
            {
                i++;
            }
        }

        missionAskScript.savedMissions.RemoveAt(index);

        Destroy(clickedMission);
    }
    public void FavorPlayerID()
    {
        favorMenuScript.CloseMenu(true);
    }
    public void Give()
    {
        givingScript.Give();
    }
    public void NPCIDGive()
    {
        StartCoroutine(npcIDInvScript.CloseMenu(true, false));
    }
    public void GiveNPCID()
    {
        StartCoroutine(givingScript.CloseMenu(true, false));
    }
    public void NPCIDShop()
    {
        StartCoroutine(npcIDInvScript.CloseMenu(false, true));
    }
    public void GiveShop()
    {
        StartCoroutine(givingScript.CloseMenu(false, true));
    }
    public void ShopNPCID()
    {
        StartCoroutine(shopMenuScript.CloseMenu(true, false));
    }
    public void ShopGive()
    {
        StartCoroutine(shopMenuScript.CloseMenu(false, true));
    }
    public void JobApply(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        string job = clicked.name;
        jobMenuScript.Apply(job);
    }
    public void JobResign(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        string job = clicked.name;
        jobMenuScript.Resign(job);
    }
    public void JobBack()
    {
        jobMenuScript.GoToAllJobView();
    }
    public void JobView(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;
        string job = clicked.name;
        jobMenuScript.OpenJobDescription(job);
    }
    public void InvCraft()
    {
        craftMenuScript.OpenMenu();
    }
    public void InvID()
    {
        StartCoroutine(playerIDInvScript.OpenMenu(false));
    }
    public void CraftCraft()
    {
        craftMenuScript.Craft();
    }
    public void CraftNotes()
    {
        Debug.Log("go to notes");
    }
    public void ToiletFlush()
    {
        toiletMenuScript.Flush();
    }
    public void PayphoneTip(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        Debug.Log("(before): " + clicked.name);
        
        if (clicked.name != "Tip1Button" && clicked.name != "Tip2Button" && clicked.name != "Tip3Button") //actual button (ts would hit the text object and not the button)
        {
            clicked = clicked.transform.parent.gameObject;
        }

        Debug.Log(clicked.name);

        string buttonText = clicked.transform.Find("Text").GetComponent<TextMeshProUGUI>().text;
        if(buttonText == "Unlocked")
        {
            string hint = null;
            switch (clicked.name)
            {
                case "Tip1Button":
                    hint = payphoneMenuScript.tip1;
                    break;
                case "Tip2Button":
                    hint = payphoneMenuScript.tip2;
                    break;
                case "Tip3Button":
                    hint = payphoneMenuScript.tip3;
                    break;
            }
            payphoneMenuScript.GoToTip(hint);
        }
        else
        {
            int hintNum = -1;
            switch (clicked.name)
            {
                case "Tip1Button":
                    hintNum = 1;
                    break;
                case "Tip2Button":
                    hintNum = 2;
                    break;
                case "Tip3Button":
                    hintNum = 3;
                    break;
            }
            payphoneMenuScript.BuyTip(hintNum);
        }
    }
    public void PayphoneReturn()
    {
        payphoneMenuScript.ReturnFromTip();
    }
}
