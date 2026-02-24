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
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        favorMenuScript = mc.Find("FavorMenuPanel").GetComponent<FavorMenu>();
        missionAskScript = mc.Find("MissionPanel").GetComponent<MissionAsk>();
        givingScript = mc.Find("NPCGiveMenuPanel").GetComponent<Giving>();
        npcIDInvScript = mc.Find("NPCMenuPanel").GetComponent<NPCIDInv>();
        shopMenuScript = mc.Find("NPCShopMenuPanel").GetComponent<ShopMenu>();
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
}
