using UnityEngine;

public class Mission
{
    public string type;
    public int item;
    public string giver;
    public string target;
    public string message;

    public Mission(string type, int item, string giver, string target, string message)
    {
        this.type = type;
        this.item = item;
        this.giver = giver;
        this.target = target;
        this.message = message;
    }
}
