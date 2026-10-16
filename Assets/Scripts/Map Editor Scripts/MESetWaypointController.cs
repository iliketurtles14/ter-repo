using TMPro;
using UnityEngine;

public class MESetWaypointController : MonoBehaviour
{
    public Transform currentWaypoint;
    public Transform uic;
    public void SetNums()
    {
        uic.Find("SetWaypointPanel").Find("IDNum").GetComponent<TMP_InputField>().text = currentWaypoint.GetComponent<MESetWaypointContainer>().id.ToString();
        uic.Find("SetWaypointPanel").Find("OrderNum").GetComponent<TMP_InputField>().text = currentWaypoint.GetComponent<MESetWaypointContainer>().order.ToString();
    }
}