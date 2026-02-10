using UnityEngine;

[System.Serializable]
public class Mission
{
    public string type; //inmateBeat, guardBeat, distract, give, stealInmateBody, stealDesk, stealGuardBody
    public int item;
    public string giver;
    public string target;
    public string message;
    public string period;
    public int pay;

    public Mission(string type, int item, string giver, string target, string message, string period, int pay)
    {
        this.type = type;
        this.item = item;
        this.giver = giver;
        this.target = target;
        this.message = message;
        this.period = period;
        this.pay = pay;
    }
}
