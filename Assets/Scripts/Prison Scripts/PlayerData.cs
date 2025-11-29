using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
[CreateAssetMenu(menuName = "Player Data")]

public class PlayerData : ScriptableObject
{
    public int health;
    public int energy;
    public int money;
    public int heat;
    public int strength;
    public int speed;
    public int intellect;
    public string displayName;
    public bool hasFood;
    public bool isDead;
}
