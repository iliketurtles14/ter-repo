using UnityEngine;
using UnityEngine.EventSystems;

public class PButtonController : MonoBehaviour
{
    private MissionAsk missionAskScript;
    private FavorMenu favorMenuScript;
    private void Start()
    {
        favorMenuScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("FavorMenuPanel").GetComponent<FavorMenu>();
        missionAskScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("MissionPanel").GetComponent<MissionAsk>();
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
}
