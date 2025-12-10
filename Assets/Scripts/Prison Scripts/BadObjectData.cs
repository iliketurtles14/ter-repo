using UnityEngine;

public class BadObjectData : MonoBehaviour
{
    public bool isMultiplied = false; //multiplied by the map difficulty
    public int heatGain = 0;
    public int heatSet = -1; //this is for immediately just setting the heat to 99
    public bool shouldAggro = false;
    public bool solitary = false;
    public bool item = false; //if true, it will pick up the item
    public bool toilet = false; //if true, it will search and unclog the toilet
    public bool untie = false; //if true, it will untie the seen npc
    public bool sheets = false; //if true, it will take down the seen sheets from the bars
    public string messageType = null; //if not null, it will make the guard say a message
    public bool shouldCall = false; //if true, this will make the inmate call a guard to this position.
    public GameObject attachedObject;
    public bool forInmate; //if this is true, this bad object will only do something if an inmate sees it
}
